using UnityEngine;
using System.Collections;

public class DotAnimation : MonoBehaviour {

	public Vector2[] defaultPositions;
    public float lifeTime = 1.5f;
    public float beginTime = 0.2f;
	float velocity = 1.0f; //block per second.

    public float alphaMultiplyer = 1.0f;

    const float fadeTime = 1.5f;
    float createTime;
    float[] timePoints;

	// Use this for initiaclization
	public void Start () {
        createTime = Time.time;
		transform.localPosition = new Vector3(defaultPositions[0].x, defaultPositions[0].y);
        setAlpha(0);
        //Build the lookup.
        timePoints = new float[defaultPositions.Length];
        timePoints[0] = 0;
        for (int i = 1; i < defaultPositions.Length; ++i) {
            float dx = defaultPositions[i].x - defaultPositions[i - 1].x;
            float dy = defaultPositions[i].y - defaultPositions[i - 1].y;
            float dist = Mathf.Sqrt(dx * dx + dy * dy);
            timePoints[i] = timePoints[i - 1] + dist;
        }
        velocity = timePoints[defaultPositions.Length - 1] / lifeTime;
        for (int i = 0; i < timePoints.Length; ++i) {
            timePoints[i] /= velocity;
        }
	}

    // Update is called once per frame
    void Update() {
        float dt = Mathf.Max(0, Time.time - createTime - beginTime);
        setAlpha(Mathf.Min(1.0f, dt * 3.0f));

        //Raw loop to find appropriate time point. (BiSearch is too mendokusai!)
        int result = timePoints.Length;
        for (int i = 0; i < timePoints.Length; ++i) {
            if (timePoints[i] > dt) {
                result = i;
                break;
            }
        }

        if (result == timePoints.Length) { //Blend.
            float blendTime = dt - timePoints[timePoints.Length - 1];
            float alpha = Mathf.Max(0.0f, 1 - blendTime * fadeTime);
            if (alpha == 0) {
                Destroy(gameObject);
            }
            transform.localPosition = defaultPositions[timePoints.Length - 1];
            setAlpha(alpha);
        } else { //Lerp the value.
            Vector3 a = defaultPositions[result - 1], b = defaultPositions[result];
            float factor = (dt - timePoints[result - 1]) / (timePoints[result] - timePoints[result - 1]);
            transform.localPosition = lerp(a, b, 1 - factor);
        }
	}

    private Vector3 toVec3(Vector2 v2) {
        return new Vector3(v2.x, v2.y, 0);
    }

    private Vector3 lerp(Vector2 a, Vector2 b, float f) {
        float mf = 1 - f;
        return new Vector3(a.x * f + b.x * mf, a.y * f + b.y * mf, 0);
    }

    private void setAlpha(float f) {
        GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, alphaMultiplyer * f);
    }
}
