using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour {
    public Transform target;
    public Transform gun;

    [Header("Needed for Laser")]
    public GameObject laser;

    float nextShotTime = 0f;
    TurretProps props;
    Shooting shooting;
    LineRenderer laserRenderer;
    ParticleSystem particles;
    Transform shotPoint;
    EnemyBehavior targetEnemy;
    bool lockedOn;

    // Start is called before the first frame update
    void Start() {
        InvokeRepeating("UpdateTarget", 0f, 0.5f);
        shooting = gun.GetComponent<Shooting>();
        props = shooting.turretProps;

        if (props.useLaser) {
            laserRenderer = laser.GetComponent<LineRenderer>();
            shotPoint = gun.Find("ShotPoint");
            particles = shotPoint.GetComponent<ParticleSystem>();
        }
    }

    // Update is called once per frame
    void Update() {
        if (target == null) {
            if (props.useLaser && laserRenderer.enabled) {
                laserRenderer.enabled = false;
                GetComponent<AudioSource>().Stop();
                particles.Stop();
            }
            return;
        }

        LockOnTarget();

        if (props.useLaser) {
            UseLaser();
        } else {
            UseBullets();
        }
    }

    void LockOnTarget() {
        Quaternion lookRotation = Quaternion.LookRotation(transform.position - target.position, Vector3.forward);
        lookRotation.x = 0f;
        lookRotation.y = 0f;
        gun.rotation = Quaternion.Slerp(gun.rotation, lookRotation, Time.deltaTime * props.turnSpeed);
        lockedOn = Mathf.Abs(gun.rotation.eulerAngles.z - lookRotation.eulerAngles.z) < 2f;
    }

    void UpdateTarget() {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        float shortestDistance = Mathf.Infinity;
        GameObject nearestEnemy = null;

        foreach (GameObject enemy in enemies) {
            float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
            if (distanceToEnemy < shortestDistance) {
                if (!enemy.GetComponent<EnemyBehavior>().dying) {
                nearestEnemy = enemy;
                shortestDistance = distanceToEnemy;
                }
            }
        }

        if (nearestEnemy != null && shortestDistance <= props.range) {
            targetEnemy = nearestEnemy.GetComponent<EnemyBehavior>();
            target = nearestEnemy.transform;
        } else {
            target = null;
        }
    }

    void UseLaser() {
        if (!laserRenderer.enabled) {
            laserRenderer.enabled = true;
            GetComponent<AudioSource>().Play();
            particles.Play();
        }
        if (Time.timeScale != 0) {
            targetEnemy.TakeDamage(props.damage * Time.deltaTime);
        }
        laserRenderer.SetPosition(0, shotPoint.position);
        laserRenderer.SetPosition(1, target.position);
    }

    void UseBullets() {
        if (Time.time >= nextShotTime) {
            shooting.Shoot();
            nextShotTime = Time.time + 1f / props.fireRate;
        }
    }

    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.red;
        if (props)
            Gizmos.DrawWireSphere(transform.position, props.range);
    }
}
