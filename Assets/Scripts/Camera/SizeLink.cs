using UnityEngine;
using System.Collections;

public class SizeLink : MonoBehaviour {
    
    // Possibly bad practice

    Camera myCamera, mainCamera;

    void Start() {
        myCamera = GetComponent<Camera>();
        mainCamera = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
    }

	void Update () {
	    myCamera.orthographicSize = mainCamera.orthographicSize;
	}
}
