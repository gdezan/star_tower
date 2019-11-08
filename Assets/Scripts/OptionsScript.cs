using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class OptionsScript : MonoBehaviour
{

    public AudioMixer audioMixer;
    public Slider slider;
    public TextMeshProUGUI scoreText;
    public FloatValue score;

    private void Start() {
        scoreText.SetText($"Pontos: {(int) score.runtimeValue}");
    }

    public void SetVolume () 
    {
        audioMixer.SetFloat("volume", Mathf.Log10(slider.value) * 20);
    }
}
