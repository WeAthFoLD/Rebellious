using UnityEngine;
using System.Collections;

public class SmallBullet : MonoBehaviour {

    float velocity;

	// Use this for initialization
	void Start () {
        velocity = Random.Range(10f, 12f);
	}
	
	// Update is called once per frame
	void Update () {
        transform.localPosition += Vector3.left * velocity * Time.deltaTime;
	}

    void OnTriggerEnter2D(Collider2D other) {
        GameObject target = other.gameObject;
        if (target.tag.Equals("Player")) {
            target.GetComponent<Controller>().Attack();
            Destroy(gameObject);
        }
    }
}
