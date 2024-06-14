using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 玩家和家具的交互类
/// </summary>
public class PlayerFurnitureInteractive : MonoBehaviour
{
	#region 基本组件和变量

	private FPS_Play_Action fpsPlayAction;

	[Header("玩家控制器")]
	[SerializeField] private PlayerController playerController;
	[Header("玩家能和家具交互的距离")]
	[SerializeField] private float interactiveDistance;

	[Header("玩家和家具交互的 CD 时间")]
	[SerializeField] private float interactiveCDTime;
	[HideInInspector] public float interactiveTotalTime;

	#endregion

	#region 基本生命周期函数

	private void Awake()
	{
		fpsPlayAction = new FPS_Play_Action();
		fpsPlayAction.Enable();
	}

	private void OnDestroy()
	{
		fpsPlayAction?.Disable();
	}

	#endregion

	#region 玩家和家具的交互

	/// <summary>
	/// 玩家和家具交互
	/// </summary>
	public void PerformInteractiveFurniture()
	{
		interactiveTotalTime = interactiveTotalTime + Time.deltaTime;
		if (interactiveTotalTime > 32768f) interactiveTotalTime = 32768f;

		if (fpsPlayAction.GamePlay_Keyboard_And_Mouse.Player_Interactive.WasPressedThisFrame() &&
			interactiveTotalTime > interactiveCDTime)
		{
			if (playerController.playerCurrentState != playerController.playerControlState) return;
			PlayerInteractiveWithFurniture();
		}
	}

	private void PlayerInteractiveWithFurniture()
	{
		// 注意：这里用忽略 Layer 而不是选择 Layer 实现，是为了不要浪费 Layer 的数量
		int ignoreLayerIndex = (1 << playerController.layerAndTagCollection_Player.playerLayerIndex) | (1 << playerController.layerAndTagCollection_Player.airWallLayerIndex) | (1 << playerController.layerAndTagCollection_Player.checkDoorDirLayerIndex) | (1 << playerController.layerAndTagCollection_Player.enemyLayerIndex);
		ignoreLayerIndex = ~(ignoreLayerIndex);

		RaycastHit hitObj;

		if (Physics.Raycast(playerController.playerCameraController.playerCamera.transform.position, playerController.playerCameraController.playerCamera.transform.forward, out hitObj, interactiveDistance, ignoreLayerIndex))
		{
			if (hitObj.collider.CompareTag(playerController.layerAndTagCollection_Player.furnitureTag))
			{
				hitObj.collider.GetComponentInParent<FurnitureInterController>().InteractiveWithFurniture();
			}
		}
	}

	#endregion

}
