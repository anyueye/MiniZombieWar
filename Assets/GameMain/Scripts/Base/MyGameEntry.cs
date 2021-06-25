using UnityEngine;
using UnityGameFramework.Runtime;


public class MyGameEntry : MonoBehaviour
{
    //
    public static ProcedureComponent Procedure;
    public static SceneComponent Scene;
    public static EventComponent Event;
    public static EntityComponent Entity;
    public static FsmComponent Fsm;
    public static DataTableComponent DataTable;

    public static ObjectPoolComponent ObjectPool;
    //
    public static InputComponent Input;
    public static HPBarComponent HPBar;

    // Start is called before the first frame update
    void Start()
    {
        Procedure = GameEntry.GetComponent<ProcedureComponent>();
        Scene = GameEntry.GetComponent<SceneComponent>();
        Event = GameEntry.GetComponent<EventComponent>();
        Entity = GameEntry.GetComponent<EntityComponent>();
        DataTable = GameEntry.GetComponent<DataTableComponent>();
        Fsm = GameEntry.GetComponent<FsmComponent>();
        ObjectPool = GameEntry.GetComponent<ObjectPoolComponent>();

        Input = GameEntry.GetComponent<InputComponent>();
        HPBar = GameEntry.GetComponent<HPBarComponent>();
    }
}