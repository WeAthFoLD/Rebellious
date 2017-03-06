using UnityEngine;

using System.Collections;

public class CameraController : MonoBehaviour {

    float playerOffset;

    GameObject player;
    Controller controller;

    float velocity;

	void Start () {
        player = GameObject.FindWithTag("Player");
        controller = player.GetComponent<Controller>();
        playerOffset = player.transform.localPosition.x - transform.localPosition.x;

	}

	void Update () {
        if(!controller.enabled)
            return;
        float dx = transform.localPosition.x - player.transform.localPosition.x + playerOffset;

        if(dx <= 0) {
            velocity = balance(velocity, controller.maxSpeed, 10 * Time.deltaTime);
        } else { //KeepUp
            float keepupSpeed = controller.cameraSpeed * 0.7f;
            velocity = balance(velocity, controller.maxSpeed * 0.7f, 10 * Time.deltaTime);
        }

        //if(transform.localPosition.x < player.transform.localPosition.x)
            transform.localPosition += Vector3.right * velocity * Time.deltaTime;
	}

    private float balance(float a, float b, float max) {
        float delta = b - a;
        delta = Mathf.Min(max, Mathf.Abs(delta)) * Mathf.Sign(delta);
        return a + delta;
    }

    void OnTriggerEnter2D(Collider2D collider) {
        if(collider.gameObject == player) {
            controller.Attack();
        }
    }

    void PlayerDead() {
        StartCoroutine("FadeOut");
    }

    IEnumerator FadeOut() {
        AudioSource src = GetComponent<AudioSource>();
        for(; ; ) {
            src.volume -= 1f * Time.deltaTime;
            if(src.volume <= 0) {
                src.Stop();
            }
            yield return null;
        }
    }

}
