using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour {
    public static int EnemiesAlive = 0;

    public int waveIndex = 0;
    public Wave[] waves;

    public Signal countdownSignal;
    public Signal waveSignal;

    public Transform enemiesParent;
    public bool isOn;

    public float countdown = 0.01f;

    private int spawned = 0;
    private bool inProgress;


    // Start is called before the first frame update
    void Start() {
        transform.position = LevelManager.instance.spawnPointLocation;
        countdownSignal.Raise();
        EnemiesAlive = 0;
    }

    // Update is called once per frame
    void Update() {
        if (!isOn) return;

        if (inProgress && EnemiesAlive == 0 && spawned == waves[waveIndex].count) {
            waveIndex++;
            waveSignal.Raise();

            if (waveIndex == waves.Length) {
                Invoke("WinGame", 2f);
            }
            inProgress = false;
        }

        if (inProgress) {
            return;
        }

        if (countdown <= 0f) {
            countdown = waves[waveIndex].timeTillNextWave;
            if (waves[waveIndex].timeTillNextWave == 0)
                countdownSignal.Raise();
            StartCoroutine(SpawnWave());
            return;
        }

        countdown -= Time.deltaTime;
        countdown = Mathf.Clamp(countdown, 0f, Mathf.Infinity);
        countdownSignal.Raise();
    }

    void WinGame() {
        GameManager.instance.WinGame();
        this.enabled = false;
    }

    IEnumerator SpawnWave() {
        Wave wave = waves[waveIndex];
        spawned = 0;
        inProgress = true;
        GameManager.instance.currentWave = waveIndex;

        for (int i = 0; i < wave.count; i++) {
            SpawnEnemy(wave.enemy);
            spawned++;
            yield return new WaitForSeconds(1 / wave.rate);
        }
    }

    void SpawnEnemy(GameObject enemy) {
        EnemiesAlive++;
        GameObject e = Instantiate(enemy, transform.position, Quaternion.identity);
        if (enemiesParent != null)
            e.transform.SetParent(enemiesParent);
    }
}
