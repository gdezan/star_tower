using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour {
    [SerializeField]
    GameObject bulletPrefab;
    [SerializeField]
    GameObject flarePrefab;
    [SerializeField]
    float fireRate = 1f;
    public int flareAmount = 4;

    public float bulletSpeed = 150.0f;

    private int bulletAmount;
    private float nextShotTime = 0f;
    private int nextBullet = 0;
    private float initialPitch;
    private List<GameObject> bullets = new List<GameObject>();
    private List<GameObject> flares = new List<GameObject>();

    void Awake() {
        bulletAmount = (int) Mathf.Ceil(fireRate * 2.5f);
        initialPitch = transform.GetComponent<AudioSource>().pitch;        
    }

    void Start() {
        for (int i = 0; i < bulletAmount; i++) {
            AddToPool(bulletPrefab, bullets);
            if (i < flareAmount) AddToPool(flarePrefab, flares);
        }
    }

    // Update is called once per frame
    void Update() {
        bool isShooting = Input.GetButton("Fire1");

        if (isShooting && Time.time >= nextShotTime) {
            Shoot();
            nextShotTime = Time.time + 1f / fireRate;
        }
    }

    void AddToPool(GameObject prefab, List<GameObject> list) {
        GameObject obj = (GameObject)Instantiate(prefab, transform.position, Quaternion.identity);
        obj.transform.parent = gameObject.transform;
        obj.SetActive(false);
        list.Add(obj);
    }

    void Shoot() {
        Vector2 shootingDirection = transform.parent.gameObject.transform.rotation * Vector2.right;
        //GameObject flare = Instantiate(flarePrefab, transform.position, Quaternion.identity);

        //flare.transform.Rotate(0, 0, Mathf.Atan2(shootingDirection.y, shootingDirection.x) * Mathf.Rad2Deg - 90);
        //flare.transform.Translate(0.03f, 1.1f, 0);

        AudioSource gunSound = transform.GetComponent<AudioSource>();
        gunSound.pitch = initialPitch * (1 + Random.Range(-0.2f, 0.2f));
        gunSound.Play();

        //Destroy(flare, 0.5f);

        bullets[nextBullet % bulletAmount].SetActive(true);
        flares[nextBullet % flareAmount].SetActive(true);
        nextBullet++;
    }
}
