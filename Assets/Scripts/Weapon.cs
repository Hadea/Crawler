using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    private Renderer[] renderers;

    protected virtual void Awake()
    {
        renderers = GetComponentsInChildren<Renderer>();
    }

    public void ToggleRenderers(bool toggle)
    {
        foreach (Renderer renderer in renderers)
        {
            renderer.enabled = toggle;
        }
    }
}
