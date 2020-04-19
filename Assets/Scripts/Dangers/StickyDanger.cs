using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class StickyDanger : MonoBehaviour
{
	public Rigidbody ownRigid;
	bool isSticking = false;
	public UnityEvent onStick;

	private void OnCollisionEnter(Collision collision)
	{
		if (isSticking)
			return;
		if (collision.rigidbody != null)
		{
			Stick(collision.rigidbody);
		}
	}
	public void Stick(Rigidbody r)
	{
		isSticking = true;
		ownRigid.angularVelocity = Vector3.zero;
		ownRigid.velocity = Vector3.zero;
		FixedJoint f = r.gameObject.AddComponent<FixedJoint>();
		f.connectedBody = ownRigid;
		onStick?.Invoke();
	}
}
