using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flare : MonoBehaviour {
    private Animation anim;

    private void Awake() {
        anim = transform.GetComponent<Animation>();
    }

    void OnEnable() {
        GameObject gun = GameObject.Find("PlayerGun");
        Vector2 shootingDirection = gun.transform.rotation * Vector2.up;
        Shooting shootingScript = gun.GetComponent<Shooting>();

        transform.parent = null;
        transform.position = gun.transform.position;
        transform.rotation = gun.transform.rotation;
        
        anim.Play();

        Invoke("StopFlare", 2f);
    }

    /// Stops the bullet by turning it off
    void StopFlare() {
        anim.Stop();
        gameObject.SetActive(false);
    }
}
