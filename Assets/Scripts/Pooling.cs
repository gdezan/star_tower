using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pooling : MonoBehaviour {
    // Show in inspector
    [SerializeField]
    int amount;
    [SerializeField]
    GameObject bullet;
    [SerializeField]
    Transform gun, startingPoint;

    // private
    int nextBullet = 0;
    List<GameObject> bullets = new List<GameObject>();

    void Start() {
        // Populate pool of bullets
        for (int i = 0; i < amount; i++) {
            GameObject obj = (GameObject)Instantiate(bullet, gun);
            obj.SetActive(false);
            bullets.Add(obj);
        }
    }

    void Update() {
        if (Input.GetMouseButtonDown(0)) {
            Debug.Log("Shoot");
            Shoot();
        }
    }

    /// <summary>
    /// Sets the next bullet and turn it on
    /// </summary>
    void Shoot() {
        bullets[nextBullet % amount].transform.position = startingPoint.position;
        bullets[nextBullet % amount].transform.rotation = gun.rotation;
        bullets[nextBullet % amount].SetActive(true);
        nextBullet++;
    }
}