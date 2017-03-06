using UnityEngine;
using System.Collections;

public class PlayerBullet : MonoBehaviour {

    GameObject obj;

    Vector3 veldir;

    float initTime;

    float speedFactor;

    float speed { get { return speedFactor * (1.6f - 0.5f * Mathf.Exp(-Time.time + initTime)); } }

	void Start () {
        obj = findTarget();
        Vector3 veldir = new Vector3(1, 0);

        if(obj != null) {
            veldir = (obj.transform.localPosition - gameObject.transform.localPosition).normalized;
        }

        initTime = Time.time;

        speedFactor = 20 * Random.Range(0.6f, 0.8f);
	}
	
	void Update () {
        if(obj != null) {
            Vector3 dir = (obj.transform.localPosition - gameObject.transform.localPosition).normalized;
            veldir = new Vector3(balance(veldir.x, dir.x), balance(veldir.y, dir.y)).normalized;
        } else {
            veldir = new Vector3(1, 0); 
        }

        transform.localPosition += veldir * speed * Time.deltaTime;
	}

    void OnTriggerEnter2D(Collider2D other) {
        GameObject target = other.gameObject;
        if(target.tag.Equals("Mob")) {
            // TODO: Implement health system
            target.GetComponent<Mob>().applyDamage();
            var animator = GameObject.FindWithTag("MainCamera").GetComponent<Animator>();
            animator.Play("Shake");
        }
        if(target.tag.Equals("Mob") || target.tag.Equals("Ground"))
            Destroy(gameObject);
    }

    private float balance(float from, float to) {
        float MAX = 4 * Time.deltaTime;
        float delta = to - from;
        if(Mathf.Abs(delta) > MAX) {
            delta = Mathf.Sign(delta) * MAX;
        }
        return from + delta;
    }

    private GameObject findTarget() {
        return Utils.findNearestInFront(gameObject, "Mob");
    }
}
