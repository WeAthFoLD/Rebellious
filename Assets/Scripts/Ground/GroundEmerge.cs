using UnityEngine;
using System.Collections;

public class GroundEmerge : MonoBehaviour {

    float startY;

    public float emergeTime = 2f;

    public float startHeight = -6f;

    float startTime;

	// Use this for initialization
	void Start () {
        startY = transform.localPosition.y;
        startTime = Time.time;
        transform.localPosition = new Vector3(transform.localPosition.x, startHeight);
	}
	
	// Update is called once per frame
	void Update () {
        float dt = Time.time - startTime;

        float y;
        if(dt > emergeTime) {
            y = startY;
            Destroy(this);
        } else {
            float lambda = dt / emergeTime;
            y = lerp(startHeight, startY, lambda);
        }

        transform.localPosition = new Vector3(transform.localPosition.x, y);
	}

    private float lerp(float a, float b, float l) {
        return a * (1 - l) + b * l;
    }
}
