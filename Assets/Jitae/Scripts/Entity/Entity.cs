using System;
using UnityEngine;
using System.Collections;
using UnityEditor.SceneManagement;


// 몬스터/플레이어는 모두 HP 게이지를 가지고 있어야 하므로, Entity 클래스에서 HP 게이지를 관리하도록 합니다.
[RequireComponent(typeof(GaugeViewer))]
public class Entity : MonoBehaviour
{
    private SpriteRenderer[] spriteRenderers;
    protected EntityType entityType;
    [SerializeField] protected float maxHp;
    public float MaxHp => maxHp;
    [SerializeField] protected float hp;
    public float Hp => hp;
    [SerializeField] protected float atk;
    public float Atk => atk;
    [SerializeField] protected float def;
    public float Def => def;
    public bool isAlive => hp > 0;
    [SerializeField] public int mobCount = 0;

    SFXPlayer sfxPlayer;
    /*
    다른 개체에게 공격을 당했을 때, 실행시킬 이벤트
    Entity 클래스에서는 HP 감소에 대한 이벤트를 정의
    이후로 맞았을 때 이벤트를 구독시켜주면됨 (ex : GaugeViewer에서 체력바 업데이트, SoundManager에서 피격 사운드 재생 등)
    OnHit의 매개변수 float는 받는 데미지
    */
    public Action<float> OnHit;
    private void Awake()
    {
        spriteRenderers = GetComponentsInChildren<SpriteRenderer>();

        if (sfxPlayer == null)
        {
            sfxPlayer = gameObject.AddComponent<SFXPlayer>();
        }
    }
    private void OnEnable()
    {
        // 실제 체력 데이터 값 감소
        OnHit += TakeDamage;
        OnHit += sfxPlayer.PlaySFX;
    }

    private void OnDisable()
    {
        OnHit -= TakeDamage;
        OnHit -= sfxPlayer.PlaySFX;
    }

    // 데미지를 받는 메서드
    public void TakeDamage(float damageValue)
    {
        hp -= damageValue;
        if (entityType == EntityType.Monster)
        {
            StartCoroutine(HitEffect());
        }
        else if (entityType == EntityType.Player)
        {

            StartCoroutine(HitEffect());
        }
        else if (entityType == EntityType.Boss)
        {

            StartCoroutine(HitEffect());
        }
    }

    /*
    데미지를 주는 메서드
    상대 개체에의 OnHit 이벤트를 호출한다.
    각자 구현한 공격 로직에서 공격할 대상을 판별하고, DealDamage 메서드를 호출하면 된다.
    */
    public void EntityHeal(float HealValue)
    {
        hp += HealValue;
    }
    private void DealDamage(Entity targetEntity, float damage)
    {
        targetEntity.OnHit?.Invoke(damage);
    }

    private IEnumerator HitEffect()
    {
        Transform root = transform.GetChild(0).GetChild(0);
        if (root == null)
        {
            yield break;
        }
        for (int i = 0; i < 3; i++)
        {
            root.gameObject.SetActive(false);
            yield return new WaitForSeconds(0.05f);
            root.gameObject.SetActive(true);
            yield return new WaitForSeconds(0.05f);
        }
    }

}