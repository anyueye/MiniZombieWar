using System;
using GameFramework.Fsm;
using UnityEngine;
using UnityGameFramework.Runtime;

/// <summary>
/// 初始状态
/// </summary>
public class PreparingState : FsmState<Soldier>
{
    protected override void OnUpdate(IFsm<Soldier> fsm, float elapseSeconds, float realElapseSeconds)
    {
        base.OnUpdate(fsm, elapseSeconds, realElapseSeconds);
        ChangeState<PrecisionState>(fsm);
    }
}

/// <summary>
/// 跑路状态，命中率降低
/// </summary>
public class RunningState : FsmState<Soldier>
{
    private Weapon m_Weapon;
    private Soldier _soldier;

    protected override void OnEnter(IFsm<Soldier> solider)
    {
        base.OnEnter(solider);
        _soldier = solider.Owner;
        m_Weapon = _soldier.m_Weapon;
    }

    protected override void OnUpdate(IFsm<Soldier> solider, float elapseSeconds, float realElapseSeconds)
    {
        base.OnUpdate(solider, elapseSeconds, realElapseSeconds);
        if (_soldier._rigidbody.velocity.magnitude < _soldier._precisionThreshold)
        {
            ChangeState<PrecisionState>(solider);
        }

        if (m_Weapon) m_Weapon.hitRate = m_Weapon.weaponObject.dynamicSector;
    }
}
/// <summary>
/// 设计状态，命中率提高
/// </summary>
public class PrecisionState : FsmState<Soldier>
{
    private Weapon m_Weapon;
    private Soldier _soldier;

    protected override void OnEnter(IFsm<Soldier> solider)
    {
        base.OnEnter(solider);
        _soldier = solider.Owner;
        m_Weapon = _soldier.m_Weapon;
    }

    protected override void OnUpdate(IFsm<Soldier> solider, float elapseSeconds, float realElapseSeconds)
    {
        base.OnUpdate(solider, elapseSeconds, realElapseSeconds);
        if (solider.Owner._rigidbody.velocity.magnitude >= _soldier._precisionThreshold)
        {
            ChangeState<RunningState>(solider);
        }

        if (!m_Weapon) return;
        float t = _soldier._rigidbody.velocity.magnitude / _soldier._precisionThreshold;
        m_Weapon.hitRate = Mathf.Lerp(m_Weapon.weaponObject.staticSector, m_Weapon.weaponObject.dynamicSector, t);
    }
}


public class Soldier : TargetableObject
{
    public IFsm<Soldier> soldierFsm;
    public Weapon m_Weapon;
    public Rigidbody2D _rigidbody;
    public float _precisionThreshold => 0.2f;


    private float _currMaxSpeed = 1f;
    private float _currWeight => 1f;

    private SoldierData m_SoldierData;


    public override ImpactData GetImpactData()
    {
        throw new System.NotImplementedException();
    }


    protected override void OnShow(object userData)
    {
        base.OnShow(userData);
        m_SoldierData = userData as SoldierData;
        if (m_SoldierData == null)
        {
            return;
        }

        FsmState<Soldier>[] soliderStates =
        {
            new PreparingState(), new RunningState(), new PrecisionState()
        };
        soldierFsm = MyGameEntry.Fsm.CreateFsm(EntityId.ToString(), this, soliderStates);
        soldierFsm.Start<PreparingState>();

        _rigidbody = GetComponent<Rigidbody2D>();
        _rigidbody.drag = _currWeight;

        MyGameEntry.Entity.ShowWeapon(m_SoldierData.WeaponData);
        MyGameEntry.HPBar.ShowHPBar(this,m_SoldierData.Hp,m_SoldierData.MaxHp,0);
    }


    protected override void OnDead(Entity attacker)
    {
        MyGameEntry.Fsm.DestroyFsm(soldierFsm);
        base.OnDead(attacker);
    }

    protected override void OnAttached(EntityLogic childEntity, Transform parentTransform, object userData)
    {
        base.OnAttached(childEntity, parentTransform, userData);
        if (childEntity is Weapon m)
        {
            m_Weapon = m;
            return;
        }
    }


    protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
    {
        base.OnUpdate(elapseSeconds, realElapseSeconds);
        
        float maxDis=AIUtility.GetNearset("Enemy", this, out var t);
        
        if (maxDis<5f)
        {
            target = t;
        }
        
        if (target)
        {
            CachedTransform.up = target.CachedTransform.position - transform.position;
            TryAttack();
        }
        else
        {
            CachedTransform.up = _rigidbody.velocity.normalized;
        }
    }
    
    


    private void LateUpdate()
    {
        if (soldierFsm.CurrentState.GetType() == typeof(PreparingState))
        {
            return;
        }

        float curSpeed = Mathf.Clamp(_rigidbody.velocity.magnitude, 0, _currMaxSpeed);
        Vector2 curDirection = _rigidbody.velocity.normalized;
        _rigidbody.velocity = curSpeed * curDirection;
    }

    private void TryAttack()
    {
        m_Weapon.TryAttack(target);
    }
    public void Pushed(Vector2 force)
    {
        _rigidbody.AddForce(force, ForceMode2D.Impulse);
    }
}