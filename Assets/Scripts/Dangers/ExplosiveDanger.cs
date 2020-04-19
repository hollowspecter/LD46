using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveDanger : MonoBehaviour
{
    bool hasExploded = false;
    public float explosionPower = 200f;
    public float explosionRadius = 10.0f;
    public ParticleSystem explosionParticles;
    Rigidbody rigid;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (hasExploded) return;
        Debug.Log("Explode " + collision.gameObject.name);
        Explode();
    }

    private void Explode()
    {
        hasExploded = true; 
        Collider[] colliders = Physics.OverlapSphere(rigid.position, explosionRadius);
        HashSet<Rigidbody> rbs = new HashSet<Rigidbody>();
        foreach (Collider hit in colliders)
        {
            if(hit.gameObject != gameObject)
            {
                if (hit.gameObject.tag == "Baby")
                {
                    Baby b = hit.GetComponent<Baby>();
                    if(b)
                    {
                        b.Die();
                    }
                }
                Rigidbody rb = (hit.GetComponentInParent<Rigidbody>());
                if(rb!= null)
                {
                    rbs.Add(rb);
                }
            }
        }
        foreach(Rigidbody rb in rbs)
        {
            rb.AddExplosionForce(explosionPower, rigid.position, explosionRadius, 0.5f, ForceMode.Impulse);
        }
        if(explosionParticles != null)
        {
            explosionParticles.Play();
        }
        rigid.isKinematic = true;
        GetComponent<MeshRenderer>().enabled = false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}
