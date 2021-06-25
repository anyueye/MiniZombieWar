
using UnityEngine;

public abstract class TargetableObjectData:EntityData
{
    [SerializeField]
    private int _hp;
    public TargetableObjectData(int entityId, int tableId) : base(entityId, tableId)
    {
        _hp = 0;
    }

    public int Hp
    {
        get => _hp;
        set => _hp = value;
    }

    public abstract int MaxHp
    {
        get;
    }

    public float HpRatio => MaxHp > 0 ? (float) Hp / MaxHp : 0;
}
