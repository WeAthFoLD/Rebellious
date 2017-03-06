using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameUI : MonoBehaviour {

    Animator animator;

    bool paused { get { return animator.GetBool("Paused"); } set { animator.SetBool("Paused", value); } }
    bool dead { get { return animator.GetBool("Dead"); } set { animator.SetBool("Dead", value); } }

	void Start () {
        animator = GetComponent<Animator>();
	}

    public void Pause() {
        paused = true;
        Time.timeScale = 0.0f;
    }

    public void EndPause() {
        paused = false;
        Time.timeScale = 1.0f;
    }

    public void PlayerDead() {
        StartCoroutine("displayDeadUI");
    }

    public void RestartGame() {
        Time.timeScale = 1.0f;
        Application.LoadLevelAsync("Game");
    }

    public void BackToMainMenu() {
        Time.timeScale = 1.0f;
        Application.LoadLevelAsync("MainMenu");
    }

    IEnumerator displayDeadUI() {
        yield return new WaitForSeconds(1);
        Time.timeScale = 0.0f;
        dead = true;
    }
}
