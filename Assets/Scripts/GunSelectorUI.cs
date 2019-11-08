using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GunSelectorUI : MonoBehaviour {
    public GameObject gunUIPrefab;
    public GunSelect gunSelect;
    public GunPickup gunPickup;

    int selectedWeaponId = 0;
    GunProps props;
    Transform selectedWeapon = null;

    // Update is called once per frame
    void Update() {
        if (Input.GetKeyDown(KeyCode.Tab)) {
            NextWeapon();
        }
    }

    public void AddGun(int gunId) {
        bool gunExists = false;
        foreach (Transform child in transform) {
            int id = child.gameObject.GetComponent<GunUIProps>().gunId;
            if (id == gunId) {
                gunExists = true;
            }
        }
        if (gunExists) {
            ChangeSelectedWeapon(gunId);
            gunSelect.AddBullets(props.bulletPickupQuantity);
        } else {
            AddGunToUI(gunId);
        }
    }

    void AddGunToUI(int gunId) {
        Vector3 newPosition = new Vector3(120 * transform.childCount, 0, 0);
        props = GameManager.instance.guns[gunId];
        GameObject newGun = Instantiate(gunUIPrefab, newPosition, Quaternion.identity);
        newGun.GetComponent<GunUIProps>().gunId = gunId;
        newGun.transform.SetParent(gameObject.transform, false);

        Image gunImage = newGun.transform.Find("Image").GetComponent<Image>();
        gunImage.sprite = props.pickupSprite;
        gunImage.SetNativeSize();
        ChangeSelectedWeapon(gunId);
        gunSelect.UpdateBullets();
    }

    void ChangeSelectedWeapon(int gunId) {
        if (selectedWeaponId == gunId) return;
        Transform oldGun = null;
        Transform newGun = null;
        foreach (Transform child in transform) {
            int id = child.gameObject.GetComponent<GunUIProps>().gunId;
            if (id == selectedWeaponId) {
                oldGun = child;
            } else if (id == gunId) {
                newGun = child;
            }
        }
        SwitchGuns(oldGun, newGun);
    }

    void NextWeapon() {
        int i = 0;
        foreach (Transform child in transform) {
            i++;
            int id = child.gameObject.GetComponent<GunUIProps>().gunId;
            if (id == selectedWeaponId) {
                break;
            }
        }
        SwitchGuns(transform.GetChild((i - 1) % transform.childCount), transform.GetChild(i % transform.childCount));
    }

    void SwitchGuns(Transform oldGun, Transform newGun) {
        GunUIProps newGunProps = newGun.gameObject.GetComponent<GunUIProps>();
        GunUIProps oldGunProps = oldGun.gameObject.GetComponent<GunUIProps>();
        newGunProps.Select();
        oldGunProps.Deselect();
        gunSelect.SwitchGun(newGunProps.gunId);
        selectedWeaponId = newGunProps.gunId;
        selectedWeapon = newGun;
        props = GameManager.instance.guns[selectedWeaponId];
    }

    public void UpdateBullets() {
        if (!selectedWeapon || gunSelect.currentShooting.gunProps.infiniteAmmo)
            return;

        Transform counter = selectedWeapon.transform.Find("BulletCounter");
        counter.GetComponent<TextMeshProUGUI>().SetText(gunSelect.bulletQuantity.ToString());

        if (gunSelect.bulletQuantity <= 0) {

            // Move the weapons to the left
            GameObject gunToDestroy = null;
            foreach (Transform child in transform) {
                if (gunToDestroy != null) {
                    Vector3 pos = child.localPosition;
                    child.localPosition = new Vector3(pos.x - 120f, pos.y, pos.z);
                } else {
                    int id = child.gameObject.GetComponent<GunUIProps>().gunId;
                    if (id == selectedWeaponId) {
                        gunToDestroy = child.gameObject;
                    }
                }
            }

            NextWeapon();
            Destroy(gunToDestroy);
        }
    }
}