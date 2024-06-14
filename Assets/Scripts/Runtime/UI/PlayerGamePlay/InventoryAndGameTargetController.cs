using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// Ŀ����ʾ����Ʒ�� UI
/// </summary>
public class InventoryAndGameTargetController : MonoBehaviour
{
	#region ��������ͱ���

	[Header("��Ʒ������ UI ������")]
	public GameObject inventoryUIRootObj;

	[Header("��ϷĿ����ʾ�ı�")]
	[SerializeField] private TextMeshProUGUI gameTargetTipMessageUI;
	[Header("UI ���Ԥ�Ƽ�")]
	[SerializeField] private GameObject itemUIPrefab;
	[Header("�����Ʒ��������")]
	[SerializeField] private Transform contentTrans;

	#endregion

	#region �����������ں���

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
			Debug.Log("��Ϸ�Ѿ�����������ȡ�������¼�");
		}
	}

	#endregion

	#region UI ���ɲ���ʾ��Ʒ

	/// <summary>
	/// ���ݴ���� ID ���ɶ�Ӧ UI ��Ʒ
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
	/// ��Ʒ��ʹ�ã����ݴ���� ID ɾ����Ӧ UI ��Ʒ
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

	#region ���ݲ�ͬ����Ϸ�׶���ʾ��ϷĿ�꣨����Ʒ�����Ͻǣ�

	private void DisplayGameTargetOnUI(GameProgress _gameProgress)
	{
		if (_gameProgress == GameProgress.None)
		{
			Debug.LogWarning("��ǰ�ؿ�û����ȷ������Ϸ�׶Σ�����ؿ����");
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
