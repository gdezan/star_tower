using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretCursor : MonoBehaviour
{
    public Color disabledColor;

    public void SetTurretCursorSprite(int turretId) {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        sr.sprite = GameManager.instance.turrets[turretId].sprite;
    }

    public void SetTurretCursorPosition(Vector3 newPosition, bool isDisabled) {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();

        if (isDisabled) {
            sr.color = disabledColor;
        } else {
            sr.color = Color.white;
        }

        transform.position = newPosition;
    }
}
