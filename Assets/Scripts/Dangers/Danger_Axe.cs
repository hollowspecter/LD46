using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Danger_Axe : Danger
{
    Rigidbody rigid;
    public float rotationSpeed = 50.0f;
    public float throwSpeed = 50.0f;
    public override void Pause()
    {
        throw new System.NotImplementedException();
    }

    public override void Unpause()
    {
        throw new System.NotImplementedException();
    }

    protected override void PausedUpdate()
    {
        throw new System.NotImplementedException();
    }

    protected override void UnpausedUpdate()
    {
        throw new System.NotImplementedException();
    }
    void Awake()
    {
        rigid = GetComponent<Rigidbody>();
    }
    // Start is called before the first frame update
    void Start()
    {
        rigid.angularVelocity = rotationSpeed * transform.right;
        rigid.AddForce(throwSpeed * transform.forward,ForceMode.Impulse);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
