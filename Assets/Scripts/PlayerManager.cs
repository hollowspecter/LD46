using UnityEngine;

public class PlayerManager : MonoBehaviour
{
	public static PlayerManager instance;

	private void Awake()
	{
		if (instance != null)
		{
			Destroy(gameObject);
		}
		else
		{
			instance = this;
			//DontDestroyOnLoad(gameObject);
		}
	}
}
