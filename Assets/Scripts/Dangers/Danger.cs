using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Danger : MonoBehaviour
{
    public float delay = 0.0f;

    public virtual void Start()
    {
        StartCoroutine(DelayDangerStart());
    }

    IEnumerator DelayDangerStart()
    {
        yield return new WaitForSeconds(delay);
        StartDanger();
    }
    protected abstract void StartDanger();
}
