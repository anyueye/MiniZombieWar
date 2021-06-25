
using GameFramework.Fsm;
using GameFramework.Procedure;

public class ProcedureGame:ProcedureBase
{
    public override bool UseNativeDialog { get; }

    private GameBase _currenGame = null;

    protected override void OnInit(IFsm<IProcedureManager> procedureOwner)
    {
        base.OnInit(procedureOwner);
        _currenGame = new GameBase();
    }

    protected override void OnDestroy(IFsm<IProcedureManager> procedureOwner)
    {
        base.OnDestroy(procedureOwner);
    }

    protected override void OnEnter(IFsm<IProcedureManager> procedureOwner)
    {
        base.OnEnter(procedureOwner);
        _currenGame.Initialize();
    }

    protected override void OnLeave(IFsm<IProcedureManager> procedureOwner, bool isShutdown)
    {
        if (_currenGame != null)
        {
            _currenGame.Shutdown();
            _currenGame = null;
        }
        base.OnLeave(procedureOwner, isShutdown);
    }

    protected override void OnUpdate(IFsm<IProcedureManager> procedureOwner, float elapseSeconds, float realElapseSeconds)
    {
        base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);
        if (_currenGame!=null&&!_currenGame.GameOver)
        {
            _currenGame.Update(elapseSeconds, realElapseSeconds);
            return;
        }
        
    }
}
