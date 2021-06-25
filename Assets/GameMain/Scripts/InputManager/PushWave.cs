using System;
using UnityEngine;

public class PushWave : MonoBehaviour
{
    public float maxForce;
    public float maxDistance;
    public float expandTime;
    
    
    private void Update()
    {
        if (transform.localScale.x<=maxDistance)
        {
            transform.localScale += maxDistance * Vector3.one * Time.deltaTime /expandTime;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Soldier soldierMover = other.GetComponent<Soldier>();
        if (soldierMover==null)
        {
            return;
        }
        Vector2 dir = other.transform.position-transform.position;
        float distance = dir.magnitude;
        soldierMover.Pushed(dir.normalized * (maxDistance - distance) / maxDistance * maxForce);
    }
}