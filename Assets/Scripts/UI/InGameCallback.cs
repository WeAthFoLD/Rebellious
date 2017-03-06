using UnityEngine;
using System.Collections;

public class InGameCallback : MonoBehaviour {

	public void EnterMainMenu() {
        Application.LoadLevelAsync("MainMenu");
    }

    void Start() {
        Time.timeScale = 1;
    }
}
