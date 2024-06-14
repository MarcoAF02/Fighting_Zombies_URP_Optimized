using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// 目标提示和物品栏 UI
/// </summary>
public class InventoryAndGameTargetController : MonoBehaviour
{
	#region 基本组件和变量

	[Header("物品栏背包 UI 根物体")]
	public GameObject inventoryUIRootObj;

	[Header("游戏目标提示文本")]
	[SerializeField] private TextMeshProUGUI gameTargetTipMessageUI;
	[Header("UI 组件预制件")]
	[SerializeField] private GameObject itemUIPrefab;
	[Header("玩家物品栏父物体")]
	[SerializeField] private Transform contentTrans;

	#endregion

	#region 基本生命周期函数

	private void Start()
	{
		GameProgressManager.Instance.eventHandler_GameManager.ChangeGameProgressEvent += DisplayGameTargetOnUI;
	}

	private void OnDestroy()
	{
		try
		{
			GameProgressManager.Instance.eventHandler_GameManager.ChangeGameProgressEvent -= DisplayGameTargetOnUI;
		}
		catch
		{
			Debug.Log("游戏已经结束，无需取消订阅事件");
		}
	}

	#endregion

	#region UI 生成并显示物品

	/// <summary>
	/// 根据传入的 ID 生成对应 UI 物品
	/// </summary>
	/// <param name="_itemID"></param>
	public void GenerateAndDisplayItem(string _itemID)
	{
		for (int i = 0; i < PickableItemMessageManager.Instance.pickableItemMessageListToUse.Count; i ++)
		{
			if (_itemID == PickableItemMessageManager.Instance.pickableItemMessageListToUse[i]._itemID)
			{
				GameObject newUI = Instantiate(itemUIPrefab, contentTrans);
				newUI.transform.SetParent(contentTrans);

				newUI.GetComponent<KeyItemDisplayUIController>()._itemID = _itemID;
				newUI.GetComponent<KeyItemDisplayUIController>()._itemDescribe.text = PickableItemMessageManager.Instance.pickableItemMessageListToUse[i]._itemDescribe;
				newUI.GetComponent<KeyItemDisplayUIController>()._itemIcon.sprite = PickableItemMessageManager.Instance.pickableItemMessageListToUse[i]._itemIcon;
			}
		}
	}

	/// <summary>
	/// 物品被使用，根据传入的 ID 删除对应 UI 物品
	/// </summary>
	/// <param name="_itemID"></param>
	public void DestroyAndHideDisplayItem(string _itemID)
	{
		for (int i = 0; i < contentTrans.childCount; i ++)
		{
			if (_itemID == contentTrans.GetChild(i).GetComponent<KeyItemDisplayUIController>()._itemID)
			{
				Destroy(contentTrans.GetChild(i).gameObject);
			}
		}
	}

	#endregion

	#region 根据不同的游戏阶段显示游戏目标（在物品栏左上角）

	private void DisplayGameTargetOnUI(GameProgress _gameProgress)
	{
		if (_gameProgress == GameProgress.None)
		{
			Debug.LogWarning("当前关卡没有正确设置游戏阶段，请检查关卡设计");
			return;
		}

		if (_gameProgress == GameProgress.EarlyStage)
		{
			gameTargetTipMessageUI.text = GameProgressManager.Instance.GameTargetTip;
		}

		if (_gameProgress == GameProgress.LaterStage)
		{
			gameTargetTipMessageUI.text = GameProgressManager.Instance.GameTargetTip + '\n' + GameProgressManager.Instance.GameLaterTargetTip;
		}
	}

	#endregion

}
