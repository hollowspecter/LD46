using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Radio : MonoBehaviour
{
	public float breakForce = 5f;

	public UnityEvent onBreak;

	private void OnCollisionEnter(Collision collision)
	{
		Debug.Log($"Collision! {collision.impulse.magnitude}");
		if (collision.impulse.magnitude > breakForce)
		{
			onBreak?.Invoke();
		}
	}
}
