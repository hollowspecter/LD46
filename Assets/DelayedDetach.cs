using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DelayedDetach : MonoBehaviour
{
    public float delay = 0.5f;

    private float t = 0.0f;
    FixedJoint f;

    private void Awake()
    {
        f = GetComponent<FixedJoint>();
    }

    // Update is called once per frame
    void Update()
    {
        if(t >= delay && f!= null)
        {
            Destroy(f);
            Destroy(this);
        }
        t += Time.deltaTime;
    }
}
