using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 玩家设置读取功能
/// </summary>
public class PlayerSettingsLoader : MonoBehaviour
{
	#region 基本组件和变量

	[Header("玩家控制器")]
	[SerializeField] private PlayerController playerController;
	[Header("教程信息触发器")]
	[SerializeField] private TutorialTipTrigger tutorialTipTrigger;
	[Header("玩家摄像机动画控制器")]
	[SerializeField] private PlayerCameraAnim playerCameraAnim;

	// TODO: 未来的更新：准星有 点状，静态十字，动态十字
	[Header("玩家准星（在 UI 上）")]
	[SerializeField] private GameObject frontSight;

	// TODO: 未来的更新：增加血条
	[Header("玩家生命值显示 UI")]
	[SerializeField] private HealthBGController healthBGController;

	// 协程
	private Coroutine applyAllGraphicsSettingsWhenStart_IECor;

	#endregion

	#region 基本生命周期函数

	private void Start()
	{
		LoadGameSettings();
	}

	#endregion

	#region 读取游戏设置

	// 读取游戏设置，这些设置不是全局的，只和玩家自己有关
	public void LoadGameSettings()
	{
		if (SettingsLoader.Instance == null)
		{
			Debug.LogWarning("场景中没有设置加载器，可能是因为本场景是点击进入的，而不是从主菜单加载进入的");
			return;
		}

		// 摄像机
		playerController.playerCameraController.mouseSpeed = SettingsLoader.Instance.gameSettingsManager.LoadMouseSensitivity();
		playerController.playerCameraController.normalFOV = SettingsLoader.Instance.gameSettingsManager.LoadCameraFOV();
		playerController.playerCameraController.CalculateCameraFOV(); // 设置摄像机 FOV

		// 摄像机动画
		playerCameraAnim.turnOnThePerspectiveShake = SettingsLoader.Instance.gameSettingsManager.LoadPerspectiveShake();

		// 玩家准星
		frontSight.SetActive(SettingsLoader.Instance.gameSettingsManager.LoadFrontSight());

		// TODO: 这个的值目前是写死的，未来可以考虑设计算法计算这两个值...
		if (SettingsLoader.Instance.gameSettingsManager.LoadScreenInjuryWarning() == 0)
		{
			healthBGController.takeDamageBGMaxOpacity = 0.4f;
			healthBGController.takeDamageBGChangeProportion = 0.013f;
		}
		if (SettingsLoader.Instance.gameSettingsManager.LoadScreenInjuryWarning() == 1)
		{
			healthBGController.takeDamageBGMaxOpacity = 0.2f;
			healthBGController.takeDamageBGChangeProportion = 0.017f;
		}

		// 修正玩家受伤屏幕显示
		healthBGController.FixTakeDamageBG();

		// 是否显示教程信息
		tutorialTipTrigger.openTutorial = SettingsLoader.Instance.gameSettingsManager.LoadDisplayTutorialInformation();
	}

	#endregion

}
