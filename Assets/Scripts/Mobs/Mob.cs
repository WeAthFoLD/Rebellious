using UnityEngine;
using System.Collections;

public class Mob : MonoBehaviour {

    private AudioClip deathSound;

    public int startHealth = 1;

    int health;

    bool attacked;
    float lastAttackTime = -1;

    bool dead;

    void Start() {
        health = startHealth;
        deathSound = Resources.Load<AudioClip>("Sounds/Mob/death");
    }

    void Update() {
        if(attacked) {
            float dt = Time.time - lastAttackTime;
            if(dt > 0.3f) {
                attacked = false;
                //GetComponent<SpriteRenderer>().color = new Color(1, 1, 1);
            }
        }
    }

    public void applyDamage() {
        if(dead)
            return;
        --health;
        BroadcastMessage("MobAttacked");
        if(health <= 0) {
            dead = true;
            BroadcastMessage("MobDead");
            StartCoroutine("ReallyDead");
        } else {
            attacked = true;
            lastAttackTime = Time.time;
            //GetComponent<SpriteRenderer>().color = new Color(1, 0.5f, 0.5f);
        }
    }

    IEnumerator ReallyDead() {
        GetComponent<ParticleSystem>().Play();
        yield return new WaitForSeconds(2f);
        Destroy(gameObject);
    }

    void MobDead() {
        Destroy(GetComponent<Collider2D>());
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.AddForce(-rb.velocity, ForceMode2D.Impulse);
        GetComponent<AudioSource>().PlayOneShot(deathSound);
    }

    void OnTriggerEnter2D(Collider2D collider) {
        if(dead) return;
        GameObject player = Utils.player();
        if(collider.gameObject == player) {
            player.GetComponent<Controller>().Attack();
        }
    }

}
