using UnityEngine;
using System.Collections;

public class OverrideHint : MonoBehaviour {

    SpriteRenderer sprite;

    Controller player;

	void Start () {
        sprite = GetComponent<SpriteRenderer>();
        player = GameObject.FindWithTag("Player").GetComponent<Controller>();
	}
	
	void Update () {
        const float period = 0.7f;
        float progress = (Time.time % period) / period;
        float alpha = player.shootOverload == 1.0f ? Mathf.Sin(progress * Mathf.PI) : 0;

        sprite.color = new Color(1, 1, 1, alpha);
        const float from = 0.6f, to = 3.0f;
        transform.localPosition = new Vector3(to * progress + from * (1 - progress), 0);
	}
}
