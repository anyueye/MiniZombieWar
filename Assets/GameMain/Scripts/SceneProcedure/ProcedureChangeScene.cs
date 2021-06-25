
using GameFramework.Event;
using GameFramework.Fsm;
using GameFramework.Procedure;
using UnityGameFramework.Runtime;

public class ProcedureChangeScene:ProcedureBase
{
    public override bool UseNativeDialog { get; }
    protected override void OnEnter(IFsm<IProcedureManager> procedureOwner)
    {
        base.OnEnter(procedureOwner);
        MyGameEntry.Event.Subscribe(LoadSceneSuccessEventArgs.EventId, OnLoadSceneSuccess);
        MyGameEntry.Event.Subscribe(LoadSceneFailureEventArgs.EventId, OnLoadSceneFailure);

        var loadedSceneAssetNames = MyGameEntry.Scene.GetLoadedSceneAssetNames();
        for (int i = 0; i < loadedSceneAssetNames.Length; i++)
        {
            MyGameEntry.Scene.UnloadScene(loadedSceneAssetNames[i]);
        }
        
        
        string sceneName = procedureOwner.GetData<VarString>("NextSceneId");
        m_ChangeToMenu = sceneName == "Scene.Menu";
        MyGameEntry.Scene.LoadScene(AssetUtility.GetSceneAsset(sceneName), Constant.AssetPriority.SceneAsset, this);
    }
    private void OnLoadSceneSuccess(object sender, GameEventArgs e)
    {
        LoadSceneSuccessEventArgs ne = (LoadSceneSuccessEventArgs)e;
        if (ne.UserData != this)
        {
            return;
        }

        Log.Info("Load scene '{0}' OK.", ne.SceneAssetName);

        m_IsChangeSceneComplete = true;
    }
    private void OnLoadSceneFailure(object sender, GameEventArgs e)
    {
        LoadSceneFailureEventArgs ne = (LoadSceneFailureEventArgs)e;
        if (ne.UserData != this)
        {
            return;
        }

        Log.Error("Load scene '{0}' failure, error message '{1}'.", ne.SceneAssetName, ne.ErrorMessage);
    }

    protected override void OnLeave(IFsm<IProcedureManager> procedureOwner, bool isShutdown)
    {
        MyGameEntry.Event.Unsubscribe(LoadSceneSuccessEventArgs.EventId, OnLoadSceneSuccess);
        MyGameEntry.Event.Unsubscribe(LoadSceneFailureEventArgs.EventId, OnLoadSceneFailure);

        base.OnLeave(procedureOwner, isShutdown);
    }

    private bool m_ChangeToMenu;
    private bool m_IsChangeSceneComplete = false;
    protected override void OnUpdate(IFsm<IProcedureManager> procedureOwner, float elapseSeconds, float realElapseSeconds)
    {
        base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);
        if (!m_IsChangeSceneComplete)
        {
            return;
        }
        if (m_ChangeToMenu)
        {
            ChangeState<ProcedureMenu>(procedureOwner);
        }
        else
        {
            ChangeState<ProcedureGame>(procedureOwner);
        }
    }
    
}
