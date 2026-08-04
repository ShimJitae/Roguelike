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
        OnHit += TakeDamage;
    }

    private void OnDisable()
    {
        OnHit -= TakeDamage;
    }

    // 데미지를 받는 메서드
    public void TakeDamage(float damageValue)
    {
        hp -= damageValue;
    }

    // 데미지를 주는 메서드
    private void DealDamage(Entity targetEntity, float damage)
    {
        targetEntity.OnHit?.Invoke(damage);
    }
}