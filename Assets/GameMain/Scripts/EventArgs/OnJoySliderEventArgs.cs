
using GameFramework;
using GameFramework.Event;
using UnityEngine;

public class OnJoySliderEventArgs:GameEventArgs
{
    public static readonly int EventId = typeof(OnJoySliderEventArgs).GetHashCode();

    public Vector2 SliderDir = Vector2.zero;
    public object UserData;

    public static OnJoySliderEventArgs Create(Vector2 sliderDir)
    {
        OnJoySliderEventArgs joySliderEventEventArgs = ReferencePool.Acquire<OnJoySliderEventArgs>();
        joySliderEventEventArgs.SliderDir = sliderDir;
        return joySliderEventEventArgs;
    }
    
    public override void Clear()
    {
        SliderDir = Vector2.zero;
        UserData = null;
    }

    public override int Id =>EventId;
}
