using UnityEngine;
using System.Collections;

public class BotParticle : MonoBehaviour {

    SpriteRenderer render;

    float life;

    Vector3 velocity;
    float spawn;

	void Start () {
        velocity = Vector3.down * Random.Range(0.15f, 0.2f);
        spawn = Time.time;

        life = Random.Range(1, 2);

        render = GetComponent<SpriteRenderer>();
	}
	
	void Update () {
        float dt = Time.time - spawn;
        if(dt > life) {
            Destroy(gameObject);
        }

        render.color = new Color(1, 1, 1, 1.0f - dt / life);

        transform.localPosition += velocity * Time.deltaTime;
	}
}
