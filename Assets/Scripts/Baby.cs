using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Baby : MonoBehaviour
{
	bool isDead = false;

	public CinemachineVirtualCamera babyCamera;
	public bool IsDead => isDead;

	// Start is called before the first frame update
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{

	}

	public void Die()
	{
		if (!isDead)
		{
			isDead = true;
			Debug.Log("Baby is dead. You lose.");
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Danger")
		{
			Debug.LogWarning(other.gameObject.name + " killed baby");
			Die();
		}

	}

	private void OnCollisionEnter(Collision collision)
	{
		if (collision.collider.tag != "Baby")
		{
			Debug.LogWarning(collision.collider.gameObject.name + " killed baby");
			Die();
		}

	}
}
