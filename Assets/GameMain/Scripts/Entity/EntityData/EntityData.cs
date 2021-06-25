using System;
using Unity.Mathematics;
using UnityEngine;

[Serializable]
public abstract class EntityData
{
    [SerializeField] private int _id = 0;
    [SerializeField] private int _tableId = 0;
    [SerializeField] private Vector3 _position=Vector3.zero;
    [SerializeField] private Quaternion _rotation=quaternion.identity;

    public EntityData(int entityId, int tableId)
    {
        _id = entityId;
        _tableId = tableId;
    }
    /// <summary>
    /// EntityId
    /// </summary>
    public int EntityId
    {
        get => _id;
        set => _id = value;
    }
    /// <summary>
    /// 数据表ID
    /// </summary>
    public int TableId
    {
        get => _tableId;
        set => _tableId = value;
    }

    public Vector3 Position
    {
        get => _position;
        set => _position = value;
    }

    public Quaternion Rotation
    {
        get => _rotation;
        set => _rotation = value;
    }
}
