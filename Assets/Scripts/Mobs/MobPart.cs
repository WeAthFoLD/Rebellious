using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MobPart : MonoBehaviour {

    SpriteRenderer sr;

    bool attacked;
    float lastAttackTime = -1000;

    void Start() {
        sr = GetComponent<SpriteRenderer>();
    }

    void Update() {
        if(sr != null && attacked && Time.time - lastAttackTime > 0.5f) {
            attacked = false;
            sr.color = new Color(1, 1, 1, 1);
        }
    }

    void MobAttacked() {
        attacked = true;
        lastAttackTime = Time.time;

        sr.color = new Color(1, 0.5f, 0.5f, 1);
    }

    void MobDead() {
        Destroy(sr);
    }                 
}
