
using GameFramework;
using UnityEngine;
using UnityGameFramework.Runtime;

public class Entity:EntityLogic
{
    [SerializeField]
    private EntityData _entityData;
    
    public int EntityId => _entityData.EntityId;
    
    protected override void OnShow(object userData)
    {
        base.OnShow(userData);
        _entityData=userData as EntityData;
        if (_entityData==null)
        {
            return;
        }
        Name = Utility.Text.Format($"[Entity {EntityId}]");
        CachedTransform.localPosition = _entityData.Position;
        CachedTransform.localRotation = _entityData.Rotation;
        CachedTransform.localScale = Vector3.one;
    }
}
