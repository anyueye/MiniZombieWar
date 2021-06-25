using System;
using UnityEngine;

namespace GameMain.Scripts.Utility
{
    public class BulletAminate : MonoBehaviour
    {
        public Texture[] BeamFrames; 
        public float FrameStep;
        int frameIndex;
        public LineRenderer lineRenderer=>GetComponent<LineRenderer>();


        private float timer;
        public void Show()
        {
            frameIndex = 0;
            if (BeamFrames.Length>0)
            {
                lineRenderer.material.mainTexture = BeamFrames[0];
            }
            timer = Time.time;
        }
        
        public void Update()
        {
            if (frameIndex==BeamFrames.Length-1) return;
            if (Time.time>timer+FrameStep)
            {
                frameIndex++;
                lineRenderer.material.mainTexture = BeamFrames[frameIndex];
                timer = Time.time;
            }
        }
    }
}