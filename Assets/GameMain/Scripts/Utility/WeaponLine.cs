using System;
using UnityEngine;


public class WeaponLine : MonoBehaviour
{
    public WeaponObject data;

    [SerializeField] private LineRenderer rangeLine;

    [SerializeField] private LineRenderer rateLine;

    private void Awake()
    {
        rangeLine.startColor = rangeLine.endColor = data.rangeColor;
        
        rateLine.startColor = rateLine.endColor = data.hitRateColor;
        rateLine.positionCount = 3;
    }

    public void DrawWireSemicircle(Transform attacker, float radius, float angle)
    {
        Vector3 leftPoint = attacker.position + Quaternion.AngleAxis(-angle / 2, Vector3.back) * attacker.up * radius;
        Vector3 rightPoint = attacker.position + Quaternion.AngleAxis(angle / 2, Vector3.back) * attacker.up * radius;

        if (angle != 360)
        {
            rateLine.SetPosition(0, leftPoint);
            rateLine.SetPosition(1, attacker.position);
            rateLine.SetPosition(2, rightPoint);
        }
    }

    public void DrawAttackRange(Transform attceker, float radius)
    {
        int pointAmount = 100;
        float eachAngle = 360f / pointAmount;
        Vector3 forwad = attceker.up;
        rangeLine.positionCount = pointAmount + 1;
        for (int i = 0; i < rangeLine.positionCount; i++)
        {
            Vector3 pos = Quaternion.Euler(0f, 0f, eachAngle * i) * forwad * radius + attceker.position;
            rangeLine.SetPosition(i, pos);
        }
    }
}