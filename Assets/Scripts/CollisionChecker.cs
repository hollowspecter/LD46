using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CollisionChecker : MonoBehaviour
{
	private int dangerCount = 0;

	[HideInInspector]
	public UnityEvent onCollidingWithDangerEnter;
	[HideInInspector]
	public UnityEvent onCollidingWithDangerExit;

	public bool IsCollidingWithDanger => dangerCount > 0;

	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Danger"))
		{
			if (!IsCollidingWithDanger)
				onCollidingWithDangerEnter?.Invoke();

			dangerCount++;
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.CompareTag("Danger"))
		{
			dangerCount--;

			if (!IsCollidingWithDanger)
				onCollidingWithDangerExit?.Invoke();
		}
	}
}
