using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Danger_Cannon : Danger
{
	public GameObject ammoPrefab;
	public Transform spawnPosition;
	public ParticleSystem pSystem;
	public float cannonPower = 10.0f;
	public UnityEvent onShot;

	protected override void StartDanger()
	{
		Rigidbody rigid = GameObject.Instantiate(ammoPrefab, spawnPosition.position, spawnPosition.rotation).GetComponent<Rigidbody>();
		rigid.AddForce(spawnPosition.forward * cannonPower, ForceMode.Impulse);
		if (pSystem)
			pSystem.Play();
		onShot?.Invoke();
	}

	private void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawRay(spawnPosition.position, spawnPosition.forward * 150.0f);
		Vector3 forwardFlat = spawnPosition.forward;
		forwardFlat.y = 0.0f;
		Gizmos.DrawRay(transform.position, forwardFlat * 150.0f);
	}
}
