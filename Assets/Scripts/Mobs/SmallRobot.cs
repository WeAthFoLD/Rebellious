using UnityEngine;
using System.Collections;

public class SmallRobot : MonoBehaviour {

    Object bulletSmall;

    GameObject scene;
    GameObject player;

    float floatFreq = 2;
    float closingSpeed;

    float watchRotation;

    float lastShootTime;
    float shootWait;

    Vector3 shootPosition { get { return transform.localPosition + new Vector3(-0.1f, 0.4f); } }

	// Use this for initialization
	void Start () {
        floatFreq += Random.Range(-0.5f, 0.5f);
        closingSpeed = Random.Range(1.6f, 2f);
        watchRotation = Random.Range(-30f, 30f);

        scene = GameObject.FindWithTag("Scene");
        player = GameObject.FindWithTag("Player");

        bulletSmall = Resources.Load("Prefabs/Mobs/small_bullet");
	}
	
	// Update is called once per frame
    void Update() {
        { //Update position
            float yOffset = Mathf.Sin(Time.time * floatFreq) * 0.1f * Time.deltaTime;
            transform.localPosition += Vector3.up * yOffset;

            Vector3 delta = player.transform.localPosition - transform.localPosition - Vector3.up * 0.2f;
            Vector3 closeVector = delta;
            if(closeVector.magnitude > 0.5f) {
                //closeVector.y *= 5.0f;
                closeVector = closeVector.normalized;

                float factor = Mathf.Min(1.0f, 0.4f * Mathf.Sqrt(delta.magnitude));
                closeVector = Quaternion.AngleAxis(watchRotation * factor, Vector3.forward) * closeVector;

                transform.localPosition += closeVector * closingSpeed * Time.deltaTime;
            }
        }

        if(Utils.inScene(gameObject)) { //Update shoot
            float dt = Time.time - lastShootTime;
            if(dt >= shootWait) {
                Object prefab = bulletSmall;
                GameObject obj = Instantiate(prefab, shootPosition, Quaternion.identity) as GameObject;
                obj.transform.parent = scene.transform;

                lastShootTime = Time.time;
                shootWait = Random.value < 0.1f ? Random.Range(4f, 5f) : Random.Range(1f, 1.2f);
            }
        }
    }

    void MobDead() {
        Destroy(GetComponent<SpriteRenderer>());
        Destroy(this);
    }                 
}
