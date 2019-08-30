using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    void OnEnable() {
        GameObject gun = GameObject.Find("PlayerGun");
        Vector2 shootingDirection = gun.transform.rotation * Vector2.up;
        Shooting shootingScript = gun.GetComponent<Shooting>();

        transform.parent = null;
        transform.position = gun.transform.position;
        transform.rotation = gun.transform.rotation;
        transform.GetComponent<Rigidbody2D>().velocity = shootingDirection.normalized * shootingScript.bulletSpeed;

        Invoke("StopBullet", 2f);
    }
 
    /// Stops the bullet by turning it off
    void StopBullet() {
        gameObject.SetActive(false);
    }
}
