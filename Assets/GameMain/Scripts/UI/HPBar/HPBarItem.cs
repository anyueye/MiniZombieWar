//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2021 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

using System;
using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;


public class HPBarItem : MonoBehaviour
{
#pragma warning disable 649
    [SerializeField] private Image hpBar;
    [SerializeField] private Image hpBarBackground;
#pragma warning restore 649
    private int maxValue;

    private Canvas m_ParentCanvas = null;
    private RectTransform m_CachedTransform = null;
    private Entity m_Owner = null;
    private int m_OwnerId = 0;

    public Entity Owner
    {
        get { return m_Owner; }
    }

    public void Init(Entity owner, Canvas parentCanvas, int hp, int maxHp, int shield)
    {
        if (owner == null)
        {
            Log.Error("Owner is invalid.");
            return;
        }

        m_ParentCanvas = parentCanvas;

        gameObject.SetActive(true);


        if (m_Owner != owner || m_OwnerId != owner.EntityId)
        {
            maxValue = maxHp;
            var newValue = hp / (float) maxValue;
            hpBar.fillAmount = newValue;
            m_Owner = owner;
            m_OwnerId = owner.EntityId;
            Refresh();
        }
        else
        {
            SetHp(hp);
        }

        SetShield(shield);
    }

    private void SetHp(int value)
    {
        var newValue = value / (float) maxValue;
        hpBar.DOFillAmount(newValue, 0.2f)
            .SetEase(Ease.InSine);

        // var seq = DOTween.Sequence();
        // seq.AppendInterval(0.5f);
        // seq.Append(hpBarBackground.DOFillAmount(newValue, 0.2f));
        // seq.SetEase(Ease.InSine);
        
    }

    private void Update()
    {
        Refresh();
    }

    private void SetShield(int value)
    {
        SetShieldActive(value > 0);
    }

    private void SetShieldActive(bool shieldActive)
    {
        
    }

    private bool Refresh()
    {
        if (m_Owner == null || !Owner.Available || Owner.EntityId != m_OwnerId) return true;
        // var pivot = m_Owner.CachedTransform;
        // var canvasPos = MyGameEntry.Scene.MainCamera.WorldToViewportPoint(pivot.position + new Vector3(0, 0, 0));
        // m_CachedTransform.anchorMin = m_CachedTransform.anchorMax = canvasPos;
        Vector3 worldPosition = m_Owner.CachedTransform.position + Vector3.down*0.15f;
        Vector3 screenPosition = MyGameEntry.Scene.MainCamera.WorldToScreenPoint(worldPosition);

        Vector2 position;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle((RectTransform)m_ParentCanvas.transform, screenPosition,
            m_ParentCanvas.worldCamera, out position))
        {
            m_CachedTransform.localPosition = position;
        }
        return true;
    }

    public void Reset()
    {
        m_Owner = null;
        gameObject.SetActive(false);
    }


    private void Awake()
    {
        m_CachedTransform = GetComponent<RectTransform>();
        if (m_CachedTransform == null)
        {
            Log.Error("RectTransform is invalid.");
            return;
        }
    }
}