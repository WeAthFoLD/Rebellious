using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ScoreDrawer : MonoBehaviour {

    public GameObject record, newRecord;

    const float inTime = .7f;

    new public Camera camera;

    ScoreBoard scoreBoard;
    NumbersDrawer drawer;

    float startTime;

	void Start () {
        drawer = new NumbersDrawer(NumbersDrawer.Align.CENTER);
        scoreBoard = GameObject.Find("Scene").GetComponent<ScoreBoard>();
        startTime = Time.unscaledTime;

        if(scoreBoard.score >= Utils.highScore) {
            Destroy(record);
        } else {
            Destroy(newRecord);
        }
	}

	void Update () {
        Vector2 pos = camera.transform.localPosition;

        float udt = Time.unscaledTime - startTime;
        
        drawer.setNumber((int) (Mathf.Min(1.0f, udt / inTime) * scoreBoard.score));
        drawer.draw(pos.x + 0.2F, pos.y - 0.4f, 0.012f);

        if(udt > inTime + 1.3f) {
            drawer.setNumber(Utils.highScore);
            drawer.draw(pos.x + 1f, pos.y - 1.48f, 0.006f); 
        }
	}
}
