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
        if (fsm.Owner._rigidbody.velocity.magnitude>0)
        {
            ChangeState<RunningState>(fsm);
        }
    }
}

/// <summary>
/// 跑路状态，会掉血
/// </summary>
public class RunningState : FsmState<Soldier>
{



    protected override void OnEnter(IFsm<Soldier> fsm)
    {
        base.OnEnter(fsm);
        fsm.Owner._Sprite.color=Color.white;
    }

    protected override void OnUpdate(IFsm<Soldier> solider, float elapseSeconds, float realElapseSeconds)
    {
        base.OnUpdate(solider, elapseSeconds, realElapseSeconds);
        var runSpeed = solider.Owner._rigidbody.velocity.magnitude;
        if (runSpeed>0.7f)
        {
            ChangeState<AccumulateState>(solider);
        }
    }
}

public class AccumulateState : FsmState<Soldier>
{
    private float accumulateTime => 1.5f;

    private float timer;
    protected override void OnEnter(IFsm<Soldier> fsm)
    {
        base.OnEnter(fsm);
        fsm.Owner._Sprite.color=Color.green;
        timer = Time.time;
    }

    protected override void OnUpdate(IFsm<Soldier> fsm, float elapseSeconds, float realElapseSeconds)
    {
        base.OnUpdate(fsm, elapseSeconds, realElapseSeconds);
        var runSpeed = fsm.Owner._rigidbody.velocity.magnitude;
        if (runSpeed <=0.7f)
        {
            ChangeState<RunningState>(fsm);
        }
        if (Time.time>timer+accumulateTime)
        {
            ChangeState<RushState>(fsm);
        }
    }
}
/// <summary>
/// 冲锋状态 无敌
/// </summary>
public class RushState : FsmState<Soldier>
{
    private float exitTime => 0.5f;

    private float timer;
    private bool rushing;
    protected override void OnEnter(IFsm<Soldier> fsm)
    {
        base.OnEnter(fsm);
        fsm.Owner._Sprite.color=new Color(0f, 0.02f, 1f);
        rushing = true;
    }

    protected override void OnUpdate(IFsm<Soldier> fsm, float elapseSeconds, float realElapseSeconds)
    {
        base.OnUpdate(fsm, elapseSeconds, realElapseSeconds);
        
        var runSpeed = fsm.Owner._rigidbody.velocity.magnitude;
        if (runSpeed <=0.7f&&rushing)
        {
            rushing = false;
            timer = Time.time;
        }

        if (!rushing)
        {
            if (Time.time>timer+exitTime)
            {
                ChangeState<RunningState>(fsm);
            }
        }
    }
}



public class Soldier : TargetableObject
{
    public IFsm<Soldier> soldierFsm;
    public SpriteRenderer _Sprite;
    
    public Rigidbody2D _rigidbody;
    public float _precisionThreshold => 0.2f;


    private float _currMaxSpeed = 1f;
    private float _currWeight => 1f;

    private SoldierData m_SoldierData;


    public override ImpactData GetImpactData()
    {
        return soldierFsm.GetState<RushState>().Equals(soldierFsm.CurrentState) 
            ? new ImpactData(m_SoldierData.Hp, int.MaxValue, int.MaxValue) 
            : new ImpactData(m_SoldierData.Hp, 10, 0);
    }


    protected override void OnShow(object userData)
    {
        base.OnShow(userData);
        m_SoldierData = userData as SoldierData;
        if (m_SoldierData == null)
        {
            return;
        }

        _Sprite = GetComponentInChildren<SpriteRenderer>();

        soldierFsm = MyGameEntry.Fsm.CreateFsm(this,new PreparingState(),new RunningState(),new AccumulateState(),new RushState());
        soldierFsm.Start<PreparingState>();
        _rigidbody = GetComponent<Rigidbody2D>();
        _rigidbody.drag = _currWeight;
        
        MyGameEntry.HPBar.ShowHPBar(this,m_SoldierData.Hp,m_SoldierData.MaxHp,0);
    }


    protected override void OnDead(Entity attacker)
    {
        MyGameEntry.Fsm.DestroyFsm(soldierFsm);
        base.OnDead(attacker);
    }

    
    protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
    {
        base.OnUpdate(elapseSeconds, realElapseSeconds);
        
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
    
    public void Pushed(Vector2 force)
    {
        _rigidbody.AddForce(force, ForceMode2D.Impulse);
    }
}