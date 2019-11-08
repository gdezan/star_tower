using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileBorder : MonoBehaviour {
    public Color enabledColor;
    public Color disabledColor;

    public void SetTileBorderPosition(Vector3 newPosition, bool isDisabled) {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        transform.position = newPosition;
        if (isDisabled) {
            sr.color = disabledColor;
        }
        else {
            sr.color = enabledColor;
        }
    }
}
