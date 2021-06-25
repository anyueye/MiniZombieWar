
using GameFramework.Event;
using UnityEngine;
using UnityGameFramework.Runtime;

public class GameBase
{
    public bool GameOver
    {
        get;
        private set;
    }

    private Soldier _player;

    public virtual void Initialize()
    {
        MyGameEntry.Event.Subscribe(ShowEntitySuccessEventArgs.EventId,OnShowEntitySuccess);
        MyGameEntry.Event.Subscribe(ShowEntityFailureEventArgs.EventId,OnShowEntityFailure);
        MyGameEntry.Entity.ShowPlayer(new SoldierData(MyGameEntry.Entity.GenerateSerialId(),10000)
        {
            Position = Vector3.zero,
        });

        // for (int i = 0; i < 10; i++)
        // {
        MyGameEntry.Entity.ShowEnemy(new ZombieData(MyGameEntry.Entity.GenerateSerialId(),20000)
        {
            Position = new Vector3(Random.Range(-1f,1f),Random.Range(3f,4f),0),
        });
        // }
    }

    private void OnShowEntityFailure(object sender, GameEventArgs e)
    {
        ShowEntityFailureEventArgs ne = (ShowEntityFailureEventArgs)e;
        Log.Warning("Show entity failure with error message '{0}'.", ne.ErrorMessage);
    }

    private void OnShowEntitySuccess(object sender, GameEventArgs e)
    {
        ShowEntitySuccessEventArgs ne = (ShowEntitySuccessEventArgs) e;
        if (ne.EntityLogicType == typeof(Soldier))
        {
            _player = (Soldier) ne.Entity.Logic;
        }
    }

    public virtual void Update(float elapseSeconds, float realEpapseSeconds)
    {
        if (_player!=null&&_player.IsDead)
        {
            // GameOver = true;
            return;
        }
    }
    public virtual void Shutdown()
    {
        MyGameEntry.Event.Unsubscribe(ShowEntitySuccessEventArgs.EventId, OnShowEntitySuccess);
        MyGameEntry.Event.Unsubscribe(ShowEntityFailureEventArgs.EventId, OnShowEntityFailure);
    }
}
