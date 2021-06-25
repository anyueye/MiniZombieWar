using System;
using GameFramework;
using UnityEngine;
using UnityGameFramework.Runtime;
using Random = UnityEngine.Random;

public class Weapon : Entity
{
     private const string AttachPoint = "Weapon Point";
     [SerializeField] private WeaponData m_WeaponData = null;
     
     private float m_NextAttackTime = 0f;
     private WeaponLine _weaponLine;
     private Transform attackTrans;
     public WeaponObject weaponObject;
     public float hitRate;
     protected override void OnShow(object userData)
     {
          base.OnShow(userData);
          m_WeaponData=userData as WeaponData;
          if (m_WeaponData==null)
          {
               return;
          }

          _weaponLine = GetComponentInChildren<WeaponLine>();
          if (_weaponLine)
          {
               weaponObject = _weaponLine.data;
               hitRate = weaponObject.staticSector;
          }
          MyGameEntry.Entity.AttachEntity(Entity, m_WeaponData.OwnerId, AttachPoint);
     }

     protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
     {
          base.OnUpdate(elapseSeconds, realElapseSeconds);
          if (_weaponLine&&weaponObject)
          {
               _weaponLine.DrawAttackRange(attackTrans, weaponObject.attackRange);
               _weaponLine.DrawWireSemicircle(attackTrans, weaponObject.attackRange, hitRate);
          }
     }

     public virtual void TryAttack(TargetableObject target)
     {
          if (Time.time<m_NextAttackTime)
          {
               return;
          }

          m_NextAttackTime = Time.time + weaponObject.interval;
          MyGameEntry.Entity.ShowBullet(new BulletData(MyGameEntry.Entity.GenerateSerialId(),m_WeaponData.BulletId,m_WeaponData.OwnerId,10,weaponObject.attackRange)
          {
               Position = CachedTransform.position,
               Rotation = Quaternion.AngleAxis(Random.Range(-hitRate/2f,hitRate/2f),Vector3.back)*attackTrans.rotation,
          });
     }

     protected override void OnAttachTo(EntityLogic parentEntity, Transform parentTransform, object userData)
     {
          base.OnAttachTo(parentEntity, parentTransform, userData);
          Name = Utility.Text.Format("Weapon of {0}", parentEntity.Name);
          CachedTransform.localPosition = Vector3.zero;
          attackTrans = parentTransform;
     }
}