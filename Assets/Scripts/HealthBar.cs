using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HealthBar : MonoBehaviour {
    public RectTransform healthBar;
    public TextMeshProUGUI healthText;
    public FloatValue currentHealth;
    public float minWidth;

    float max;
    float min;
    float maxWidth;
    float currentValue;
    float currentPercent;
    Animator mainCameraAnimator;
    Animator redScreenAnimator;

    private void Start() {
        max = currentHealth.initialValue;
        min = 0;
        maxWidth = healthBar.sizeDelta.x;
        UpdateHealth();

        mainCameraAnimator = GameObject.Find("Main Camera").GetComponent<Animator>();
        redScreenAnimator = GameObject.Find("RedScreen").GetComponent<Animator>();
    }

    public void UpdateHealth() {
        if (max == min) {
            currentValue = 0;
            currentPercent = 0;
        }
        else {
            if (currentHealth.runtimeValue < currentValue) {
                PlayCameraHitEffect();
            }
            currentValue = currentHealth.runtimeValue;
            currentPercent = currentValue / (max - min);
        }
        healthText.SetText(string.Format("{0}%", Mathf.RoundToInt(currentPercent * 100)));
        healthBar.sizeDelta = new Vector2(maxWidth * currentPercent, healthBar.sizeDelta.y);
    }

    public void PlayCameraHitEffect() {
        redScreenAnimator.transform.localScale = new Vector3(1,1,1);
        if (mainCameraAnimator == null || redScreenAnimator == null) {
            Debug.LogError("Animator for Camera Hit Effect Missing");
        }

        redScreenAnimator.SetTrigger("wasHit");
        mainCameraAnimator.SetTrigger("shake");
    }
}
