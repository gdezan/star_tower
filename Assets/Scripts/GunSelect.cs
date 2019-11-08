using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class GunSelect : MonoBehaviour {
    public int bulletQuantity = int.MaxValue;
    public Shooting currentShooting;

    GameObject currentHand;

    private void Start() {
        SwitchGun(0);
    }

    public void SwitchGun(int gunId) {
        if (currentHand != null) {
            currentHand.SetActive(false);
        };
        Transform existingGun = GetExistingGun(gunId);
        if (existingGun == null) {
            currentHand = Instantiate(GameManager.instance.guns[gunId].prefab, transform);
            return;
        }
        currentHand = existingGun.gameObject;
        currentHand.SetActive(true);
        currentShooting = currentHand.GetComponent<Shooting>();
    }

    Transform GetExistingGun(int gunId) {
        foreach (Transform child in transform) {
            if (child.GetComponent<Shooting>().gunProps.id == gunId) return child;
        }
        return null;
    }

    public void AddBullets(int bulletQuantity) {
        int newBulletQty = Math.Min(currentShooting.bulletCounter + bulletQuantity, currentShooting.gunProps.maxBullets);
        currentHand.GetComponent<Shooting>().bulletCounter = newBulletQty;
        UpdateBullets();
    }

    public void UpdateBullets() {
        Shooting chs = currentHand.GetComponent<Shooting>();
        bulletQuantity = chs.bulletCounter;
        GunSelectorUI gunUI = GameObject.Find("GunSelector").GetComponent<GunSelectorUI>();
        gunUI.UpdateBullets();
    }
}
