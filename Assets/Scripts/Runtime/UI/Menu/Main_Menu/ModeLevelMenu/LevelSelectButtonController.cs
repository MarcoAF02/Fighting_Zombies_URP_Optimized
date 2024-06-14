using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

// 这个脚本挂载在具体的关卡选择按钮上

/// <summary>
/// 关卡选择按钮控制器（传递信息给 SceneLoader 用）
/// </summary>
public class LevelSelectButtonController : MonoBehaviour
{
	#region 基本组件和变量

	[Header("关卡选择的按钮")]
	[SerializeField] private Button levelSelectButton;
	[Header("目标场景是否制作完成了")]
	[SerializeField] private bool isMakeComplete;
	[Header("目标场景的名称")]
	[SerializeField] private string targetSceneName;
	[Header("目标场景是否要玩家控制跳转")]
	[SerializeField] private bool playerActiveLoad;
	[Header("该按钮是否是玩家重试本局游戏的按钮")]
	[SerializeField] private bool isRestartButton;

	private SceneLoadData newData;

	#endregion

	#region 基本生命周期函数

	private void Start()
	{
		levelSelectButton.onClick.AddListener(LoadSelectScene);
		if (isRestartButton) targetSceneName = SceneManager.GetActiveScene().name;
	}

	#endregion

	#region 场景跳转功能

	public void LoadSelectScene()
	{
		if (!isMakeComplete)
		{
			Debug.LogWarning("需要加载的目标场景没有做完");
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

			MenuOperateSound.Instance.PlayCheckSound(); // 播放音效
			SceneLoader.Instance.AsyncLoadTargetScene(newData.sceneName, newData.sceneDescribeText, newData.sceneBGSprite, playerActiveLoad);
		}
		catch
		{
			Debug.LogWarning("场景跳转失败，原因可能是：缺少 SceneLoader（不是从主菜单加载的场景）；没有音频管理器（不是从主菜单加载的场景）；当前场景没有在构建列表中；当前场景 AssetBundle 资源加载失败");
		}
	}

	#endregion
}
