using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Danger_Cannon : Danger
{
    public GameObject ammoPrefab;
    public Transform spawnPosition;
    public ParticleSystem pSystem;
    public float cannonPower = 10.0f;

    protected override void StartDanger()
    {
        Rigidbody rigid = GameObject.Instantiate(ammoPrefab, spawnPosition.position, spawnPosition.rotation).GetComponent<Rigidbody>();
        rigid.AddForce(spawnPosition.forward * cannonPower, ForceMode.Impulse);
        if(pSystem)
            pSystem.Play();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(spawnPosition.position, spawnPosition.forward * 50.0f);
    }
}
