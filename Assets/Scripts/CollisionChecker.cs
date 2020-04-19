using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CollisionChecker : MonoBehaviour
{
	private int collidingCount = 0;

	public bool isColliding = false; // for debug

	[HideInInspector]
	public UnityEvent onCollidingEnter;
	[HideInInspector]
	public UnityEvent onCollidingExit;

	public bool IsColliding => collidingCount > 0;

	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Danger") ||
			other.CompareTag("Player") ||
			other.gameObject.isStatic)
		{
			if (!IsColliding)
				onCollidingEnter?.Invoke();

			collidingCount++;

			isColliding = IsColliding; // for debug
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.CompareTag("Danger") ||
			other.CompareTag("Player") ||
			other.gameObject.isStatic)
		{
			collidingCount--;

			if (!IsColliding)
				onCollidingExit?.Invoke();

			isColliding = IsColliding; // for debug
		}
	}
}
