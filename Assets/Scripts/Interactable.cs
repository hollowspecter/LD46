using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
public class Interactable : MonoBehaviour
{
	public enum State
	{
		Default, // just laying aorund in the world
		Hovering, // player is hovering with the cursor over it
		Holding // the player holds the object
	}

	protected State state = State.Default;
	protected new Rigidbody rigidbody;
	protected new Collider collider;
	protected int originalLayer;
	protected int ignoreRaycastLayer;
	protected CollisionChecker collisionChecker;

	public State CurrentState => state;
	public bool IsCollidingWithDanger => collisionChecker.IsCollidingWithDanger;

	void Awake()
	{
		rigidbody = GetComponent<Rigidbody>();
		collider = GetComponent<Collider>();
		originalLayer = gameObject.layer;
		ignoreRaycastLayer = LayerMask.NameToLayer("Ignore Raycast");
		collisionChecker = GetComponentInChildren<CollisionChecker>();
	}

	private void OnEnable()
	{
		collisionChecker.onCollidingWithDangerEnter.AddListener(OnCollidingWithDangerEnter);
		collisionChecker.onCollidingWithDangerExit.AddListener(OnCollidingWithDangerExit);
	}

	private void OnDisable()
	{
		collisionChecker.onCollidingWithDangerEnter.RemoveListener(OnCollidingWithDangerEnter);
		collisionChecker.onCollidingWithDangerExit.RemoveListener(OnCollidingWithDangerExit);
	}

	public void ChangeState(State newState)
	{
		// check old state
		switch (state)
		{
			case State.Holding:
				rigidbody.isKinematic = false;
				gameObject.layer = originalLayer;
				collider.enabled = true;
				break;
		}

		state = newState;

		// check new state
		switch (newState)
		{
			case State.Holding:
				rigidbody.isKinematic = true;
				gameObject.layer = ignoreRaycastLayer;
				collider.enabled = false;
				break;
		}
	}

	public void OnCollidingWithDangerEnter()
	{
		if (CurrentState == State.Holding)
		{
			// TODO mathew: change color to negative
		}
	}

	public void OnCollidingWithDangerExit()
	{
		if (CurrentState == State.Holding)
		{
			// TODO mathew: change color to positive
		}
	}
}
