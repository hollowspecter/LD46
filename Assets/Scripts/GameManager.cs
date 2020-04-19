using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using DG.Tweening;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering;
using UnityEngine.UI;
using UnityEditor;
using TMPro;

public class GameManager : MonoBehaviour
{
	public static GameManager Instance = null;
	private const string CurrentLevelKey = "CurrentLevel";

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
	[Tooltip("Additively loads the lighting scene and sets it as active scene.")]
	[SerializeField]
	protected string lightingScene = "";
	[SerializeField]
	protected Color fadingColor = Color.white;
	[SerializeField]
	protected bool makeBabyKinematicWhileTimeStop = true;
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
	[SerializeField]
	protected Volume bulletTimeVolume;
	[SerializeField]
	protected Image fader;
	[SerializeField]
	protected GameObject levelButtonPrefab;
	[SerializeField]
	protected Transform levelButtonParent;

	private float originalFixedDelta;
	private Baby baby;
	private Rigidbody[] babyBodies;
	private FMOD.Studio.EventInstance bulletTimeInstance;
	private CinemachineBrain brain;
	private Interactable[] interactables;
	private LayerMask dangerLayer;
	private Coroutine gameFlowCoroutine;

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
		interactables = FindObjectsOfType<Interactable>();
		dangerLayer = LayerMask.NameToLayer("Danger");

		// Load Player Prefs
		if (!PlayerPrefs.HasKey(CurrentLevelKey))
			PlayerPrefs.SetInt(CurrentLevelKey, 0);

		int currentLevel = Mathf.Max(SceneManager.GetActiveScene().buildIndex, PlayerPrefs.GetInt(CurrentLevelKey));
		PlayerPrefs.SetInt(CurrentLevelKey, currentLevel);

		// Setup the Buttons
		var levelCount = SceneManager.sceneCountInBuildSettings - 1;
		for (int i = 0; i < levelCount; ++i)
		{
			var levelButton = Instantiate(levelButtonPrefab, levelButtonParent);
			levelButton.GetComponentInChildren<TextMeshProUGUI>().text = (i + 1) + "";
			levelButton.GetComponent<Button>().interactable = i <= currentLevel;
			levelButton.GetComponent<Button>().onClick.AddListener(() =>
			{
				LoadLevel(levelButton);
			});
		}
	}

	void Start()
	{
		/*
		 * Setup level
		 */
		Debug.Log("Starting the Scene!");
		cutsceneCamera.Priority = 100;
		DisableInput = true;
		startUI.SetActive(true);
		Time.timeScale = 0f;
		bulletTimeVolume.weight = 0f;
		fader.color = fadingColor;
		baby.babyCamera.Priority = 0;
		Cursor.lockState = CursorLockMode.None;
		Cursor.visible = true;
		gameFlowCoroutine = StartCoroutine(GameFlow());
	}

	private IEnumerator GameFlow()
	{
		/*
		 * Check for Lighting scene and additively load it
		 */
		Debug.Log("Load lighting scene");
		yield return SceneManager.LoadSceneAsync(lightingScene, LoadSceneMode.Additive);
		SceneManager.SetActiveScene(SceneManager.GetSceneByName(lightingScene));

		/*
		 * Fade In
		 */
		Debug.Log("Fade out!");
		var faderOut = fader.DOFade(0f, 1f).SetUpdate(UpdateType.Normal, true);
		yield return faderOut.WaitForCompletion();

		/*
		 * Wait for Input to Start
		 */
		Debug.Log("Waiting for Input");
		yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));
		// Hide cursor
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;

		/*
		 * Start cutscene
		 */
		Debug.Log("Start Cutscene");
		Time.timeScale = 1f;
		startUI.SetActive(false);
		yield return new WaitForSeconds(timeUntilTimeStop);

		/*
		 * Time is stopping now
		 */
		Debug.Log("TimeStop starts now");
		bulletTimeInstance.start();
		var timescaleTweener = DOTween.To(() => Time.timeScale, x => Time.timeScale = x, minTimeScale, timeStopDuration).SetEase(timeStopEase).SetUpdate(UpdateType.Normal, true);
		DOTween.To(() => Time.fixedDeltaTime, x => Time.fixedDeltaTime = x, Time.fixedDeltaTime * minTimeScale, timeStopDuration).SetEase(timeStopEase).SetUpdate(UpdateType.Normal, true);
		DOTween.To(() => bulletTimeVolume.weight, x => bulletTimeVolume.weight = x, 1f, timeStopDuration).SetEase(timeStopEase).SetUpdate(UpdateType.Normal, true);
		if (makeBabyKinematicWhileTimeStop)
		{
			foreach (var body in babyBodies)
				body.isKinematic = true;
		}
		yield return timescaleTweener.WaitForCompletion();

		/*
		 * Wait for the camera blend to end
		 */
		cutsceneCamera.Priority = 0;
		yield return new WaitForSecondsRealtime(brain.m_DefaultBlend.m_Time);

		/*
		 * Start Game!
		 */
		Debug.Log("Time is nearly stopped, enable Input");
		DisableInput = false;
		GameStartTime = Time.unscaledTime;
		yield return new WaitForSecondsRealtime(SlowedTimeDuration);

		/*
		 * Turn on Baby Camera
		 */
		baby.babyCamera.Priority = 200;
		yield return new WaitForSecondsRealtime(brain.m_DefaultBlend.m_Time);

		/*
		 * Reset the time scale
		 */
		Debug.Log("Time's up! Let's play");
		bulletTimeInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
		timescaleTweener = DOTween.To(() => Time.timeScale, x => Time.timeScale = x, 1f, timeStopDuration).SetEase(timeStopEase);
		DOTween.To(() => Time.fixedDeltaTime, x => Time.fixedDeltaTime = x, originalFixedDelta, timeStopDuration).SetEase(timeStopEase);
		DOTween.To(() => bulletTimeVolume.weight, x => bulletTimeVolume.weight = x, 0f, timeStopDuration).SetEase(timeStopEase);
		// make all interactables to dangers so they can also kill the baby
		foreach (var interactable in interactables)
		{
			interactable.gameObject.layer = dangerLayer;
			interactable.gameObject.tag = "Danger";
		}
		// make baby able to fly away
		if (makeBabyKinematicWhileTimeStop)
		{
			foreach (var body in babyBodies)
				body.isKinematic = false;
		}

		yield return timescaleTweener.WaitForCompletion();

		yield return new WaitForSeconds(checkWinConditionAfterDuration);

		DisableInput = true;
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
		yield return StartCoroutine(ELoadLevel(SceneManager.GetSceneAt(0).buildIndex + 1));
#if UNITY_EDITOR
		EditorApplication.ExitPlaymode();
#endif
	}

	private IEnumerator OnLose()
	{
		Debug.Log("You lose!");
		loseUI.SetActive(true);
		yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));
		yield return StartCoroutine(ELoadLevel(SceneManager.GetSceneAt(0).buildIndex));
#if UNITY_EDITOR
		EditorApplication.ExitPlaymode();
#endif
	}

	private void LoadLevel(GameObject buttonObject)
	{
		int level = int.Parse(buttonObject.GetComponentInChildren<TextMeshProUGUI>().text);
		if (level - 1 == SceneManager.GetSceneAt(0).buildIndex)
			return;
		StopCoroutine(gameFlowCoroutine);
		StartCoroutine(ELoadLevel(level - 1));
	}

	private IEnumerator ELoadLevel(int buildIndex)
	{
		Debug.Log($"Switching Levels! Level {buildIndex}");
		var fadeIn = fader.DOFade(1f, 1f).SetUpdate(UpdateType.Normal, true);
		yield return fadeIn.WaitForCompletion();
		SceneManager.LoadScene(buildIndex);
	}

	[ContextMenu("Reset PlayerPrefs")]
	private void ResetPlayerPrefs()
	{
		PlayerPrefs.DeleteAll();
	}
}
