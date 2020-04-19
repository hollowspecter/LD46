using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Danger_Car : Danger
{
	Rigidbody rigid;
	public Vector3 centerOfMass;
	public float forwardSpeed = 5.0f;
	public float maxSpeed = 5.0f;
	GroundedChecker grounded;
	public float breakOnImpact = 100f;
	public UnityEvent onBreak;

	private CrashHandler crashHandler;

	// Start is called before the first frame update
	void Awake()
	{
		rigid = GetComponent<Rigidbody>();
		rigid.centerOfMass = centerOfMass;
		grounded = GetComponentInChildren<GroundedChecker>();
	}

	private void OnEnable()
	{
		if (crashHandler == null)
			crashHandler = GetComponentInChildren<CrashHandler>();

		if (crashHandler != null)
			crashHandler.onCrash.AddListener(OnCrash);
	}

	private void OnDisable()
	{
		if (crashHandler != null)
			crashHandler.onCrash.RemoveListener(OnCrash);
	}

	protected override void StartDanger()
	{
		rigid.velocity = forwardSpeed * transform.forward;
	}

	private void Update()
	{
		if (hasStarted && grounded.isGrounded)
		{
			rigid.AddForce(forwardSpeed * transform.forward, ForceMode.Acceleration);
			if (rigid.velocity.magnitude > maxSpeed)
			{
				rigid.velocity = Vector3.ClampMagnitude(rigid.velocity, maxSpeed);
			}
		}
	}

	private void OnDrawGizmosSelected()
	{
		if (rigid == null)
		{
			rigid = GetComponent<Rigidbody>();
			rigid.centerOfMass = centerOfMass;
		}
		Gizmos.color = Color.green;
		Gizmos.DrawWireSphere(rigid.worldCenterOfMass, 0.2f);
	}

	void OnCrash(float impact)
	{
		if (impact >= breakOnImpact)
		{
			Debug.Log($"Car breaks with impact {impact}");
			onBreak?.Invoke();
			forwardSpeed = 0f;
			GetComponent<FMODUnity.StudioEventEmitter>().Stop();
		}
	}

}
