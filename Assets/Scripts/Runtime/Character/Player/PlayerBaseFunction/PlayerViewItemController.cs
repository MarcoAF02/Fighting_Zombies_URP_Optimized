using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ���չʾ�������Ҫ��Ʒ
/// </summary>
public class PlayerViewItemController : MonoBehaviour
{
	#region ��������ͱ���

	[Header("�����������")]
	[SerializeField] private PlayerController playerController;
	[Header("չʾ����ĸ�����")]
	[SerializeField] private GameObject viewObjectRoot;
	[Header("��ǰչʾ������ ID��Ҳ�� GameObject �����ƣ�")]
	[SerializeField] private string currentViewItemID;

	#endregion

	#region ��Ʒչʾ�����ع���

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

		Debug.LogWarning("û���ҵ�����չʾ����Ϸģ�ͣ������Ƿ�������ȷ��չʾ�� GameObject");
		currentViewItemID = string.Empty;
	}

	public void HideItemGameObject()
	{
		if (currentViewItemID == string.Empty)
		{
			Debug.Log("�ϴ�ʰȡ��Ʒû��չʾ�κζ���");
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
