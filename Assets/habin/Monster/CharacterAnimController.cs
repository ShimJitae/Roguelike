using UnityEngine;

public class CharacterAnimController : MonoBehaviour
{
    private Animator an;
    private Animator animator;
    private static readonly int IdleHash = Animator.StringToHash("IDLE");
    private static readonly int moveHash = Animator.StringToHash("MOVE");
    private static readonly int CastHash = Animator.StringToHash("CAST");
    private static readonly int AttackHash = Animator.StringToHash("ATTACK");
    private static readonly int Attack2Hash = Animator.StringToHash("ATTACK2");
    private static readonly int AttackBowHash = Animator.StringToHash("ATTACKBOW");
    private static readonly int DamagedHash = Animator.StringToHash("DAMAGED");
    private static readonly int DeathHash = Animator.StringToHash("DEATH");
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Transform unitRoot = transform.Find("UnitRoot");
        an = unitRoot.GetComponent<Animator>();
        animator = GetComponent<Animator>();

    }
    public void PlayIdle()
    {
        an.Play(IdleHash);
    }
    public void Playmove()
    {
        an.Play(moveHash);
    }
    public void PlayAttack()
    {
        an.Play(AttackHash);
    }
    public void PlayAttack2()
    {
        an.Play(Attack2Hash);
    }
    public void PlayCast()
    {
        an.Play(CastHash);
    }
    public void PlayAttackBow()
    {
        an.Play(AttackBowHash);
    }
    public void PlayDamaged()
    {
        an.Play(DamagedHash);
    }
    public void PlayDeath()
    {
        an.Play(DeathHash);
    }
}
