using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeIndicator : MonoBehaviour
{
	void Update()
	{
		if (GameManager.Instance.GameStartTime != null)
		{
			float scale = 1f - (Time.unscaledTime - GameManager.Instance.GameStartTime.Value) / GameManager.Instance.SlowedTimeDuration;
			transform.localScale = new Vector3(Mathf.Clamp01(scale), 1f, 1f);
		}
	}
}
