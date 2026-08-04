using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WeaponSlot : MonoBehaviour
{
    [SerializeField] private Image weaponImage;
    [SerializeField] private Image shadowImage;
    [SerializeField] private TMP_Text weaponNameText;

    public void ActiveShadow(bool active)
    {
        shadowImage.gameObject.SetActive(active);
    }
}
