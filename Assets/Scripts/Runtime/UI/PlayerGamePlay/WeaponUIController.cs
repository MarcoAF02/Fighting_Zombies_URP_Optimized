using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 武器 UI 管理器
/// </summary>
public class WeaponUIController : MonoBehaviour
{
	// 该脚本管理武器所需的 UI，主要是准星相关

	#region 基本组件和变量

	[Header("玩家角色控制器")]
	[SerializeField] private PlayerController playerController;
	[Header("玩家准星根物体")]
	[SerializeField] private GameObject frontSightParent;

	#endregion

	#region 基本生命周期函数

	private void OnEnable()
	{
		playerController.eventHandler_Player.PlayerStateChangeEvent += ShowAndHideFrontSight;
	}

	private void OnDisable()
	{
		playerController.eventHandler_Player.PlayerStateChangeEvent -= ShowAndHideFrontSight;
	}

	#endregion

	#region 玩家准星相关功能

	private void ShowAndHideFrontSight(PlayerBaseState playerState)
	{
		if (playerState == playerController.playerControlState)
		{
			frontSightParent.SetActive(true);
		}
		else
		{
			frontSightParent.SetActive(false);
		}
	}

	#endregion
}
