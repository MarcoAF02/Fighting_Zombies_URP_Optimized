using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 玩家展示捡起的重要物品
/// </summary>
public class PlayerViewItemController : MonoBehaviour
{
	#region 基本组件和变量

	[Header("玩家主控制器")]
	[SerializeField] private PlayerController playerController;
	[Header("展示物体的根物体")]
	[SerializeField] private GameObject viewObjectRoot;
	[Header("当前展示的物体 ID（也是 GameObject 的名称）")]
	[SerializeField] private string currentViewItemID;

	#endregion

	#region 物品展示和隐藏功能

	public void ShowItemGameObject(string itemID)
	{
		for (int i = 0; i < viewObjectRoot.transform.childCount; i ++)
		{
			if (viewObjectRoot.transform.GetChild(i).name == itemID)
			{
				viewObjectRoot.transform.GetChild(i).gameObject.SetActive(true);
				currentViewItemID = itemID;
				return;
			}
		}

		Debug.LogWarning("没有找到可以展示的游戏模型，请检查是否设置正确了展示用 GameObject");
		currentViewItemID = string.Empty;
	}

	public void HideItemGameObject()
	{
		if (currentViewItemID == string.Empty)
		{
			Debug.Log("上次拾取物品没有展示任何东西");
			playerController.SwitchState(playerController.playerControlState);
			return;
		}

		for (int i = 0; i < viewObjectRoot.transform.childCount; i++)
		{
			if (viewObjectRoot.transform.GetChild(i).name == currentViewItemID)
			{
				viewObjectRoot.transform.GetChild(i).gameObject.SetActive(false);
				playerController.SwitchState(playerController.playerControlState);
				return;
			}
		}
	}

	#endregion
}
