using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AstronautMovement : MonoBehaviour {
    public GunSelect playerHand;
    public float movementSpeed = 10.0f;
    public float knockbackTime = 1f;

    [Header("Player Health")]
    public FloatValue playerHealth;
    public Signal playerHealthSignal;

    [Header("Player Points")]
    public FloatValue playerPoints;
    public Signal playerPointsSignal;

    Vector2 movementDirection;
    Rigidbody2D rb;
    bool onKnockback = false;
    bool isBuilding = false;

    private void Start() {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update() {
        ProcessInputs();
        bool playerEnabled = !onKnockback && !isBuilding && Time.timeScale > 0;
        if (playerEnabled) {
            FaceMouse();
            Move();
        }
    }

    void FaceMouse() {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);

        Vector2 direction = new Vector2(
            mousePosition.x - transform.position.x,
            mousePosition.y - transform.position.y
        );

        transform.right = direction;
    }

    void ProcessInputs() {
        movementDirection = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
    }

    void Move() {
        rb.velocity = movementDirection * movementSpeed;
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag("Pickup")) {
            collision.gameObject.GetComponent<GunPickup>().Pickup();
        } else if (collision.CompareTag("Cog")) {
            PickUpCog(collision.gameObject);
        }
    }

    void PickUpCog(GameObject cog) {
        cog.GetComponent<AudioSource>().Play();
        cog.GetComponent<SpriteRenderer>().enabled = false;
        cog.GetComponent<BoxCollider2D>().enabled = false;
        playerPoints.runtimeValue += (int) cog.GetComponent<Cog>().value;
        playerPointsSignal.Raise();
        Destroy(cog, 0.5f);
    }

    public void TakeDamage(float damage) {
        playerHealth.runtimeValue -= damage;
        playerHealthSignal.Raise();

        if (playerHealth.runtimeValue <= 0) {
            GameManager.instance.GameOver();
        }
    }

    public void Knockback(float damage, Vector3 enemyPosition) {
        Vector3 diff = transform.position - enemyPosition;
        Vector2 thrust = diff.normalized * damage;
        rb.AddForce(thrust * 10, ForceMode2D.Impulse);
        onKnockback = true;
        StartCoroutine(KnockCo());
    }

    private IEnumerator KnockCo() {
        yield return new WaitForSeconds(knockbackTime);
        onKnockback = false;
    }

    public void StartBuilding() {
        rb.velocity = Vector2.zero;
        rb.freezeRotation = true;
        isBuilding = true;
    }

    public void StopBuilding() {
        isBuilding = false;
        rb.freezeRotation = false;
    }
}
