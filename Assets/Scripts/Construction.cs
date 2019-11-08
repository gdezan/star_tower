using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Construction : MonoBehaviour {
    public Canvas tooltip;
    public GameObject spinner;
    public KeyCode constructionKey;

    private bool playerInZone = false;
    private AstronautMovement player;
    private BoxCollider2D boxCollider;
    private bool mouseInZone = false;
    private int turretId;

    public int TurretId { get => turretId; set => turretId = value; }

    // Start is called before the first frame update
    void Start() {
        player = GameObject.Find("Player").GetComponent<AstronautMovement>();
        boxCollider = gameObject.GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update() {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition = new Vector3(mousePosition.x, mousePosition.y, transform.position.z);

        if (!mouseInZone && boxCollider.bounds.Contains(mousePosition)) {
            SetMouseInZone(true);
        }
        else if (mouseInZone && !boxCollider.bounds.Contains(mousePosition)) {
            SetMouseInZone(false);
        }

        if (mouseInZone && playerInZone) {
            if (Input.GetKey(constructionKey)) {
                StartConstruction();
            }
        }
    }

    private void SetMouseInZone(bool inZone) {
        if (inZone) {
            mouseInZone = true;
        }
        else {
            mouseInZone = false;
        }
        tooltip.GetComponent<Animator>().SetBool("isOn", playerInZone && mouseInZone);
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag("Player")) {
            playerInZone = true;
            tooltip.GetComponent<Animator>().SetBool("isOn", playerInZone && mouseInZone);
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        if (collision.CompareTag("Player")) {
            playerInZone = false;
            tooltip.GetComponent<Animator>().SetBool("isOn", playerInZone && mouseInZone);
        }
    }

    private void StartConstruction() {
        player.StartBuilding();
        GameManager.instance.SetPlayerShooting(false);
        spinner.SetActive(true);
        tooltip.enabled = false;
        StartCoroutine(Build());
    }

    private IEnumerator Build() {
        yield return new WaitForSeconds(GameManager.instance.turrets[turretId].timeToBuild);
        player.StopBuilding();
        spinner.SetActive(false);
        GameManager.instance.SetPlayerShooting(true);
        GameManager.instance.CreateTurret(transform.position, turretId);
        Destroy(gameObject);
    }
}