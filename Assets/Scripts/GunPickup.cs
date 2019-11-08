using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunPickup : MonoBehaviour
{
    public int gunId;

    GunSelect gunHand;
    GunSelectorUI gunSelectorUI;

    private void Start() {
        SetPickup(gunId);
        gunHand = GameObject.Find("Hand").GetComponent<GunSelect>();
        gunSelectorUI = GameObject.Find("GunSelector").GetComponent<GunSelectorUI>();
    }

    public void SetPickup(int pickupId = 0) {
        gunId = pickupId;
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        sr.sprite = GameManager.instance.guns[gunId].pickupSprite;

        BoxCollider2D bc = GetComponent<BoxCollider2D>();
        bc.size = sr.size;
    }

    public void Pickup() {
        gunHand.SwitchGun(gunId);
        gunSelectorUI.AddGun(gunId);
        AudioSource audio = transform.GetComponent<AudioSource>();
        audio.Play();
        gameObject.GetComponent<SpriteRenderer>().enabled = false;
        gameObject.GetComponent<BoxCollider2D>().enabled = false;
        Destroy(gameObject, audio.clip.length);
    }
}
