using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorManager : MonoBehaviour {
    public Texture2D regularCursorTexture;
    public Texture2D shootingCursorTexture;
    public bool ccEnabled = false;

    [Header("Needed for the \"Turret Cursor\"")]
    public TurretCursor turretCursor;
    public TileBorder tileBorder;

    bool turretSelected = false;
    Vector3 lastTilePosition;
    int selectedTurretId;
    int[] tile;
    Texture2D currentCursor;

    void Start() {
        Invoke("SetRegularCursor", 0.2f);
    }

    private void Update() {
        if (Time.timeScale == 0) {
            if (currentCursor != regularCursorTexture) {
                SetRegularCursor();
            }
            return;
        }

        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        tile = LevelManager.instance.GetCurrentTile(mousePosition);
        int tileIndex = LevelManager.instance.indexMatrix[tile[1], tile[0]];


        if (tileIndex < 9 && currentCursor != shootingCursorTexture) {
            SetShootingCursor();
            GameManager.instance.SetPlayerShooting(true);
        } else if (tileIndex >= 9 && currentCursor != regularCursorTexture) {
            SetRegularCursor();
            GameManager.instance.SetPlayerShooting(false);
        }
        if (turretSelected) {
            ControlTurretCursor();
        }
    }

    void DisableCustomCursor() {
        //Resets the cursor to the default  
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
        //Set the _ccEnabled variable to false  
        this.ccEnabled = false;
    }

    private void SetRegularCursor() {
        Vector2 cursorHotspot = new Vector2(regularCursorTexture.width / 2, regularCursorTexture.height / 2);
        Cursor.SetCursor(this.regularCursorTexture, cursorHotspot, CursorMode.Auto);
        this.ccEnabled = true;
        currentCursor = regularCursorTexture;
    }

    private void SetShootingCursor() {
        Vector2 cursorHotspot = new Vector2(shootingCursorTexture.width / 2, shootingCursorTexture.height / 2);
        Cursor.SetCursor(this.shootingCursorTexture, cursorHotspot, CursorMode.Auto);
        this.ccEnabled = true;
        currentCursor = shootingCursorTexture;
    }

    public void EnableTurretCursor(int turretId) {
        turretCursor.gameObject.SetActive(true);
        tileBorder.gameObject.SetActive(true);
        turretSelected = true;
        turretCursor.SetTurretCursorSprite(turretId);
        selectedTurretId = turretId;
    }

    public void DisableTurretCursor() {
        turretCursor.gameObject.SetActive(false);
        tileBorder.gameObject.SetActive(false);
        turretSelected = false;
    }

    void ControlTurretCursor() {
        GameManager.instance.SetPlayerShooting(false);
        Vector3 newTilePosition = LevelManager.instance.positionMatrix[tile[1], tile[0]];
        bool isDisabled = LevelManager.instance.indexMatrix[tile[1], tile[0]] != 0;

        if (lastTilePosition != newTilePosition) {
            turretCursor.SetTurretCursorPosition(newTilePosition, isDisabled);
            tileBorder.SetTileBorderPosition(newTilePosition, isDisabled);
            lastTilePosition = newTilePosition;
        }

        if (Input.GetButton("Fire1")) {
            if (isDisabled) {
                // play animation here
            } else {
                GameManager.instance.SetUpConstruction(tile, newTilePosition, selectedTurretId);
                GameManager.instance.SetPlayerShooting(true);
                DisableTurretCursor();
            }
        }
    }
}
