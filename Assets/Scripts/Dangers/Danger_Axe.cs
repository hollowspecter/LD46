using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Danger_Axe : Danger
{
    Rigidbody rigid;
    public float throwSpeed = 50.0f;
    void Awake()
    {
        rigid = GetComponent<Rigidbody>();
    }
    // Start is called before the first frame update
    protected override void StartDanger()
    {
        rigid.AddForceAtPosition(throwSpeed * transform.forward, transform.position + transform.up * (1.0f), ForceMode.Impulse);
    }

    private void OnDrawGizmosSelected()
    {
        if (rigid == null)
            rigid = GetComponent<Rigidbody>();
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, transform.forward * throwSpeed/6.0f);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(rigid.worldCenterOfMass, 0.2f);
    }
}
