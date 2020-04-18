using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Baby : MonoBehaviour
{
	bool isDead = false;

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
			Die();
		}

	}
}
