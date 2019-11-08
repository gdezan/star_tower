using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonHandler : MonoBehaviour {
    public void Exit() {
        Application.Quit();
    }

    public void ChangeScene(string sceneName) {
        SceneManager.LoadScene(sceneName);
        Time.timeScale = 1;
    }
}
