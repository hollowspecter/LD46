using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupAbility : MonoBehaviour
{
	private const string InteractableTag = "Interactable";

	[Header("References")]
	[SerializeField]
	protected Transform cameraTransform;
	[SerializeField]
	protected Transform socket;
	[Header("Settings")]
	[SerializeField]
	protected float pickupDistance = 3f;
	[SerializeField]
	protected float minPickupDistance = 3f;
	[SerializeField]
	protected LayerMask layerMask;

	protected PlayerInputHandler inputHandler;

	protected Interactable currentInteractable = null;

	void Start()
	{
		inputHandler = GetComponent<PlayerInputHandler>();
		DebugUtility.HandleErrorIfNullGetComponent<PlayerInputHandler, PlayerCharacterController>(inputHandler, this, gameObject);
	}


	void Update()
	{
		if (inputHandler.GetFireInputDown())
		{
			if (currentInteractable?.CurrentState == Interactable.State.Hovering)
			{
				PickupItem();
			}
			else if (currentInteractable?.CurrentState == Interactable.State.Holding)
			{
				ReleaseItem();
			}
		}
	}

	private void FixedUpdate()
	{
		if (currentInteractable == null ||
			currentInteractable.CurrentState == Interactable.State.Hovering)
		{
			CheckForInteractables();
		}
	}

	private void CheckForInteractables()
	{
		// Check if you are currently looking at an interactable
		if (Physics.Raycast(cameraTransform.position, cameraTransform.forward, out var hit, pickupDistance, layerMask))
		{
			if (hit.collider.CompareTag(InteractableTag))
			{
				var interactable = hit.collider.GetComponent<Interactable>();
				if (currentInteractable == interactable)
					return;
				else if (currentInteractable != null)
					ResetCurrentInteractable();

				currentInteractable = interactable;
				currentInteractable.ChangeState(Interactable.State.Hovering);
				return;
			}
		}

		ResetCurrentInteractable();
	}

	private void ResetCurrentInteractable()
	{
		// If not, get rid of the old one
		if (currentInteractable != null)
			currentInteractable.ChangeState(Interactable.State.Default);

		currentInteractable = null;
	}

	private void PickupItem()
	{
		Debug.Log($"Picking up {currentInteractable.name}");
		currentInteractable.transform.SetParent(socket);
		currentInteractable.ChangeState(Interactable.State.Holding);
		currentInteractable.transform.position = cameraTransform.position + cameraTransform.forward * minPickupDistance;
	}

	private void ReleaseItem()
	{
		currentInteractable.transform.SetParent(null);
		ResetCurrentInteractable();
	}
}
