using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour {
    public Animator redScreenAnimator;
    public Animator mainCameraAnimator;
    public GameObject turretsContainer;

    [Header("Points")]
    public FloatValue playerPoints;
    public TextMeshProUGUI pointsText;

    [Header("Countdown")]
    public Canvas countdown;
    public EnemySpawner enemySpawner;

    [Header("Pause Menu")]
    public Canvas menu;
    public KeyCode menuKey = KeyCode.Escape;


    private bool menuOpen = false;
    private TextMeshProUGUI countdownText;


    private void Start() {
        countdownText = countdown.transform.Find("Time").GetComponent<TextMeshProUGUI>();
    }

    private void Update() {
        if (Input.GetKeyDown(menuKey)) {
            if (menuOpen) {
                menu.GetComponent<Animator>().SetTrigger("hide");
                Time.timeScale = 1;
            } else {
                menu.GetComponent<Animator>().SetTrigger("show");
                Time.timeScale = 0;
            }
            menuOpen = !menuOpen;
            GameManager.instance.SetPlayerShooting(!menuOpen);
        }
    }

    public void UpdatePoints() {
        pointsText.GetComponent<Animator>().SetTrigger("scale");
        pointsText.GetComponent<TextMeshProUGUI>().SetText(playerPoints.runtimeValue.ToString());

        foreach (Transform child in turretsContainer.transform) {
            TurretUI ui = child.gameObject.GetComponent<TurretUI>();
            if (ui != null)
                ui.UpdatePoints((int) playerPoints.runtimeValue);
        }
    }

    public void UpdateCountdown() {
        if (!enemySpawner || !countdownText)
            return;

        float timeLeft = enemySpawner.countdown;

        if (timeLeft > 0f) {
            countdown.enabled = true;
            countdownText.SetText(string.Format("{0:00.00}s", timeLeft));
        } else {
            countdown.enabled = false;
        }

    }

    public void UpdateLocks() {
        foreach (Transform child in turretsContainer.transform) {
            TurretUI tui = child.gameObject.GetComponent<TurretUI>();
            if (tui != null) {
                tui.UpdateLock();
            }
        }
    }

}
