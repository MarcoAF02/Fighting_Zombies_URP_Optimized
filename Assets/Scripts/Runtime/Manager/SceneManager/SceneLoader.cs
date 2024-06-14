using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// 场景加载器，根据传入的参数异步加载指定场景
/// </summary>
public class SceneLoader : MonoSingleton<SceneLoader>
{
	#region 基本组件和变量

	private FPS_Play_Action fpsPlayAction;

	[Header("场景加载数据类")]
	public SceneLoadDataList sceneLoadDataList;
	[Header("音频播放组件")]
	[SerializeField] private SceneLoadSound sceneLoadSound;

	[Header("加载场景用的背景图片")]
	[SerializeField] private Image targetSceneBG;
	[SerializeField] private Slider progressSlider; //进度条

	[Header("提示文字")]
	[SerializeField] private GameObject loadTipText;
	[SerializeField] private GameObject checkTipText;
	[Header("关卡描述文本")]
	[SerializeField] private TextMeshProUGUI sceneDescribeText;

	// 协程
	private Coroutine asyncLoadTargetScene_IECor;
	private Coroutine enterLoaderScene_IECor;

	#endregion

	#region 基本生命周期函数

	protected override void Awake()
	{
		base.Awake();
		DontDestroyOnLoad(gameObject);

		fpsPlayAction = new FPS_Play_Action();
		fpsPlayAction?.Enable();
	}

	protected override void OnDestroy()
	{
		base.OnDestroy();
		fpsPlayAction?.Disable();
	}

	#endregion

	#region 场景加载函数

	/// <summary>
	/// 异步加载目标场景，_playerActive 表示玩家是否要主动跳转场景
	/// </summary>
	/// <param name="_targetSceneName"></param>
	/// <param name="_targetSceneBG"></param>
	/// <param name="_playerActive"></param>
	public void AsyncLoadTargetScene(string _targetSceneName, string _targetSceneDescribe, Sprite _targetSceneBG, bool _playerActive)
	{
		// 加载场景前，先清空注册的摄像机（防止空引用）
		SettingsLoader.Instance.cameraSettingsLoaderList.Clear();

		sceneLoadSound.PlaySceneLoadSound();
		enterLoaderScene_IECor = StartCoroutine(AsyncEnterLoaderScene_IE(_targetSceneName, _targetSceneDescribe, _targetSceneBG, _playerActive));
	}

	// 异步加载目标场景
	private IEnumerator AsyncLoadTargetScene_IE(string _targetSceneName, string _targetSceneDescribe, Sprite _targetSceneBG, bool _playerActive)
	{
		targetSceneBG.sprite = _targetSceneBG;
		targetSceneBG.gameObject.SetActive(true);
		progressSlider.gameObject.SetActive(true);
		loadTipText.SetActive(true);

		sceneDescribeText.text = _targetSceneDescribe;

		AsyncOperation asyncOperation;

		try
		{
			asyncOperation = SceneManager.LoadSceneAsync(_targetSceneName); // 根据选好的地图异步加载场景
			asyncOperation.allowSceneActivation = false; // 加载完成后先不自动跳转
		}
		catch
		{
			Debug.LogWarning("需要加载的场景还没有制作");

			asyncOperation = null;
			yield break;
		}

		while (!asyncOperation.isDone) // 如果加载没有完成，那肯定是继续加载啊
		{
			// asyncOperation.progress * 100 可以显示为百分比
			progressSlider.value = asyncOperation.progress; // 同步 Silderbar 的值

			if (asyncOperation.progress >= 0.9f) // 如果加载到了90%
			{
				progressSlider.value = 1.0f; // 直接设置为100%，请见官方文档

				loadTipText.SetActive(false);

				if (_playerActive) // 玩家主动加载场景
				{
					checkTipText.SetActive(!asyncOperation.allowSceneActivation);

					if (fpsPlayAction.UI_Keyboard_And_Mouse.Mouse_Confirm.WasPressedThisFrame())
					{
						asyncOperation.allowSceneActivation = true; // 执行场景跳转
						checkTipText.SetActive(false);
						targetSceneBG.gameObject.SetActive(false);
						progressSlider.gameObject.SetActive(false);
						sceneDescribeText.text = string.Empty;
					}
				}
				else // 自动加载场景
				{
					asyncOperation.allowSceneActivation = true; // 执行场景跳转
					checkTipText.SetActive(false);
					targetSceneBG.gameObject.SetActive(false);
					progressSlider.gameObject.SetActive(false);
					sceneDescribeText.text = string.Empty;
				}
			}

			// Debug.Log(_playerActive);
			yield return null;
		}
	}

	// 先异步加载进入防止穿帮的临时场景
	private IEnumerator AsyncEnterLoaderScene_IE(string _targetSceneName, string _targetSceneDescribe, Sprite _targetSceneBG, bool _playerActive)
	{
		AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneLoadDataList._sceneLoaderName);

		while (!asyncOperation.isDone)
		{
			if (asyncOperation.progress >= 0.9f) // 如果加载到了90%
			{
				asyncOperation.allowSceneActivation = true;

				if (asyncOperation.allowSceneActivation)
				{
					asyncLoadTargetScene_IECor = StartCoroutine(AsyncLoadTargetScene_IE(_targetSceneName, _targetSceneDescribe, _targetSceneBG, _playerActive));
				}
			}

			yield return null;
		}
	}

	#endregion

}
