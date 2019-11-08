using UnityEngine;
using System.Collections;


public class Loader : MonoBehaviour {
    public GameObject gameManager;
    public LevelManager levelManager;

    void Awake() {
        if (GameManager.instance == null)
            Instantiate(gameManager);

        if (LevelManager.instance == null)
            Instantiate(levelManager);
    }
}