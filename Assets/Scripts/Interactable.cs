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
	public bool IsCollidingWithDanger => collisionChecker.IsCollidingWithDanger;

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
			{opaqueMaterials = meshRenderer.materials;}
		pickupIndicator = GetComponentInChildren<PickupIndicator>();
	}
	void Start()
	{
		GenerateHoloMaterialArrays();
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
					meshRenderer.materials = opaqueMaterials;
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
				if (meshRenderer != null)
				{
					meshRenderer.materials = validHoloMaterials;
					meshRenderer.receiveShadows = false;
					meshRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
				}

				//Set Indicator
				pickupIndicator?.disableIndicator();

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
			print("materials of: " + name + i + ": " + meshRenderer.materials[i]);
		}

	} 

	public void OnCollidingWithDangerEnter()
	{
		if (CurrentState == State.Holding)
		{
			//change color to negative
			if (meshRenderer == null) return;
			meshRenderer.materials = validHoloMaterials;

		}
	}

	public void OnCollidingWithDangerExit()
	{
		if (CurrentState == State.Holding)
		{
			//change color to positive
			if (meshRenderer == null) return;
 			meshRenderer.materials = invalidHoloMaterials;
		}
	}
}
