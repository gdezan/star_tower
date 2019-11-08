using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class MainMenu : MonoBehaviour {
    public bool skipIntro = false;
    public VideoPlayer intro;
    public AudioClip cutsceneMusic;
    public TextMeshProUGUI skipText;

    void Start() {
        if (intro != null) {
            intro.loopPointReached += StartGame;
        }
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            StartGame();
        }
    }

    public void ExitGame() {
        Application.Quit();
    }

    public void NewGame() {
        if (skipIntro) {
            StartGame();
        } else {
            PlayIntro();
        }
    }

    void PlayIntro() {
        intro.Play();

        foreach(Transform child in transform) {
            child.gameObject.SetActive(false);
        }

        AudioSource source = GetComponent<AudioSource>();
        source.Stop();
        source.clip = cutsceneMusic;
        source.Play();
        skipText.gameObject.SetActive(true);

    }

    public void setSkipIntro(bool skip) {
        skipIntro = skip;
    }

    public void StartGame() {
        SceneManager.LoadScene("MainScene");
        Time.timeScale = 1;
    }
    public void StartGame(VideoPlayer vp) {
        SceneManager.LoadScene("MainScene");
        Time.timeScale = 1;
    }
}
