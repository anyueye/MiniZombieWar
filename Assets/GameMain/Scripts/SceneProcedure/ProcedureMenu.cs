
    using GameFramework.Fsm;
    using GameFramework.Procedure;
    using UnityGameFramework.Runtime;

    public class ProcedureMenu:ProcedureBase
    {
        public override bool UseNativeDialog { get; }
        protected override void OnUpdate(IFsm<IProcedureManager> procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);
            procedureOwner.SetData<VarString>("NextSceneId", "Scene.Game");
            ChangeState<ProcedureChangeScene>(procedureOwner);
        }
    }
