using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MainMenuUI : MonoBehaviour {

    Animator animator;
    Animator settingsAnimator;

    AudioClip clip;

	void Start () {
	    animator = GetComponent<Animator>();
        settingsAnimator = GameObject.Find("SettingsUI").GetComponent<Animator>();

        clip = Resources.Load<AudioClip>("Sounds/UI/buttonhit");
        Time.timeScale = 1.0f;
	}

    public void startTheGame() {
        animator.SetBool("pressed_loading", true);
        Application.LoadLevelAsync("Game");
    }

    public void onPauseDown() {
        animator.SetBool("opening", false);
        settingsAnimator.SetBool("opening", true);
    }

    public void onPauseQuit() {
        animator.SetBool("opening", true);
        settingsAnimator.SetBool("opening", false);
    }

    public void PlaySound() {
        GameObject.FindWithTag("MainCamera").GetComponent<AudioSource>().PlayOneShot(clip);
    }
}
