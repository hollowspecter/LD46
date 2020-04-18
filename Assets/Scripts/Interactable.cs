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
	protected Material opaqueMaterial;
	protected MeshRenderer meshRenderer;
	protected PickupIndicator pickupIndicator;

	public State CurrentState => state;
	public bool IsCollidingWithDanger => collisionChecker.IsCollidingWithDanger;

	public Material validHoloMaterial;
	public Material invalidHoloMaterial;

	void Awake()
	{
		rigidbody = GetComponent<Rigidbody>();
		collider = GetComponent<Collider>();
		originalLayer = gameObject.layer;
		ignoreRaycastLayer = LayerMask.NameToLayer("Ignore Raycast");
		collisionChecker = GetComponentInChildren<CollisionChecker>();
		meshRenderer = GetComponent<MeshRenderer>();
		if (meshRenderer != null)
			opaqueMaterial = meshRenderer.material;
		pickupIndicator = GetComponentInChildren<PickupIndicator>();
	}


	private void OnEnable()
	{
		collisionChecker.onCollidingWithDangerEnter.AddListener(OnCollidingWithDangerEnter);
		collisionChecker.onCollidingWithDangerExit.AddListener(OnCollidingWithDangerExit);
		pickupIndicator?.enableIndicator();
	}

	private void OnDisable()
	{
		collisionChecker.onCollidingWithDangerEnter.RemoveListener(OnCollidingWithDangerEnter);
		collisionChecker.onCollidingWithDangerExit.RemoveListener(OnCollidingWithDangerExit);
		pickupIndicator?.disableIndicator();
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

				//Set Material
				if (meshRenderer != null)
				{
					meshRenderer.material = opaqueMaterial;
					meshRenderer.receiveShadows = true;
					meshRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
				}

				//Set Indicator
				pickupIndicator?.enableIndicator();

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

				//set Material
				if (validHoloMaterial == null)
				{
					print("Missing Holomaterial: " + this.name);
					break;
				}
				if (meshRenderer != null)
				{
					meshRenderer.material = validHoloMaterial;
					meshRenderer.receiveShadows = false;
					meshRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
				}

				//Set Indicator
				pickupIndicator?.disableIndicator();

				break;
		}
	}

	public void OnCollidingWithDangerEnter()
	{
		if (CurrentState == State.Holding)
		{
			//change color to negative
			if (meshRenderer)
				meshRenderer.material = invalidHoloMaterial;

		}
	}

	public void OnCollidingWithDangerExit()
	{
		if (CurrentState == State.Holding)
		{
			//change color to positive
			if (meshRenderer)
				meshRenderer.material = validHoloMaterial;
		}
	}
}
