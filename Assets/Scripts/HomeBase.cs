using UnityEngine;

public class HomeBase : MonoBehaviour
{
    public UIManager UIManager;
    public Signal baseHealthSignal;
    public FloatValue baseHealth;

    private void Start() {
        transform.position = LevelManager.instance.homeBaseLocation;
    }

    public void TakeDamage(float damage) {
        baseHealth.runtimeValue = Mathf.Max(baseHealth.runtimeValue - damage, 0);
        baseHealthSignal.Raise();
        if (baseHealth.runtimeValue == 0) {
            GameManager.instance.GameOver(); ;
        }
    }
}
