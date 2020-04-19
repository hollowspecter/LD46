using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Danger_AxeThrower : Danger
{
	public GameObject ammoPrefab;
	public Transform spawnPosition;
	Animator animator;

	private void Awake()
	{
		animator = GetComponent<Animator>();
	}

	protected override void StartDanger()
	{
		animator.SetTrigger("Throw");
	}

	private void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawRay(spawnPosition.position, spawnPosition.forward * 150.0f);
		Vector3 forwardFlat = spawnPosition.forward;
		forwardFlat.y = 0.0f;
		Vector3 spawnFlat = spawnPosition.position;
		spawnFlat.y = 0.0f;
		Gizmos.DrawRay(spawnFlat, forwardFlat * 150.0f);
	}

	public void OnAxeThrow()
	{
		Rigidbody rigid = GameObject.Instantiate(ammoPrefab, spawnPosition.position, spawnPosition.rotation).GetComponent<Rigidbody>();
	}
}
