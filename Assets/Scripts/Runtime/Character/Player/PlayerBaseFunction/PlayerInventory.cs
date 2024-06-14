using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 玩家物品栏
/// </summary>
public class PlayerInventory : MonoBehaviour
{
	#region 基本组件和变量

	private FPS_Play_Action fpsPlayAction;

	[Header("玩家角色控制器")]
	[SerializeField] private PlayerController playerController;
	[Header("玩家物品栏音效")]
	[SerializeField] private PlayerInventoryUISound playerInventoryUISound;
	[Header("玩家物品栏 UI 控制器")]
	public InventoryAndGameTargetController inventoryAndGameTargetController;

	[Header("玩家开关物品栏的时间冷却")]
	[Header("至少要打开多少秒后才可以关闭")]
	public float openCDTime;
	public float openTotalTime; // 上次打开经过了多长时间

	[Header("关闭后至少多少秒才可以打开")]
	public float closeCDTime;
	public float closeTotalTime; // 上次关闭经过了多长时间

	[Header("专门用于储存玩家钥匙的物品栏，仅储存钥匙 ID")]
	[SerializeField] private List<string> playerKeyList = new List<string>();

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

	#region 玩家打开和关闭物品栏

	/// <summary>
	/// 玩家打开物品栏
	/// </summary>
	public void PlayerOpenInventory()
	{
		if (fpsPlayAction.UI_Keyboard_And_Mouse.Player_OpenInventory.WasPressedThisFrame() &&
			playerController.weaponManager.playerGunState == PlayerGunState.Standby &&
			!playerController.isAiming && closeTotalTime <= 0f)
		{
			playerInventoryUISound.PlayOpenInventorySound();
			playerController.SwitchState(playerController.playerOpenInventoryState);
		}
	}

	/// <summary>
	/// 玩家关闭物品栏
	/// </summary>
	public void PlayerCloseInventory()
	{
		if (fpsPlayAction.UI_Keyboard_And_Mouse.Player_OpenInventory.WasPressedThisFrame() ||
			fpsPlayAction.UI_Keyboard_And_Mouse.Player_OpenPauseMenu.WasPressedThisFrame())
		{
			if (openTotalTime > 0f) return;
			playerInventoryUISound.PlayCloseInventorySound();
			playerController.SwitchState(playerController.playerControlState);
		}
	}

	#endregion

	#region 玩家物品栏基本功能

	/// <summary>
	/// 在钥匙列表里新增一把钥匙
	/// </summary>
	/// <param name="keyID"></param>
	public void AddNewKeyToList(string keyID)
	{
		for (int i = 0; i < playerKeyList.Count; i ++)
		{
			if (playerKeyList[i] == string.Empty)
			{
				playerKeyList[i] = keyID;
				inventoryAndGameTargetController.GenerateAndDisplayItem(keyID); // 更新显示的 UI
				return;
			}
		}

		Debug.LogWarning("玩家背包没有足够的空间");
	}

	/// <summary>
	/// 查找玩家物品栏中是否有某把钥匙
	/// </summary>
	/// <param name="keyID"></param>
	/// <returns></returns>
	public bool CheckPlayerHaveKey(string keyID)
	{
		for (int i = 0; i < playerKeyList.Count; i ++)
		{
			if (playerKeyList[i].Equals(keyID))
			{
				return true;
			}
		}

		return false;
	}

	/// <summary>
	/// 钥匙使用后，从背包中移除对应钥匙
	/// </summary>
	/// <param name="keyID"></param>
	public void RemoveKey(string keyID)
	{
		for (int i = 0; i < playerKeyList.Count; i ++)
		{
			if (playerKeyList[i] == keyID)
			{
				inventoryAndGameTargetController.DestroyAndHideDisplayItem(keyID);
				playerKeyList[i] = string.Empty;
				return;
			}
		}

		Debug.LogWarning("玩家物品栏中没有 ID 为 " + keyID + " 的钥匙");
	}

	#endregion
}
