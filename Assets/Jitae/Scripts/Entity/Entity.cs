using System;
using UnityEngine;

public class Entity : MonoBehaviour
{
    [SerializeField] private float hp;
    [SerializeField] private float atk;
    [SerializeField] private float def;

    [SerializeField] GaugeViewer hpViewer;

    private void Awake()
    {
        if (hpViewer == null)
        {
            Debug.LogError("hpViewer에 오브젝트가 할당되지 않았습니다.");
        }
    }

    public Action<float> OnHit;

    private void OnEnable()
    {
        // 실제 체력 데이터 값 감소
        OnHit += dealDamage;

        // 감소된 체력 데이터를 UI에 반영
        OnHit += hpViewer.SetGauge;
    }

    // 데미지를 주는 메서드
    public void dealDamage(float damageValue)
    {
        hp -= damageValue;
    }

    // 데미지를 받는 메서드
    public void TakeDamage(Entity targetEntity, float damage)
    {
        targetEntity.OnHit?.Invoke(damage);
    }
}
