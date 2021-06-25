
using System;
using UnityEngine;

public abstract class TargetableObject:Entity
{
    [SerializeField] private TargetableObjectData _targetableObjectData;
    public TargetableObject target;
    public bool IsDead => _targetableObjectData.Hp <= 0;

    public abstract ImpactData GetImpactData();

    public void ApplyDamage(Entity attacker, int damageHp)
    {
        _targetableObjectData.Hp -= damageHp;
        MyGameEntry.HPBar.ShowHPBar(this, _targetableObjectData.Hp, _targetableObjectData.MaxHp,0);
        
        if (IsDead)
        {
            OnDead(attacker);
        }
    }

    protected override void OnShow(object userData)
    {
        base.OnShow(userData);
        _targetableObjectData = userData as TargetableObjectData;
        if (_targetableObjectData==null)
        {
            return;
        }
    }

    protected virtual void OnDead(Entity attacker)
    {
        MyGameEntry.Entity.HideEntity(this);
       
    }

    protected override void OnHide(bool isShutdown, object userData)
    {
        base.OnHide(isShutdown, userData);
        MyGameEntry.HPBar.HideHPBar(this);
    }

    // private void OnTriggerEnter2D(Collider2D other)
    // {
    //     Entity entity = other.GetComponent<Entity>();
    //     if (entity==null)
    //     {
    //         return;
    //     }
    //
    //     if (entity is TargetableObject && entity.EntityId>=EntityId)
    //     {
    //         return;
    //     }
    //
    //     var e = entity as TargetableObject;
    //     if (e!=null)
    //     {
    //         target = e;
    //     }
    // }
}
