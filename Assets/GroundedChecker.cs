using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundedChecker : MonoBehaviour
{
    public bool isGrounded = false;
    private void OnTriggerEnter(Collider other)
    {
        if(other.transform.root != transform.root)
            isGrounded = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform.root != transform.root)
            isGrounded = false;
    }
}
