using System.IO;
using UnityEngine;
using UnityEngine.InputSystem;

public enum WeaponType
{
    Sword,
    Bow,

}

public class WeaponSwap : MonoBehaviour
{
    
    //에셋 경로
    //Assets/Imports/SPUM/Resources/Addons/Ver300/0_Unit/0_Sprite/8_Weapons/0_Sword/New_Weapon_06.png
    //Assets/Imports/SPUM/Resources/Addons/Legacy/0_Unit/0_Sprite/6_Weapons/2_Bow/Bow_1.png

    public WeaponType weaponType {  get; private set; }
    private PlayerAnim anim;
    [SerializeField] private WeaponPanel panel;
    [SerializeField] private SpriteRenderer sr;
    [SerializeField] private Sprite swordSr;
    [SerializeField] private Sprite bowSr;
    [SerializeField] private Sprite currentWeapon;

    private void Awake()
    {
        weaponType = WeaponType.Sword;
        currentWeapon = swordSr;
        anim = GetComponentInChildren<PlayerAnim>();
    }

    void Start()
    {
        //TODO Find 안쓰고 하면 좋지않을까?
        //panel = GameObject.Find("WeaponPanel").GetComponent<WeaponPanel>();
    }


    public void OnSwap(InputValue value)
    {
        // 공격 중에는 교체 불가
        if (anim.isAttacking)
            return;

        if (weaponType == WeaponType.Sword)
        {
            weaponType = WeaponType.Bow;
        }
        else
        {
            weaponType = WeaponType.Sword;
        }

        panel.SwapEquipment();
    }


    void Update()
    {

        switch (weaponType)
        {
            case WeaponType.Sword:
                
                currentWeapon = swordSr;
                sr.sprite = currentWeapon;
                break;
            case WeaponType.Bow:
                currentWeapon = bowSr;
                sr.sprite = currentWeapon;
                break;

            default: break;
        }
    }
}
