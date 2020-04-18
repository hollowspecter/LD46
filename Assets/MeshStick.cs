﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MeshStick : MonoBehaviour
{
    public Rigidbody ownRigid;
    bool isSticking = false;
    private void OnCollisionEnter(Collision collision)
    {
        if (isSticking) return;
        if(collision.rigidbody != null)
        {
            Stick(collision.rigidbody);
        }
    }
    public void Stick(Rigidbody r)
    {
        isSticking = true;
        ownRigid.angularVelocity = Vector3.zero;
        ownRigid.velocity = Vector3.zero;
        FixedJoint f = r.gameObject.AddComponent<FixedJoint>();
        f.connectedBody = ownRigid;
    }
}
