using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class EnemyBehavior : MonoBehaviour {
    [Header("Monster Stats")]
    public float health;
    public float speed;
    public float turnSpeed = 10f;
    public float damage = 10f;

    [Header("Drops")]
    public Drop[] drops;

    [Header("Unity Stuff")]
    public GameObject deathEffect;
    public Transform sprite;
    public Image healthBar;
    public bool dying = false;

    Vector3 nextTile;
    Vector3[,] positionMatrix;
    int[,] indexMatrix;
    int ntX;
    int ntY;
    float maxHealth;
    Quaternion rotation;
    float originalSpeed;

    private void Update() {
        LevelManager lm = LevelManager.instance;

        if (lm.indexMatrix != null && indexMatrix == null) {
            positionMatrix = lm.positionMatrix;
            indexMatrix = new int[LevelManager.verticalTilesQty, LevelManager.horizontalTilesQty];
            Array.Copy(lm.indexMatrix, indexMatrix, lm.indexMatrix.Length);
        }

        WalkToNextTile();
        sprite.rotation = Quaternion.Slerp(sprite.rotation, rotation, Time.deltaTime * turnSpeed);
    }

    // Start is called before the first frame update
    void Start() {
        maxHealth = health;
        originalSpeed = speed;
        int[] homeBase = LevelManager.instance.GetCurrentTile(LevelManager.instance.spawnPointLocation);
        ntX = homeBase[0];
        ntY = homeBase[1];
    }

    public void TakeDamage(float damage) {
        health -= damage;
        healthBar.fillAmount = health / maxHealth;
        //lifeText.SetText(life.ToString());

        if (health <= 0) {
            Death();
        }
    }

    void Death(bool withDrops = true) {
        if (!dying) {
            healthBar.transform.parent.gameObject.SetActive(false);
            GetComponent<AudioSource>().Play();
            sprite.gameObject.SetActive(false);
            foreach (Collider2D col in GetComponents<BoxCollider2D>()) {
                col.enabled = false;
            }

            Instantiate(deathEffect, transform.position, Quaternion.identity);

            if (withDrops && drops.Length > 0) {
                foreach (Drop drop in drops) {
                    float rn = UnityEngine.Random.Range(0f, 1f);

                    if (rn <= drop.dropChance) {
                        DropObject(drop);
                        break;
                    }
                }
            }
            EnemySpawner.EnemiesAlive--;
        }

        Destroy(gameObject, 1f);
        dying = true;
    }

    public void ModifySpeed(float modifier, float time) {
        speed = originalSpeed * modifier;
        Invoke("SetOriginalSpeed", time);
    }

    void SetOriginalSpeed() {
        speed = originalSpeed;
    }

    void DropObject(Drop drop) {
        GameObject d = Instantiate(drop.drop, transform.position, Quaternion.identity);
        d.transform.SetParent(null);

        switch (drop.typeOfDrop) {
            case Drop.dropTypes.Gun:
                d.GetComponent<GunPickup>().SetPickup(drop.gunId);
                break;
            case Drop.dropTypes.Points:
                d.GetComponent<Cog>().value = drop.pointsQuantity;
                break;
        }
    }

    int[] GetNextTile(int x, int y) {
        indexMatrix[y, x] = 2;
        int ht = LevelManager.horizontalTilesQty;
        int vt = LevelManager.verticalTilesQty;

        if (y < vt - 1 && indexMatrix[y + 1, x] == 1) {
            rotation = Quaternion.Euler(0, 0, -90);
            return new int[] { y + 1, x };
        } else if (y > 0 && indexMatrix[y - 1, x] == 1) {
            rotation = Quaternion.Euler(0, 0, 90);
            return new int[] { y - 1, x };
        } else if (x < ht - 1 && indexMatrix[y, x + 1] == 1) {
            rotation = Quaternion.Euler(0, 0, 0);
            return new int[] { y, x + 1 };
        } else if (x > 0 && indexMatrix[y, x - 1] == 1) {
            rotation = Quaternion.Euler(0, 0, 180);
            return new int[] { y, x - 1 };
        }
        return new int[] { y, x };
    }

    void WalkToNextTile() {
        float step = speed * Time.deltaTime; // calculate distance to move
        nextTile = positionMatrix[ntY, ntX];
        transform.position = Vector3.MoveTowards(transform.position, nextTile, step);
        if (Vector3.Distance(transform.position, nextTile) < 5f) {
            int[] nt = GetNextTile(ntX, ntY);
            //if (nt[1] == ntX && ntY == nt[0]) Death(false);
            ntX = nt[1];
            ntY = nt[0];
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag("Base")) {
            collision.gameObject.GetComponent<HomeBase>().TakeDamage(damage);
            Death(false);
        }

        if (collision.CompareTag("Player")) {
            AstronautMovement player = collision.gameObject.GetComponent<AstronautMovement>();
            player.TakeDamage(damage);
            player.Knockback(damage, transform.position);
            Death(false);
        }
    }
}
