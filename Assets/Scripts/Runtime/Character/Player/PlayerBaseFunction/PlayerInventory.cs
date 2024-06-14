using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �����Ʒ��
/// </summary>
public class PlayerInventory : MonoBehaviour
{
	#region ��������ͱ���

	private FPS_Play_Action fpsPlayAction;

	[Header("��ҽ�ɫ������")]
	[SerializeField] private PlayerController playerController;
	[Header("�����Ʒ����Ч")]
	[SerializeField] private PlayerInventoryUISound playerInventoryUISound;
	[Header("�����Ʒ�� UI ������")]
	public InventoryAndGameTargetController inventoryAndGameTargetController;

	[Header("��ҿ�����Ʒ����ʱ����ȴ")]
	[Header("����Ҫ�򿪶������ſ��Թر�")]
	public float openCDTime;
	public float openTotalTime; // �ϴδ򿪾����˶೤ʱ��

	[Header("�رպ����ٶ�����ſ��Դ�")]
	public float closeCDTime;
	public float closeTotalTime; // �ϴιرվ����˶೤ʱ��

	[Header("ר�����ڴ������Կ�׵���Ʒ����������Կ�� ID")]
	[SerializeField] private List<string> playerKeyList = new List<string>();

	#endregion

	#region �����������ں���

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

	#region ��Ҵ򿪺͹ر���Ʒ��

	/// <summary>
	/// ��Ҵ���Ʒ��
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
	/// ��ҹر���Ʒ��
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

	#region �����Ʒ����������

	/// <summary>
	/// ��Կ���б�������һ��Կ��
	/// </summary>
	/// <param name="keyID"></param>
	public void AddNewKeyToList(string keyID)
	{
		for (int i = 0; i < playerKeyList.Count; i ++)
		{
			if (playerKeyList[i] == string.Empty)
			{
				playerKeyList[i] = keyID;
				inventoryAndGameTargetController.GenerateAndDisplayItem(keyID); // ������ʾ�� UI
				return;
			}
		}

		Debug.LogWarning("��ұ���û���㹻�Ŀռ�");
	}

	/// <summary>
	/// ���������Ʒ�����Ƿ���ĳ��Կ��
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
	/// Կ��ʹ�ú󣬴ӱ������Ƴ���ӦԿ��
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

		Debug.LogWarning("�����Ʒ����û�� ID Ϊ " + keyID + " ��Կ��");
	}

	#endregion
}
