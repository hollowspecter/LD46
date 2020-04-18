using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickableMesh : MonoBehaviour
{
    Rigidbody rigid;
    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
    }
}
