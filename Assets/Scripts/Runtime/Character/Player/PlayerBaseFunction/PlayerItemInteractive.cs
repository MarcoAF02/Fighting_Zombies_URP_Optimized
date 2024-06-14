using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 玩家和可拾取物品的交互类
/// </summary>
public class PlayerItemInteractive : MonoBehaviour
{
	#region 基本组件和变量

	private FPS_Play_Action fpsPlayAction;

	[Header("玩家角色控制器")]
	[SerializeField] private PlayerController playerController;
	[Header("玩家提示信息控制器")]
	[SerializeField] private TipMessageController tipMessageController;
	[Header("玩家物品查看器")]
	[SerializeField] private PlayerViewItemController playerViewItem;
	[Header("玩家物品栏")]
	[SerializeField] private PlayerInventory playerInventory;
	[Header("拾取音效控制器")]
	[SerializeField] private PlayerPickupItemSound playerPickupItemSound;

	[Header("玩家可以隔多远拾取物品")]
	[SerializeField] private float interactiveDistance;

	[Header("玩家和物品交互的 CD 时间")]
	[SerializeField] private float interactiveCDTime;
	[HideInInspector] public float interactiveTotalTime;

	[Header("玩家拾取物品后，将可视物品隐藏的 CD 时间")]
	[SerializeField] private float hideViewItemCDTime;
	[HideInInspector] public float hideViewItemTotalTime;
	[Header("玩家拾取物品后，下方提示信息的显示时长")]
	[SerializeField] private float interTipMessageDisplayTime;
	[Header("玩家拾取物品后，下方提示信息的背景")]
	[SerializeField] private Color interTipMessageBGColor;

	#endregion

	#region 基本生命周期函数

	private void Awake()
	{
		fpsPlayAction = new FPS_Play_Action();
		fpsPlayAction.Enable();

		// 初始化 CD 时间
		interactiveTotalTime = interactiveCDTime;
		hideViewItemTotalTime = 0f;
	}

	private void Update()
	{
		PlayerCollectItem();
	}

	private void OnDestroy()
	{
		fpsPlayAction?.Disable();
	}

	#endregion

	#region 玩家和可拾取物品的交互

	// 注意：这两个玩家操作函数都被状态机全权管理

	/// <summary>
	/// 玩家拾取物品
	/// </summary>
	public void PlayerCollectItem()
	{
		interactiveTotalTime = interactiveTotalTime + Time.deltaTime;

		if (interactiveTotalTime > 32768f) interactiveTotalTime = 32768f;

		if (playerController.playerCurrentState != playerController.playerControlState) return;
		if (playerController.isAiming) return;
		if (playerController.weaponManager.playerGunState != PlayerGunState.Standby) return;

		if (fpsPlayAction.GamePlay_Keyboard_And_Mouse.Player_Interactive.WasPressedThisFrame() &&
			interactiveTotalTime > interactiveCDTime)
		{
			PlayerInteractiveWithItem();
		}
	}

	public void PlayerHideShowItemObj()
	{
		if (fpsPlayAction.GamePlay_Keyboard_And_Mouse.Player_Interactive.WasPressedThisFrame() &&
			hideViewItemTotalTime > hideViewItemCDTime)
		{
			playerViewItem.HideItemGameObject();
		}
	}

	// 物品交互核心功能
	private void PlayerInteractiveWithItem()
	{
		// 注意：这里用忽略 Layer 而不是选择 Layer 实现，是为了不要浪费 Layer 的数量
		int ignoreLayerIndex = (1 << playerController.layerAndTagCollection_Player.playerLayerIndex) | (1 << playerController.layerAndTagCollection_Player.airWallLayerIndex) | (1 << playerController.layerAndTagCollection_Player.checkDoorDirLayerIndex) | (1 << playerController.layerAndTagCollection_Player.enemyLayerIndex) | (1 << playerController.layerAndTagCollection_Player.doorInteractiveLayerIndex);
		ignoreLayerIndex = ~(ignoreLayerIndex);

		RaycastHit hitObj;

		if (Physics.Raycast(playerController.playerCameraController.playerCamera.transform.position, playerController.playerCameraController.playerCamera.transform.forward, out hitObj, interactiveDistance, ignoreLayerIndex))
		{
			// Debug.Log(hitObj.collider.name);

			if (hitObj.collider.CompareTag(playerController.layerAndTagCollection_Player.itemTag))
			{
				playerController.playerFurnitureInteractive.interactiveTotalTime = 0f; // 捡物品的时候先重置家具交互 CD 时间，避免拾取物品时重复操作家具

				if (hitObj.collider.GetComponent<PickableItem>().showTipMessage) // 拾取的物品是否需要显示物品信息
				{
					tipMessageController.ShowInteractiveMessage(hitObj.collider.GetComponent<PickableItem>().tipMessage, interTipMessageDisplayTime, interTipMessageBGColor);
				}

				if (hitObj.collider.GetComponent<PickableItem>().canNoticePlayer) // 拾取的物品是否要提示玩家游戏阶段改变
				{
					GameProgressManager.Instance.NoticePlayerGameTarget(hitObj.collider.GetComponent<PickableItem>().gameTargetTipState);
				}

				if (hitObj.collider.GetComponent<PickableItem>().itemType == ItemType.Key) // 捡到的是解谜关键物品
				{
					playerController.SwitchState(playerController.playerViewState); // 进入查看物品状态
					playerInventory.AddNewKeyToList(hitObj.collider.GetComponent<PickableItem>().itemID);

					// 在第一人称相机下展示对应的钥匙模型
					// 注意：在镜头前展示的钥匙模型名称应该和钥匙 ID 保持一致（否则找不到对应模型）
					playerViewItem.ShowItemGameObject(hitObj.collider.GetComponent<PickableItem>().itemID);

					playerPickupItemSound.PlayPickupSound(ItemType.Key, hitObj.collider.GetComponent<PickableItem>().itemID); // 播放音效

					// 最后，关闭地上的游戏物体显示
					hitObj.collider.gameObject.SetActive(false);
				}

				if (hitObj.collider.GetComponent<PickableItem>().itemType == ItemType.Supply)
				{
					if (hitObj.collider.GetComponent<PickableItem>().itemID == playerController.layerAndTagCollection_Player.pistolAmmoItemID)
					{
						playerController.weaponManager.playerPistolShooting.SupplementBullet(hitObj.collider.GetComponent<PickableItem>().supplies);
						playerPickupItemSound.PlayPickupSound(ItemType.Supply, hitObj.collider.GetComponent<PickableItem>().itemID); // 播放音效
						hitObj.collider.gameObject.SetActive(false);
					}

					if (hitObj.collider.GetComponent<PickableItem>().itemID == playerController.layerAndTagCollection_Player.medicineItemID)
					{
						playerController.weaponManager.playerSyringeUsing.SupplementMedicine(hitObj.collider.GetComponent<PickableItem>().supplies);
						playerPickupItemSound.PlayPickupSound(ItemType.Supply, hitObj.collider.GetComponent<PickableItem>().itemID); // 播放音效
						hitObj.collider.gameObject.SetActive(false);
					}
				}
			}
		}

	}

	#endregion
}
