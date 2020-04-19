using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class CameraFOVUpdate : MonoBehaviour
{
	private new Camera camera;
	private Camera mainCamera;

	// Start is called before the first frame update
	void Start()
	{
		camera = GetComponent<Camera>();
		mainCamera = transform.parent.GetComponent<Camera>();
	}

	// Update is called once per frame
	void Update()
	{
		camera.fieldOfView = mainCamera.fieldOfView;
	}
}
