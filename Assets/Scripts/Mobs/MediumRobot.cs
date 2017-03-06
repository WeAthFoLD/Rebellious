using UnityEngine;
using System.Collections;

public class MediumRobot : MonoBehaviour {

    public AudioClip chargeSound;

    Object bulletLarge;

    GameObject scene;
    GameObject player;

    float floatFreq = 2;
    float closingSpeed;

    float watchRotation;

    float lastShootTime;
    float shootWait;

    Vector3 shootPosition { get { return transform.localPosition + new Vector3(-0.5f, 0f); } }

    // Use this for initialization
    void Start() {
        floatFreq += Random.Range(-0.5f, 0.5f);
        closingSpeed = Random.Range(3f, 4f);
        watchRotation = Random.Range(-30f, 30f);

        scene = GameObject.FindWithTag("Scene");
        player = GameObject.FindWithTag("Player");

        bulletLarge = Resources.Load("Prefabs/Mobs/large_bullet");
    }

    // Update is called once per frame
    void Update() {
        { //Update position
            float yOffset = Mathf.Sin(Time.time * floatFreq) * 0.1f * Time.deltaTime;
            transform.localPosition += Vector3.up * yOffset;

            Vector3 delta = player.transform.localPosition - transform.localPosition - Vector3.up * 0.2f;
            Vector3 closeVector = delta;
            //closeVector.y *= 5.0f;
            closeVector = closeVector.normalized;

            float factor = Mathf.Min(1.0f, 0.4f * Mathf.Sqrt(delta.magnitude));
            closeVector = Quaternion.AngleAxis(watchRotation * factor, Vector3.forward) * closeVector;

            transform.localPosition += closeVector * closingSpeed * Time.deltaTime;
        }

        if(Utils.inScene(gameObject)) { //Update shoot
            float dt = Time.time - lastShootTime;
            if(dt >= shootWait) {
                Object prefab = bulletLarge;
                GameObject obj = Instantiate(prefab, shootPosition, Quaternion.identity) as GameObject;
                obj.transform.parent = scene.transform;

                lastShootTime = Time.time;
                shootWait = Random.value < 0.1f ? Random.Range(4f, 5f) : Random.Range(1f, 1.2f);
            }
        }

        if(isCharging) {
            transform.localPosition = Utils.lerp(chargeStart, chargeDest, (Time.time - startChargeTime) / CHARGE_TIME);
        } else if(canCharge) {
            hasCharged = true;

            //Start charging
            Vector3 lookVec = (player.transform.localPosition - transform.localPosition).normalized;
            float chargeDistance = Random.Range(3f, 4f);
            RaycastHit2D result = Physics2D.Raycast(transform.localPosition, lookVec, chargeDistance, LayerMask.GetMask("Ground"));
            if(result.collider != null) {
                chargeStart = result.centroid;
            } else {
                
            }
            chargeDest = transform.localPosition + lookVec * chargeDistance;

            chargeStart = transform.localPosition;
            startChargeTime = Time.time;

            GetComponent<AudioSource>().PlayOneShot(chargeSound);
        }

        GetComponent<Animator>().SetBool("attacking", isCharging);
    }

    void MobDead() {
        Destroy(this);
    }                 

    const float CHARGE_TIME = 0.3f;
    float startChargeTime;
    Vector3 chargeStart, chargeDest;
    bool hasCharged = false;

    bool isCharging { get { return Time.time - startChargeTime <= CHARGE_TIME && hasCharged; } }
    bool canCharge { 
        get {
            float distance = (player.transform.localPosition - transform.localPosition).magnitude;
            //print(distance);
            return !hasCharged && distance < 5f && distance > 0.4f && Random.value < 0.3f;
        } 
    }

}
