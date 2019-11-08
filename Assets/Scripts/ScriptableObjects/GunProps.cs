using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu]
public class GunProps : ScriptableObject, ISerializationCallbackReceiver {
    public int id;
    public Sprite handSprite;
    public Sprite pickupSprite;
    public int maxBullets;
    public int bulletPickupQuantity;
    public bool infiniteAmmo;
    public GameObject prefab;

    [Header("Use Bullets (default)")]
    public float fireRate;
    public GameObject bullet;
    public GameObject flare;

    [Header("Use Laser")]
    public bool useLaser;
    public float damage;
    public float ammoDecayRate;
    public LayerMask whatIsSolid;

    public void OnBeforeSerialize() {
    }

    public void OnAfterDeserialize() {
    }
}
