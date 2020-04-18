using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CollisionChecker : MonoBehaviour
{
	private int dangerCount = 0;

	public bool isCollidingWithDanger = false; // for debug

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

			isCollidingWithDanger = IsCollidingWithDanger; // for debug
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.CompareTag("Danger"))
		{
			dangerCount--;

			if (!IsCollidingWithDanger)
				onCollidingWithDangerExit?.Invoke();

			isCollidingWithDanger = IsCollidingWithDanger; // for debug
		}
	}
}
