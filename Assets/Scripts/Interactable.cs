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

	public State CurrentState => state;

	void Awake()
	{
		rigidbody = GetComponent<Rigidbody>();
		collider = GetComponent<Collider>();
	}

	public void ChangeState(State newState)
	{
		// check old state
		switch (state)
		{
			case State.Holding:
				rigidbody.isKinematic = false;
				//collider.enabled = true;
				break;
		}

		state = newState;

		// check new state
		switch (newState)
		{
			case State.Holding:
				rigidbody.isKinematic = true;
				//collider.enabled = false;
				break;
		}
	}
}
