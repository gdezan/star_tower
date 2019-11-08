using UnityEditor;
using UnityEngine;

[System.Serializable]
public class Drop {
    public GameObject drop;

    [Range(0f, 1f)]
    public float dropChance;

    public enum dropTypes {
        Gun,
        Points,
    }

    public dropTypes typeOfDrop;

    [Header("For Gun Drops")]
    public int gunId;

    [Header("For Points Drops")]
    public int pointsQuantity;
}
