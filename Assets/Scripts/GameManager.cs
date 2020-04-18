using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using DG.Tweening;
using UnityEngine.SceneManagement;

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
	[SerializeField]
	protected GameObject startUI;
	[SerializeField]
	protected GameObject winUI;
	[SerializeField]
	protected GameObject loseUI;

	private float originalFixedDelta;
	private Baby baby;
	private Rigidbody[] babyBodies;
	private FMOD.Studio.EventInstance bulletTimeInstance;
	private CinemachineBrain brain;

	public bool DisableInput
	{
		private set; get;
	} = false;

	public float? GameStartTime
	{
		private set; get;
	} = null;
	public float SlowedTimeDuration
	{
		get => slowedTimeDuration;
	}

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
		brain = FindObjectOfType<CinemachineBrain>();
	}

	void Start()
	{
		StartCoroutine(GameFlow());
	}

	private IEnumerator GameFlow()
	{
		// Setup level and wait for input
		Debug.Log("Starting the Scene!");
		cutsceneCamera.Priority = 100;
		DisableInput = true;
		startUI.SetActive(true);
		Time.timeScale = 0f;
		yield return new WaitUntil(() => Input.anyKeyDown);

		// Start cutscene
		Time.timeScale = 1f;
		startUI.SetActive(false);
		yield return new WaitForSeconds(timeUntilTimeStop);

		// Time is stopping now
		Debug.Log("TimeStop starts now");
		bulletTimeInstance.start();
		var timescaleTweener = DOTween.To(() => Time.timeScale, x => Time.timeScale = x, minTimeScale, timeStopDuration).SetEase(timeStopEase);
		DOTween.To(() => Time.fixedDeltaTime, x => Time.fixedDeltaTime = x, Time.fixedDeltaTime * minTimeScale, timeStopDuration).SetEase(timeStopEase);
		foreach (var body in babyBodies)
			body.isKinematic = true;
		yield return timescaleTweener.WaitForCompletion();

		// Wait for the camera blend to end
		cutsceneCamera.Priority = 0;
		yield return new WaitForSecondsRealtime(brain.m_DefaultBlend.m_Time);

		// Start Game!
		Debug.Log("Time is nearly stopped, enable Input");
		DisableInput = false;
		GameStartTime = Time.unscaledTime;
		yield return new WaitForSecondsRealtime(SlowedTimeDuration);

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
			yield return StartCoroutine(OnLose());
		else
			yield return StartCoroutine(OnWin());
	}

	private IEnumerator OnWin()
	{
		Debug.Log("You win!");
		winUI.SetActive(true);
		yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
	}

	private IEnumerator OnLose()
	{
		Debug.Log("You lose!");
		loseUI.SetActive(true);
		yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
	}
}
