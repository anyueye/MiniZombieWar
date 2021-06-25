using System;
using GameFramework.Entity;
using GameFramework.Fsm;
using UnityEngine;
using UnityGameFramework.Runtime;

/// <summary>
/// 游荡状态，视野范围为锥形和听力范围圆形
/// </summary>
public class Wandering : FsmState<Zombie>
{
    //开始漫无目的的游荡
    protected override void OnEnter(IFsm<Zombie> fsm)
    {
        base.OnEnter(fsm);
        fsm.Owner.target = null;
    }

    //持续监测目标,当有目标切换到追踪状态
    protected override void OnUpdate(IFsm<Zombie> fsm, float elapseSeconds, float realElapseSeconds)
    {
        base.OnUpdate(fsm, elapseSeconds, realElapseSeconds);
        var pg = MyGameEntry.Entity.GetEntityGroup("Player").GetAllEntities();
        float maxDis = Single.MaxValue;
        TargetableObject nearestTaget = default;
        foreach (var entity in pg)
        {
            var t = (UnityGameFramework.Runtime.Entity) entity;
            var  distance = Vector2.Distance(t.Logic.CachedTransform.position, fsm.Owner.CachedTransform.position);
            if (distance<maxDis)
            {
                nearestTaget = t.Logic as TargetableObject;
                maxDis = distance;
            }
        }

        if (maxDis<fsm.Owner.ZombieData.PursueRange)
        {
            fsm.Owner.target = nearestTaget;
            ChangeState<Pursuing>(fsm);
        }
    }
}

/// <summary>
/// 追踪状态
/// </summary>
public class Pursuing : FsmState<Zombie>
{
    protected override void OnEnter(IFsm<Zombie> fsm)
    {
        base.OnEnter(fsm);
    }

    //持续追击目标，当目标进入攻击范围，切换攻击状态，当脱离追击范围，则切回游荡状态
    protected override void OnUpdate(IFsm<Zombie> fsm, float elapseSeconds, float realElapseSeconds)
    {
        base.OnUpdate(fsm, elapseSeconds, realElapseSeconds);
        if (fsm.Owner.target==null||Vector3.Distance(fsm.Owner.CachedTransform.position,fsm.Owner.target.CachedTransform.position)>fsm.Owner.ZombieData.PursueRange)
        {
            ChangeState<Wandering>(fsm);
        }
        if (Vector3.Distance(fsm.Owner.CachedTransform.position,fsm.Owner.target.CachedTransform.position)<=fsm.Owner.ZombieData.AttackRange)
        {
            ChangeState<Attacking>(fsm);
        }

       

        var zombiePos = fsm.Owner.CachedTransform.position;
        var soldierPos = fsm.Owner.target.CachedTransform.position;
        var dic = soldierPos - zombiePos;
        fsm.Owner.CachedTransform.up = dic.normalized;
        zombiePos = Vector3.MoveTowards(zombiePos, soldierPos, 0.5f * Time.deltaTime);
        fsm.Owner.CachedTransform.position = zombiePos;
    }
}

/// <summary>
/// 攻击状态，如果目标在攻击范围内攻击，否则切换到追击状态
/// </summary>
public class Attacking : FsmState<Zombie>
{
    protected override void OnInit(IFsm<Zombie> fsm)
    {
        base.OnInit(fsm);
        timer = Time.time;
    }

    private float timer = 0;
    //每次进入攻击状态，重置攻击间隔
    protected override void OnEnter(IFsm<Zombie> fsm)
    {
        base.OnEnter(fsm);
        
    }

    //根据攻击间隔 持续攻击，脱离攻击范围则切换到追击
    protected override void OnUpdate(IFsm<Zombie> fsm, float elapseSeconds, float realElapseSeconds)
    {
        base.OnUpdate(fsm, elapseSeconds, realElapseSeconds);
        if (fsm.Owner.target==null)
        {
            ChangeState<Wandering>(fsm);
        }
        if (Vector3.Distance(fsm.Owner.CachedTransform.position,fsm.Owner.target.CachedTransform.position)>fsm.Owner.ZombieData.AttackRange)
        {
            ChangeState<Pursuing>(fsm);
        }

        if (Time.time> timer+fsm.Owner.ZombieData.AtkInterval)
        {
            fsm.Owner.target.ApplyDamage(fsm.Owner,fsm.Owner.ZombieData.Attack);
            timer = Time.time;
        }
    }
}

public class Zombie : TargetableObject
{
    public ZombieData ZombieData => m_ZombieData;
    
    private IFsm<Zombie> zombieFsm;
    private ZombieData m_ZombieData = null;
    
    public override ImpactData GetImpactData()
    {
        throw new System.NotImplementedException();
    }

    protected override void OnShow(object userData)
    {
        base.OnShow(userData);
        m_ZombieData = userData as ZombieData;
        if (m_ZombieData == null)
        {
            return;
        }

        FsmState<Zombie>[] zombieStates =
        {
            new Wandering(), new Pursuing(), new Attacking()
        };
        zombieFsm = MyGameEntry.Fsm.CreateFsm(EntityId.ToString(), this, zombieStates);
        zombieFsm.Start<Wandering>();
        
        MyGameEntry.HPBar.ShowHPBar(this, m_ZombieData.Hp, m_ZombieData.MaxHp, 0);
    }

    protected override void OnDead(Entity attacker)
    {
        MyGameEntry.Fsm.DestroyFsm(zombieFsm);
        target = null;
        base.OnDead(attacker);
    }
}