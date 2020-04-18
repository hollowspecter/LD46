using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Danger : MonoBehaviour
{
    public bool isPaused = true;
    protected virtual void Update()
    {
        if(isPaused)
        {
            PausedUpdate();
        }
        else
        {
            UnpausedUpdate();
        }
    }

    public abstract void Pause();
    public abstract void Unpause();

    // Update is called once per frame
    protected abstract void PausedUpdate();

    protected abstract void UnpausedUpdate();
}
