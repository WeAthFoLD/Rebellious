using UnityEngine;
using System.Collections;

public class FloatPlatform : MonoBehaviour {

    float shakeSpeed = 0;
    float shakeAmp = 0;

    float creationTime;
    float initY;

	void Start () {
        initY = transform.localPosition.y;
        creationTime = Time.time;

        if(Random.Range(0f, 10f) > 5f) {
            shakeSpeed = Random.Range(1f, 2f);
            shakeAmp = Random.Range(0.2f, 0.4f);
        } else {
            Destroy(this);
        }
	}
	
	void Update () {
        float y = initY + shakeAmp * Mathf.Sin((Time.time - creationTime) * shakeSpeed);
        transform.localPosition = new Vector3(transform.localPosition.x, y, 10);
	}
}
