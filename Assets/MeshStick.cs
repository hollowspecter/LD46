using System.Collections;
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
        Debug.Log("CollisionEnter");
        if(collision.rigidbody != null)
        {
            Stick(collision.rigidbody);
        }
    }
    public void Stick(Rigidbody r)
    {
        isSticking = true;
        FixedJoint f = ownRigid.gameObject.AddComponent<FixedJoint>();
        f.connectedBody = r;
    }
}
