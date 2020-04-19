using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CrashHandler : MonoBehaviour
{
	private Rigidbody parentRigidbody;

	[FMODUnity.EventRef]
	[SerializeField]
	protected string crashEvent = "";

	public UnityFloatEvent onCrash;

	private void Awake()
	{
		parentRigidbody = GetComponentInParent<Rigidbody>();
	}

	private void OnTriggerEnter(Collider other)
	{
		// Calculate my momentum
		var momentum1 = parentRigidbody.mass * parentRigidbody.velocity;
		var momentum2 = other.attachedRigidbody.mass * other.attachedRigidbody.velocity;
		var diff = momentum1 - momentum2;
		var impact = diff.magnitude;
		onCrash?.Invoke(impact);
	}
}
