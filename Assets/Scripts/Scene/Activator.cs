using UnityEngine;
using UnityEngine.Audio;
using System.Collections;

public class Activator : MonoBehaviour {

    public AudioMixerSnapshot muted;
    public AudioMixerGroup master;

    public float time = 1.0f;

    float startTime;

	// Use this for initialization
	void Start () {
        startTime = Time.time;

        Time.timeScale = 1.0f;
        
        if(!Utils.sound) {
            master.audioMixer.TransitionToSnapshots(
                new AudioMixerSnapshot[] { muted }, new float[] { 1.0f }, 0);
        }
    }
	
	// Update is called once per frame
	void Update () {
        float dt = Time.time - startTime;
        if(dt >= time) {
            gameObject.GetComponent<Controller>().enabled = true;
            gameObject.GetComponent<Rigidbody2D>().isKinematic = false;
            Destroy(this);
        }
	}
}
