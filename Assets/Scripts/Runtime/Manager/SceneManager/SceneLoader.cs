using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// ���������������ݴ���Ĳ����첽����ָ������
/// </summary>
public class SceneLoader : MonoSingleton<SceneLoader>
{
	#region ��������ͱ���

	private FPS_Play_Action fpsPlayAction;

	[Header("��������������")]
	public SceneLoadDataList sceneLoadDataList;
	[Header("��Ƶ�������")]
	[SerializeField] private SceneLoadSound sceneLoadSound;

	[Header("���س����õı���ͼƬ")]
	[SerializeField] private Image targetSceneBG;
	[SerializeField] private Slider progressSlider; //������

	[Header("��ʾ����")]
	[SerializeField] private GameObject loadTipText;
	[SerializeField] private GameObject checkTipText;
	[Header("�ؿ������ı�")]
	[SerializeField] private TextMeshProUGUI sceneDescribeText;

	// Э��
	private Coroutine asyncLoadTargetScene_IECor;
	private Coroutine enterLoaderScene_IECor;

	#endregion

	#region �����������ں���

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

	#region �������غ���

	/// <summary>
	/// �첽����Ŀ�곡����_playerActive ��ʾ����Ƿ�Ҫ������ת����
	/// </summary>
	/// <param name="_targetSceneName"></param>
	/// <param name="_targetSceneBG"></param>
	/// <param name="_playerActive"></param>
	public void AsyncLoadTargetScene(string _targetSceneName, string _targetSceneDescribe, Sprite _targetSceneBG, bool _playerActive)
	{
		// ���س���ǰ�������ע������������ֹ�����ã�
		SettingsLoader.Instance.cameraSettingsLoaderList.Clear();

		sceneLoadSound.PlaySceneLoadSound();
		enterLoaderScene_IECor = StartCoroutine(AsyncEnterLoaderScene_IE(_targetSceneName, _targetSceneDescribe, _targetSceneBG, _playerActive));
	}

	// �첽����Ŀ�곡��
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
			asyncOperation = SceneManager.LoadSceneAsync(_targetSceneName); // ����ѡ�õĵ�ͼ�첽���س���
			asyncOperation.allowSceneActivation = false; // ������ɺ��Ȳ��Զ���ת
		}
		catch
		{
			Debug.LogWarning("��Ҫ���صĳ�����û������");

			asyncOperation = null;
			yield break;
		}

		while (!asyncOperation.isDone) // �������û����ɣ��ǿ϶��Ǽ������ذ�
		{
			// asyncOperation.progress * 100 ������ʾΪ�ٷֱ�
			progressSlider.value = asyncOperation.progress; // ͬ�� Silderbar ��ֵ

			if (asyncOperation.progress >= 0.9f) // ������ص���90%
			{
				progressSlider.value = 1.0f; // ֱ������Ϊ100%������ٷ��ĵ�

				loadTipText.SetActive(false);

				if (_playerActive) // ����������س���
				{
					checkTipText.SetActive(!asyncOperation.allowSceneActivation);

					if (fpsPlayAction.UI_Keyboard_And_Mouse.Mouse_Confirm.WasPressedThisFrame())
					{
						asyncOperation.allowSceneActivation = true; // ִ�г�����ת
						checkTipText.SetActive(false);
						targetSceneBG.gameObject.SetActive(false);
						progressSlider.gameObject.SetActive(false);
						sceneDescribeText.text = string.Empty;
					}
				}
				else // �Զ����س���
				{
					asyncOperation.allowSceneActivation = true; // ִ�г�����ת
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

	// ���첽���ؽ����ֹ�������ʱ����
	private IEnumerator AsyncEnterLoaderScene_IE(string _targetSceneName, string _targetSceneDescribe, Sprite _targetSceneBG, bool _playerActive)
	{
		AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneLoadDataList._sceneLoaderName);

		while (!asyncOperation.isDone)
		{
			if (asyncOperation.progress >= 0.9f) // ������ص���90%
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
