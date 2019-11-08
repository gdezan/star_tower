using UnityEngine;

public class Shooting : MonoBehaviour {
    public Transform shotPoint;
    public GunProps gunProps;
    public TurretProps turretProps;
    public int bulletCounter;
    public bool shootingEnabled = true;

    private bool isTurret = false;
    private GunSelect gunSelect;
    private float nextShotTime = 0f;
    private float initialPitch;
    private GameObject bulletPrefab;
    private GameObject flarePrefab;
    LineRenderer laserRenderer;
    private float laserBullets;

    void Awake() {
        if (transform.GetComponent<AudioSource>() != null)
            initialPitch = transform.GetComponent<AudioSource>().pitch;

        if (gunProps != null && turretProps == null) {
            if (gunProps.useLaser) {
                laserRenderer = gameObject.GetComponent<LineRenderer>();
                laserRenderer.enabled = false;
            }
            isTurret = false;
            bulletPrefab = gunProps.bullet;
            flarePrefab = gunProps.flare;
            if (!gunProps.infiniteAmmo) {
                gunSelect = transform.parent.gameObject.GetComponent<GunSelect>();
                bulletCounter = gunProps.bulletPickupQuantity;
                gunSelect.UpdateBullets();

                if (gunProps.useLaser) {
                    laserBullets = bulletCounter;
                }
            }
        } else if (gunProps == null && turretProps != null) {
            isTurret = true;
            bulletPrefab = turretProps.bullet;
            flarePrefab = turretProps.flare;
        } else {
            Debug.LogError("Shooting script missing a GunProps or TurretProps");
        }
    }


    // Update is called once per frame
    void Update() {
        bool isShooting = Input.GetButton("Fire1");

        if (!isTurret && isShooting && Time.time >= nextShotTime && shootingEnabled) {
            if (gunProps.useLaser) {
                Laser();
            } else {
                Shoot();
                nextShotTime = Time.time + 1f / gunProps.fireRate;
            }
        } else if (!isTurret && gunProps.useLaser && laserRenderer.enabled) {
            laserRenderer.enabled = false;
            GetComponent<AudioSource>().Stop();
            //particles.Stop();
        }
    }

    private void OnEnable() {
        if (!isTurret && bulletCounter <= 0 && !gunProps.infiniteAmmo) {
            bulletCounter = gunProps.bulletPickupQuantity;
            gunSelect.UpdateBullets();

            if (gunProps.useLaser) {
                laserBullets = bulletCounter;
            }
        }
    }

    public void Shoot() {
        GameObject bullet = Instantiate(bulletPrefab, shotPoint.position, shotPoint.rotation);

        AudioSource gunSound = transform.GetComponent<AudioSource>();
        if (gunSound != null) {
            if (!isTurret) gunSound.pitch = initialPitch * (1 + Random.Range(-0.2f, 0.2f));
            gunSound.Play();
        }

        if (!isTurret && !gunProps.infiniteAmmo) {
            bulletCounter--;
            gunSelect.UpdateBullets();
        }

        Effect();
        Destroy(bullet, 0.5f);
    }

    private void Laser() {
        RaycastHit2D hit = Physics2D.Raycast(shotPoint.position, transform.up, Mathf.Infinity, gunProps.whatIsSolid);
        if (!laserRenderer.enabled) {
            laserRenderer.enabled = true;
            GetComponent<AudioSource>().Play();
            //particles.Play();
        }
        if (Time.timeScale != 0) {
            laserBullets -= gunProps.ammoDecayRate * (Time.deltaTime);
            bulletCounter = (int) laserBullets;
            gunSelect.UpdateBullets();
            if (hit.collider != null) {
                if (hit.collider.CompareTag("Enemy")) {
                    hit.collider.gameObject.GetComponent<EnemyBehavior>().TakeDamage(gunProps.damage * Time.deltaTime);
                }
            }
        }
        laserRenderer.SetPosition(0, shotPoint.position);
        laserRenderer.SetPosition(1, hit.point);
    }

    void Effect() {
        //GameObject flare = Instantiate(flarePrefab, shotPoint.position, transform.rotation);
        //float size = Random.Range(0.9f, 1.5f);
        //flare.transform.localScale = new Vector3(size, size, size);
        //Destroy(flare, 0.02f);
    }
}
