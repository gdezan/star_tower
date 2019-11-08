using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu]
public class TurretProps : ScriptableObject, ISerializationCallbackReceiver {
    public int id;
    public Sprite sprite;
    public float cost;
    public float timeToBuild;
    public float range;
    public float turnSpeed;
    public GameObject turretPrefab;

    [Header("Use Bullets (default)")]
    public float fireRate;
    public GameObject flare;
    public GameObject bullet;

    [Header("Use Laser")]
    public bool useLaser = false;
    public float damage;

    public void OnBeforeSerialize() {
    }

    public void OnAfterDeserialize() {
    }
}
