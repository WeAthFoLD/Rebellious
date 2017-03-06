using UnityEngine;
using System.Collections;

public class FadeIn : MonoBehaviour {

    public float timeOffset = 0.0f;
    public float maxAlpha = 1.0f;
    public float time = 1.0f;

    float startTime;
    bool end = false;

    SpriteRenderer spriteRenderer;

	void Start () {
        startTime = Time.time;
        spriteRenderer = GetComponent<SpriteRenderer>();
	}
	
	void Update () {
        float dt = Mathf.Max(0.0f, Time.time - startTime - timeOffset);
        if(dt <= time) {
            changeAlpha(maxAlpha * dt / time);
        } else if(!end) {
            end = true;
            changeAlpha(maxAlpha);
        }
	}

    private void changeAlpha(float a) {
        spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, a);
    }
}
