#if UNITY_2_6 || UNITY_2_6_1 || UNITY_3_0 || UNITY_3_0_0 || UNITY_3_1 || UNITY_3_2 || UNITY_3_3 || UNITY_3_4 || UNITY_3_5 || UNITY_4_0 || UNITY_4_0_1 || UNITY_4_1 || UNITY_4_2 || UNITY_4_3 || UNITY_4_5 || UNITY_4_6 || UNITY_5_0 || UNITY_5_1 || UNITY_5_2
#define UNITY_BEFORE_SCENEMANAGER
#endif

using UnityEngine;
using UnityEngine.Events;
using System.Collections;
#if !UNITY_BEFORE_SCENEMANAGER
using UnityEngine.SceneManagement;
#endif

/// <summary>
/// Level Loader Singleton
/// 
/// Should be the only place that Scene loading/ level loading happens through
///	This will prevent multiple loads at the same time, and reimplementation of the same loading scripts
/// </summary>
[UnitySingleton(UnitySingletonAttribute.Type.CreateOnNewGameObject, false)]
public class LevelLoader : UnitySingleton<LevelLoader>
{
	AsyncOperation levelLoadOperation = null;

	[AdvancedInspector.Inspect]
	public bool IsLoading { get { return levelLoadOperation != null && !levelLoadOperation.isDone; } }

	[AdvancedInspector.Inspect]
	public float LoadProgress { get { return levelLoadOperation == null ? 0f : levelLoadOperation.progress; } }

	[AdvancedInspector.Inspect]
	public string CurrentSceneName
	{
		get
		{
#if UNITY_BEFORE_SCENEMANAGER
			return Application.loadedLevelName;
#else
			return SceneManager.GetActiveScene().name;
#endif
		}
	}

	[AdvancedInspector.Inspect]
	public int CurrentSceneId
	{
		get
		{
#if UNITY_BEFORE_SCENEMANAGER
			return Application.loadedLevel;
#else
			return SceneManager.GetActiveScene().buildIndex;
#endif
		}
	}
	
	[AdvancedInspector.Inspect]
	public int SceneCount
	{
		get
		{
#if UNITY_BEFORE_SCENEMANAGER
			return Application.levelCount;
#else
			return SceneManager.sceneCount;
#endif
		}
	}

	[SerializeField]
	private UnityEvent onLoadStarted;
	public UnityEvent OnLoadStarted { get { return onLoadStarted; } }

	[SerializeField]
	private UnityEvent onLoadFinished;
	public UnityEvent OnLoadFinished { get { return onLoadFinished; } }

	public void LoadLevelForce(string levelName)
	{
		if (!IsLoading)
		{
#if UNITY_BEFORE_SCENEMANAGER
			Application.LoadLevel(levelName);
#else
			SceneManager.LoadScene(levelName);
#endif
		}
	}

	public void LoadLevelForce(int levelId)
	{
		if (!IsLoading)
		{
#if UNITY_BEFORE_SCENEMANAGER
			Application.LoadLevel(levelId);
#else
			SceneManager.LoadScene(levelId);
#endif
		}
	}

	public void LoadLevelAsync(string levelName, bool manuallyEnterLevel = false)
	{
		if (!IsLoading)
		{
#if UNITY_BEFORE_SCENEMANAGER
			levelLoadOperation = Application.LoadLevelAsync(levelName);
#else
			levelLoadOperation = SceneManager.LoadSceneAsync(levelName);
#endif
			StartCoroutine(DoLevelLoadOperation(manuallyEnterLevel));
		}
	}

	public void LoadLevelAsync(int levelId, bool manuallyEnterLevel = false)
	{
		if (!IsLoading)
		{
#if UNITY_BEFORE_SCENEMANAGER
			levelLoadOperation = Application.LoadLevelAsync(levelId);
#else
			levelLoadOperation = SceneManager.LoadSceneAsync(levelId);
#endif
			StartCoroutine(DoLevelLoadOperation(manuallyEnterLevel));
		}
	}

	public void ManualFinishLoadLevel()
	{
		if (IsLoading)
		{
			levelLoadOperation.allowSceneActivation = true;
		}
	}

	IEnumerator DoLevelLoadOperation(bool manuallyEnterLevel)
	{
		OnLoadStarted.Invoke();
		if (manuallyEnterLevel)
			levelLoadOperation.allowSceneActivation = false;

		while (
			(!manuallyEnterLevel && !levelLoadOperation.isDone) ||
			(manuallyEnterLevel && Mathf.Approximately(levelLoadOperation.progress, 0.9f))
			)
		{
			yield return null;
		}

		if (!manuallyEnterLevel)
			levelLoadOperation = null;

		OnLoadFinished.Invoke();
    }

	void OnLevelWasLoaded(int levelId)
	{
		levelLoadOperation = null;
    }
}
