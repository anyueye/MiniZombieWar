using System.Collections.Generic;
using System.Linq;
using GameFramework.Event;
using GameFramework.Fsm;
using GameFramework.Procedure;
using UnityGameFramework.Runtime;


public class ProcedurePreload : ProcedureBase
{
    private readonly Dictionary<string, bool> m_loadedFlag = new Dictionary<string, bool>();

    public static readonly string[] DataTableNames =
    {
        "Soldier",
        "Entity",
        "Weapon",
        "Enemy"
    };

    public override bool UseNativeDialog { get; }

    protected override void OnEnter(IFsm<IProcedureManager> procedureOwner)
    {
        base.OnEnter(procedureOwner);
        MyGameEntry.Event.Subscribe(LoadDataTableSuccessEventArgs.EventId, OnLoadDataTableSuccess);
        MyGameEntry.Event.Subscribe(LoadDataTableFailureEventArgs.EventId, OnLoadDataTableFailure);


        m_loadedFlag.Clear();
        foreach (var tableName in DataTableNames)
        {
            LoadDataTable(tableName);
        }
    }

    private void OnLoadDataTableFailure(object sender, GameEventArgs e)
    {
        LoadDataTableFailureEventArgs ne = (LoadDataTableFailureEventArgs) e;
        if (ne.UserData != this)
        {
            return;
        }

        Log.Error("Can not load data table '{0}' from '{1}' with error message '{2}'.", ne.DataTableAssetName, ne.DataTableAssetName, ne.ErrorMessage);
    }

    private void OnLoadDataTableSuccess(object sender, GameEventArgs e)
    {
        LoadDataTableSuccessEventArgs ne = (LoadDataTableSuccessEventArgs) e;
        if (ne.UserData != this)
        {
            return;
        }

        m_loadedFlag[ne.DataTableAssetName] = true;
        Log.Info("Load data table '{0}' OK.", ne.DataTableAssetName);
    }

    private void LoadDataTable(string dataTableName)
    {
        var dataTableAssetName = AssetUtility.GetDataTableAsset(dataTableName, false);
        m_loadedFlag.Add(dataTableAssetName, false);
        MyGameEntry.DataTable.LoadDataTable(dataTableName, dataTableAssetName, this);
    }

    protected override void OnUpdate(IFsm<IProcedureManager> procedureOwner, float elapseSeconds, float realElapseSeconds)
    {
        base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);
        if (m_loadedFlag.Any(b => !b.Value))
        {
            return;
        }

        procedureOwner.SetData<VarString>("NextSceneId", "Scene.Menu");
        ChangeState<ProcedureChangeScene>(procedureOwner);
    }
}