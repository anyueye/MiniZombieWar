
using System;
using GameMain.Scripts.Utility;
using UnityEngine;

public class Bullet:Entity
{
    [SerializeField] private BulletData m_BulletData = null;

    private BulletAminate bulletAnim;
    private float _cacheLiftTime ;
    public ImpactData GetImpactData()
    {
        return new ImpactData( 0, m_BulletData.Attack, 0);
    }

    protected override void OnShow(object userData)
    {
        base.OnShow(userData);
        m_BulletData=userData as BulletData;
        if (m_BulletData==null)
        {
            return;
        }

        _cacheLiftTime = 1f;
        bulletAnim = GetComponentInChildren<BulletAminate>();
        bulletAnim.Show();
        Raycast();
    }

    protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
    {
        base.OnUpdate(elapseSeconds, realElapseSeconds);
        if (_cacheLiftTime <= 0)
        {
            MyGameEntry.Entity.HideEntity(this);
        }

        _cacheLiftTime -= elapseSeconds;
    }

    void Raycast()
    {
        var dis = 0f;
        float propMult = m_BulletData.MaxLength*0.1f;
        var hit = Physics2D.Raycast(CachedTransform.position, CachedTransform.up, m_BulletData.MaxLength,LayerMask.GetMask($"Enemy"));
        if (hit.collider!=null)
        {
            dis = Vector2.Distance(CachedTransform.position, hit.point);
            propMult = dis * 0.1f;
            ApplyDamage(hit);
        }
        else
        {
            dis = m_BulletData.MaxLength;
        }
        bulletAnim.lineRenderer.SetPosition(1,new Vector3(0,0,dis));
        bulletAnim.lineRenderer.material.SetTextureScale("_MainTex", new Vector2(propMult, 1f));
    }

    protected void ApplyDamage(RaycastHit2D hit)
    {
        if (!hit.collider) return;
        TargetableObject target = hit.collider.GetComponent<TargetableObject>();
        target.ApplyDamage(this, m_BulletData.Attack);
    }
}
