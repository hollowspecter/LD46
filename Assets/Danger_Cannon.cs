using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Danger_Cannon : Danger
{
    public Rigidbody cannonBallRigid;
    public Transform barrel;
    public ParticleSystem pSystem;
    public float cannonPower = 10.0f;

    protected override void StartDanger()
    {
        cannonBallRigid.gameObject.SetActive(true);
        cannonBallRigid.isKinematic = false;
        cannonBallRigid.AddForce(-barrel.forward * cannonPower, ForceMode.Impulse);
        pSystem.Play();
    }
}
