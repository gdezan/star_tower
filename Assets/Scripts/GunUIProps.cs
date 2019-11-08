using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunUIProps : MonoBehaviour
{
    public int gunId;
    public bool isSelected;

    Vector3 ogScale;

    private void Awake() {
        ogScale = new Vector3(1,1,1);
    }

    public void Select() {
        isSelected = true;
        transform.localScale = ogScale;
    }

    public void Deselect() {
        isSelected = false;
        transform.localScale *= 0.7f;
    }
}
