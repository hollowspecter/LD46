using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using DG.Tweening;

public class GameManager : MonoBehaviour
{
	public static GameManager Instance = null;

	[Header("Settings")]
	[SerializeField]
	protected float timeUntilTimeStop = 1f;
	[SerializeField]
	protected float timeStopDuration = 1f;
	[SerializeField]
	protected Ease timeStopEase = Ease.OutCubic;
	[SerializeField]
	protected float minTimeScale = 0.05f;
	[SerializeField]
	protected float slowedTimeDuration = 10f;
	[SerializeField]
	protected float checkWinConditionAfterDuration = 5f;
	[Header("References")]
	[SerializeField]
	protected CinemachineVirtualCamera cutsceneCamera;
	[SerializeField]
	[FMODUnity.EventRef]
	protected string bulletTimeSnapshot = "";

	private float originalFixedDelta;
	private Baby baby;
	private Rigidbody[] babyBodies;
	private FMOD.Studio.EventInstance bulletTimeInstance;

	public bool DisableInput
	{
		private set; get;
	} = false;

	void Awake()
	{
		if (Instance == null)
			Instance = this;
		else
			Destroy(this.gameObject);

		originalFixedDelta = Time.fixedDeltaTime;
		baby = FindObjectOfType<Baby>();
		babyBodies = baby.GetComponentsInChildren<Rigidbody>();
		bulletTimeInstance = FMODUnity.RuntimeManager.CreateInstance(bulletTimeSnapshot);
	}

	void Start()
	{
		StartCoroutine(GameFlow());
	}

	private IEnumerator GameFlow()
	{
		// Starting cutscene
		Debug.Log("Starting the Scene!");
		cutsceneCamera.Priority = 100;
		DisableInput = true;
		yield return new WaitForSeconds(timeUntilTimeStop);

		// Time is stopping now
		Debug.Log("TimeStop starts now");
		bulletTimeInstance.start();
		var timescaleTweener = DOTween.To(() => Time.timeScale, x => Time.timeScale = x, minTimeScale, timeStopDuration).SetEase(timeStopEase);
		DOTween.To(() => Time.fixedDeltaTime, x => Time.fixedDeltaTime = x, Time.fixedDeltaTime * minTimeScale, timeStopDuration).SetEase(timeStopEase);
		foreach (var body in babyBodies)
			body.isKinematic = true;
		yield return timescaleTweener.WaitForCompletion();

		// Start Game!
		Debug.Log("Time is nearly stopped, enable Input");
		DisableInput = false;
		cutsceneCamera.Priority = 0;
		yield return new WaitForSecondsRealtime(slowedTimeDuration);

		// Reset the time scale
		Debug.Log("Time's up! Let's play");
		bulletTimeInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
		timescaleTweener = DOTween.To(() => Time.timeScale, x => Time.timeScale = x, 1f, timeStopDuration).SetEase(timeStopEase);
		DOTween.To(() => Time.fixedDeltaTime, x => Time.fixedDeltaTime = x, originalFixedDelta, timeStopDuration).SetEase(timeStopEase);
		foreach (var body in babyBodies)
			body.isKinematic = false;
		yield return timescaleTweener.WaitForCompletion();

		yield return new WaitForSeconds(checkWinConditionAfterDuration);

		if (baby.IsDead)
			OnLose();
		else
			OnWin();
	}

	private void OnWin()
	{
		Debug.Log("You win!");
	}

	private void OnLose()
	{
		Debug.Log("You lose!");
	}
}
