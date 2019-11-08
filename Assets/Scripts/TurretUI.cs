using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TurretUI : MonoBehaviour {
    public CursorManager cursorManager;
    public TurretProps props;
    public TextMeshProUGUI costText;
    public int waveToUnlock;

    Image turretImage;

    private void Start() {
        turretImage = transform.Find("Image").GetComponent<Image>();
        costText.SetText(props.cost.ToString());
        UpdatePoints(0);
    }

    public void UpdatePoints(int points) {
        Button button = GetComponent<Button>();
        if (points >= props.cost) {
            button.enabled = true;
            turretImage.color = Color.white;
        } else {
            button.enabled = false;
            turretImage.color = Color.gray;
        }
    }

    public void SetTurretCursor() {
        if (GetComponent<Button>().enabled) {
            cursorManager.EnableTurretCursor(props.id);
        }
    }

    public void UpdateLock() {
        if (GameManager.instance.currentWave >= waveToUnlock) {
            Transform locked = transform.Find("Locked");
            if (locked != null) {
                locked.GetComponent<Image>().enabled = false;
            }
        }
    }
}
