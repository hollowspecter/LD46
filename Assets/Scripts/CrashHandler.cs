using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CrashHandler : MonoBehaviour
{
	private const string ImpactParameter = "Impact";
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
		if (other.isTrigger)
			return;

		// Calculate my momentum
		var momentum1 = parentRigidbody.mass * parentRigidbody.velocity;
		Vector3 momentum2 = Vector3.zero;
		if (other.attachedRigidbody != null)
		{
			momentum2 = other.attachedRigidbody.mass * other.attachedRigidbody.velocity;
		}
		var diff = momentum1 - momentum2;
		var impact = diff.magnitude;
		onCrash?.Invoke(impact);

		// Play Sound
		Debug.Log($"CrashSound! {impact}");
		FMOD.Studio.EventInstance crash = FMODUnity.RuntimeManager.CreateInstance(crashEvent);
		crash.setParameterByName(ImpactParameter, impact);
		crash.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameObject));
		crash.start();
		crash.release();
	}
}
