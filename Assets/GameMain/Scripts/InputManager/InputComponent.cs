
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityGameFramework.Runtime;

public class InputComponent:GameFrameworkComponent
{
     public GameObject PushWavePrefab;
    [SerializeField] UnityEvent OnTouch;
    [SerializeField] float PushInterval;
    public int AllowFingers => 5;
    
    Transform[] fingerRoots;
    float[] touchTimers;
    
    private void Start()
    {
        fingerRoots = new Transform[AllowFingers];
        touchTimers = new float[AllowFingers];
        for (int i = 0; i < fingerRoots.Length; i++)
        {
            fingerRoots[i] = new GameObject("fingerRoot" + i.ToString()).transform;
            touchTimers[i] = 0f;
        }
    }

    
    void Update()
    {
        
        if (Input.touchCount!=0)
        {
            for (int i = 0; i < Mathf.Min(Input.touchCount, fingerRoots.Length); i++)
            {
                Touch touch = Input.GetTouch(i);
                Vector3 touchWorldPos;
                switch (touch.phase)
                {
                    case TouchPhase.Began:
                        touchTimers[i] = 0f;
                        touchWorldPos = MyGameEntry.Scene.MainCamera.ScreenToWorldPoint(touch.position) + Vector3.forward;
                        fingerRoots[i].position = touchWorldPos;
                        if (!EventSystem.current.IsPointerOverGameObject())
                        {
                            Instantiate(PushWavePrefab, touchWorldPos, Quaternion.identity).transform.parent = fingerRoots[i];
                            OnTouch.Invoke();
                        }
                       
                        break;
                    case TouchPhase.Moved:
                    case TouchPhase.Stationary:
                        touchWorldPos = MyGameEntry.Scene.MainCamera.ScreenToWorldPoint(touch.position) + Vector3.forward;
                        fingerRoots[i].position = touchWorldPos;
                        touchTimers[i] += Time.deltaTime;
                        if (touchTimers[i] > PushInterval)
                        {
                            touchTimers[i] = 0f;

                            if (!EventSystem.current.IsPointerOverGameObject())
                            {
                                Instantiate(PushWavePrefab, touchWorldPos, Quaternion.identity).transform.parent = fingerRoots[i];
                                OnTouch.Invoke();
                            }
                        }
                        break;
                    case TouchPhase.Ended:
                        for (int j = 0; j < fingerRoots[i].childCount; j++)
                        {
                            fingerRoots[i].GetChild(j).parent = null;
                        }
                        break;
                    case TouchPhase.Canceled:
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
