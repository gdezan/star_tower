using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {
    public float speed;
    public float distance;
    public float damage;
    public float explosionRadius = 0f;
    public LayerMask whatIsSolid;
    public GameObject hitPrefab;

    [Header("Slow Down")]
    public bool slowEffect;
    [Range(0f, 1f)]
    public float speedModifier;
    public float timeSlowedDown;

    private void Start() {
        Invoke("DestroyBullet", 2f);
    }

    private void Update() {
        transform.position += transform.up * speed * Time.deltaTime;

        RaycastHit2D hitInfo = Physics2D.Raycast(transform.position, transform.up, distance, whatIsSolid);
        if (hitInfo.collider != null) {
            if (hitInfo.collider.CompareTag("Enemy")) {
                EnemyBehavior enemy = hitInfo.collider.gameObject.GetComponent<EnemyBehavior>();
                if (damage > 0) {
                    if (explosionRadius > 0f) {
                        Explode();
                    } else {
                        enemy.TakeDamage(damage);
                        if (GetComponent<AudioSource>()) {
                            GetComponent<AudioSource>().Play();
                        }
                    }
                }
                if (slowEffect) {
                    enemy.ModifySpeed(speedModifier, timeSlowedDown);
                }
            }
            HitBullet();
        }
    }

    void HitBullet() {

        GameObject hit = Instantiate(hitPrefab, transform.position, transform.rotation);
        hit.transform.localRotation *= Quaternion.Euler(0, 0, 225);
        ParticleSystem hitPs = hit.GetComponent<ParticleSystem>();
        hitPs.Play();
        DestroyBullet();
        Destroy(hit.gameObject, hitPs.main.duration);
    }

    /// Stops the bullet by turning it off
    void DestroyBullet() {
        Destroy(gameObject);
    }

    void Explode() {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, explosionRadius);
        foreach (Collider2D collider in colliders) {
            if (collider.CompareTag("Enemy")) {
                collider.gameObject.GetComponent<EnemyBehavior>().TakeDamage(damage);
            }
        }
    }

    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}

