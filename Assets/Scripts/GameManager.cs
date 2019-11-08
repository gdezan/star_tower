using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
    public static GameManager instance = null;
    public GameObject constructionPrefab;
    public TurretProps[] turrets;
    public GunProps[] guns;
    public int currentWave = 0;
    public bool playerWon;

    public FloatValue playerHealth;
    public FloatValue homeBaseHealth;
    public FloatValue playerScore;

    [Header("Points")]
    public FloatValue playerPoints;
    public Signal playerPointsSignal;

    private float currentTimeScale = 1;

    private void Awake() {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        //DontDestroyOnLoad(gameObject);
    }

    private void Update() {
        if (currentTimeScale == 1 && Time.timeScale == 0) {
            currentTimeScale = 0;
            GetComponent<AudioSource>().Pause();
        } else if (currentTimeScale == 0 && Time.timeScale == 1) {
            currentTimeScale = 1;
            GetComponent<AudioSource>().Play();
        }
    }

    public void SetUpConstruction(int[] tile, Vector3 position, int turretId) {
        playerPoints.runtimeValue -= turrets[turretId].cost;
        playerPointsSignal.Raise();

        GameObject construction = Instantiate(constructionPrefab, position, Quaternion.identity);
        construction.GetComponent<Construction>().TurretId = turretId;

        LevelManager.instance.indexMatrix[tile[1], tile[0]] = 3;
    }

    public void CreateTurret(Vector3 position, int turretId) {
        playerPointsSignal.Raise();
        Instantiate(turrets[turretId].turretPrefab, position, Quaternion.identity);
    }

    public void SetPlayerShooting(bool enabled) {
        Transform player = GameObject.Find("Player").transform;
        foreach (Transform child in player.Find("Hand").transform) {
            child.GetComponent<Shooting>().shootingEnabled = enabled;
        }
    }

    public void GameOver() {
        CalculatePoints(false);
        SceneManager.LoadScene("GameOver");
    }

    public void WinGame() {
        CalculatePoints(true);
        SceneManager.LoadScene("GameWin");
    }

    void CalculatePoints(bool gameWon) {
        // 300 points for winning the game
        float points = gameWon ? 300 : 0;

        // Points for wave progression
        points += currentWave * 20;

        // Points for health
        if (playerHealth.runtimeValue > 0)
            points += playerHealth.runtimeValue / playerHealth.initialValue * 30;
        if (homeBaseHealth.runtimeValue > 0)
            points += homeBaseHealth.runtimeValue / homeBaseHealth.initialValue * 100;

        // Points for cogs in the bank
        points += playerPoints.runtimeValue / 5;

        playerScore.runtimeValue = points;
    }
}
