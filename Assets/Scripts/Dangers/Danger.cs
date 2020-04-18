using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Danger : MonoBehaviour
{
    public float delay = 0.0f;
    protected bool hasStarted = false;

    public virtual void Start()
    {
        StartCoroutine(DelayDangerStart());
    }

    IEnumerator DelayDangerStart()
    {
        yield return new WaitForSeconds(delay);
        hasStarted = true;
        StartDanger();
    }
    protected abstract void StartDanger();
}
