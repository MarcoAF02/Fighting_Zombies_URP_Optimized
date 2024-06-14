using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

// ����ű������ھ���Ĺؿ�ѡ��ť��

/// <summary>
/// �ؿ�ѡ��ť��������������Ϣ�� SceneLoader �ã�
/// </summary>
public class LevelSelectButtonController : MonoBehaviour
{
	#region ��������ͱ���

	[Header("�ؿ�ѡ��İ�ť")]
	[SerializeField] private Button levelSelectButton;
	[Header("Ŀ�곡���Ƿ����������")]
	[SerializeField] private bool isMakeComplete;
	[Header("Ŀ�곡��������")]
	[SerializeField] private string targetSceneName;
	[Header("Ŀ�곡���Ƿ�Ҫ��ҿ�����ת")]
	[SerializeField] private bool playerActiveLoad;
	[Header("�ð�ť�Ƿ���������Ա�����Ϸ�İ�ť")]
	[SerializeField] private bool isRestartButton;

	private SceneLoadData newData;

	#endregion

	#region �����������ں���

	private void Start()
	{
		levelSelectButton.onClick.AddListener(LoadSelectScene);
		if (isRestartButton) targetSceneName = SceneManager.GetActiveScene().name;
	}

	#endregion

	#region ������ת����

	public void LoadSelectScene()
	{
		if (!isMakeComplete)
		{
			Debug.LogWarning("��Ҫ���ص�Ŀ�곡��û������");
			return;
		}

		try
		{
			for (int i = 0; i < SceneLoader.Instance.sceneLoadDataList._sceneLoadDataList.Count; i++)
			{
				if (SceneLoader.Instance.sceneLoadDataList._sceneLoadDataList[i].sceneName == targetSceneName)
				{
					newData = new SceneLoadData()
					{
						sceneName = SceneLoader.Instance.sceneLoadDataList._sceneLoadDataList[i].sceneName,
						sceneDescribeText = SceneLoader.Instance.sceneLoadDataList._sceneLoadDataList[i].sceneDescribeText,
						sceneBGSprite = SceneLoader.Instance.sceneLoadDataList._sceneLoadDataList[i].sceneBGSprite,
					};
				}
			}

			MenuOperateSound.Instance.PlayCheckSound(); // ������Ч
			SceneLoader.Instance.AsyncLoadTargetScene(newData.sceneName, newData.sceneDescribeText, newData.sceneBGSprite, playerActiveLoad);
		}
		catch
		{
			Debug.LogWarning("������תʧ�ܣ�ԭ������ǣ�ȱ�� SceneLoader�����Ǵ����˵����صĳ�������û����Ƶ�����������Ǵ����˵����صĳ���������ǰ����û���ڹ����б��У���ǰ���� AssetBundle ��Դ����ʧ��");
		}
	}

	#endregion
}
