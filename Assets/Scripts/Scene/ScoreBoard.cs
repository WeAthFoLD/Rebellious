using UnityEngine;
using System.Collections;

public class ScoreBoard : MonoBehaviour {

    Controller player;

    public int score { get { return baseScore + additionalScore; } }

    public int baseScore {
        get { return (int)Mathf.Max(0, (player.travelDistance * 2)); }
    }

    private int additionalScore;

    public void addScore(int score) {
        additionalScore += score;
    }

	void Start () {
        player = GameObject.FindWithTag("Player").GetComponent<Controller>();
	}
	
	void Update () {

	}

    void PlayerDead() {
        if(Utils.highScore < score)
            Utils.highScore = score;
    }
}
