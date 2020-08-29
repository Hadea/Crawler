﻿using UnityEngine;

public class DestroyAfterTime : MonoBehaviour
{
    public float timeToLive = 1;
    private float spawnTime;

    void Start()
    {
        spawnTime = Time.time;
    }

    void Update()
    {
        if (spawnTime + timeToLive < Time.time)
        {
            Destroy(this.gameObject);
        }
    }
}
