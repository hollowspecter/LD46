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
	protected Material[] opaqueMaterials;
	protected Material[] validHoloMaterials;
	protected Material[] invalidHoloMaterials;

	protected PickupIndicator pickupIndicator;

	public State CurrentState => state;
	public bool IsColliding => collisionChecker.IsColliding;

	public Material validHoloMaterial;
	public Material invalidHoloMaterial;
	public MeshRenderer meshRenderer;

	void Awake()
	{
		rigidbody = GetComponent<Rigidbody>();
		collider = GetComponent<Collider>();
		originalLayer = gameObject.layer;
		ignoreRaycastLayer = LayerMask.NameToLayer("Ignore Raycast");
		collisionChecker = GetComponentInChildren<CollisionChecker>();
		if (meshRenderer != null)
		{
			opaqueMaterials = meshRenderer.materials;
		}
		pickupIndicator = GetComponentInChildren<PickupIndicator>();
	}
	void Start()
	{
		GenerateHoloMaterialArrays();
		pickupIndicator?.disableIndicator();
	}


	private void OnEnable()
	{
		collisionChecker.onCollidingEnter.AddListener(OnCollidingEnter);
		collisionChecker.onCollidingExit.AddListener(OnCollidingExit);
	}

	private void OnDisable()
	{
		collisionChecker.onCollidingEnter.RemoveListener(OnCollidingEnter);
		collisionChecker.onCollidingExit.RemoveListener(OnCollidingExit);
	}

	public void ChangeState(State newState)
	{
		// check old state
		switch (state)
		{
			case State.Hovering:
				//Set Indicator
				pickupIndicator?.disableIndicator();
				break;

			case State.Holding:
				rigidbody.isKinematic = false;
				gameObject.layer = originalLayer;
				collider.enabled = true;

				//Set Material
				if (meshRenderer != null)
				{
					meshRenderer.materials = opaqueMaterials;
					meshRenderer.receiveShadows = true;
					meshRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
				}

				break;
		}

		state = newState;

		// check new state
		switch (newState)
		{
			case State.Hovering:
				//Set Indicator
				pickupIndicator?.enableIndicator();
				break;

			case State.Holding:
				rigidbody.isKinematic = true;
				gameObject.layer = ignoreRaycastLayer;
				collider.enabled = false;

				//set Material
				if (meshRenderer != null)
				{
					meshRenderer.materials = validHoloMaterials;
					meshRenderer.receiveShadows = false;
					meshRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
				}

				break;
		}
	}
	private void GenerateHoloMaterialArrays()
	{
		validHoloMaterials = meshRenderer.materials;
		invalidHoloMaterials = meshRenderer.materials;
		for (int i = 0; i < meshRenderer.materials.Length; i++)
		{
			validHoloMaterials[i] = validHoloMaterial;
			invalidHoloMaterials[i] = invalidHoloMaterial;
			// print("materials of: " + name + i + ": " + meshRenderer.materials[i]);
		}

	}

	public void OnCollidingEnter()
	{
		if (CurrentState == State.Holding)
		{
			//change color to negative
			if (meshRenderer == null)
				return;
			meshRenderer.materials = invalidHoloMaterials;
		}
	}

	public void OnCollidingExit()
	{
		if (CurrentState == State.Holding)
		{
			//change color to positive
			if (meshRenderer == null)
				return;
			meshRenderer.materials = validHoloMaterials;
		}
	}
}
