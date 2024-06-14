using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ��ҺͿ�ʰȡ��Ʒ�Ľ�����
/// </summary>
public class PlayerItemInteractive : MonoBehaviour
{
	#region ��������ͱ���

	private FPS_Play_Action fpsPlayAction;

	[Header("��ҽ�ɫ������")]
	[SerializeField] private PlayerController playerController;
	[Header("�����ʾ��Ϣ������")]
	[SerializeField] private TipMessageController tipMessageController;
	[Header("�����Ʒ�鿴��")]
	[SerializeField] private PlayerViewItemController playerViewItem;
	[Header("�����Ʒ��")]
	[SerializeField] private PlayerInventory playerInventory;
	[Header("ʰȡ��Ч������")]
	[SerializeField] private PlayerPickupItemSound playerPickupItemSound;

	[Header("��ҿ��Ը���Զʰȡ��Ʒ")]
	[SerializeField] private float interactiveDistance;

	[Header("��Һ���Ʒ������ CD ʱ��")]
	[SerializeField] private float interactiveCDTime;
	[HideInInspector] public float interactiveTotalTime;

	[Header("���ʰȡ��Ʒ�󣬽�������Ʒ���ص� CD ʱ��")]
	[SerializeField] private float hideViewItemCDTime;
	[HideInInspector] public float hideViewItemTotalTime;
	[Header("���ʰȡ��Ʒ���·���ʾ��Ϣ����ʾʱ��")]
	[SerializeField] private float interTipMessageDisplayTime;
	[Header("���ʰȡ��Ʒ���·���ʾ��Ϣ�ı���")]
	[SerializeField] private Color interTipMessageBGColor;

	#endregion

	#region �����������ں���

	private void Awake()
	{
		fpsPlayAction = new FPS_Play_Action();
		fpsPlayAction.Enable();

		// ��ʼ�� CD ʱ��
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

	#region ��ҺͿ�ʰȡ��Ʒ�Ľ���

	// ע�⣺��������Ҳ�����������״̬��ȫȨ����

	/// <summary>
	/// ���ʰȡ��Ʒ
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

	// ��Ʒ�������Ĺ���
	private void PlayerInteractiveWithItem()
	{
		// ע�⣺�����ú��� Layer ������ѡ�� Layer ʵ�֣���Ϊ�˲�Ҫ�˷� Layer ������
		int ignoreLayerIndex = (1 << playerController.layerAndTagCollection_Player.playerLayerIndex) | (1 << playerController.layerAndTagCollection_Player.airWallLayerIndex) | (1 << playerController.layerAndTagCollection_Player.checkDoorDirLayerIndex) | (1 << playerController.layerAndTagCollection_Player.enemyLayerIndex) | (1 << playerController.layerAndTagCollection_Player.doorInteractiveLayerIndex);
		ignoreLayerIndex = ~(ignoreLayerIndex);

		RaycastHit hitObj;

		if (Physics.Raycast(playerController.playerCameraController.playerCamera.transform.position, playerController.playerCameraController.playerCamera.transform.forward, out hitObj, interactiveDistance, ignoreLayerIndex))
		{
			// Debug.Log(hitObj.collider.name);

			if (hitObj.collider.CompareTag(playerController.layerAndTagCollection_Player.itemTag))
			{
				playerController.playerFurnitureInteractive.interactiveTotalTime = 0f; // ����Ʒ��ʱ�������üҾ߽��� CD ʱ�䣬����ʰȡ��Ʒʱ�ظ������Ҿ�

				if (hitObj.collider.GetComponent<PickableItem>().showTipMessage) // ʰȡ����Ʒ�Ƿ���Ҫ��ʾ��Ʒ��Ϣ
				{
					tipMessageController.ShowInteractiveMessage(hitObj.collider.GetComponent<PickableItem>().tipMessage, interTipMessageDisplayTime, interTipMessageBGColor);
				}

				if (hitObj.collider.GetComponent<PickableItem>().canNoticePlayer) // ʰȡ����Ʒ�Ƿ�Ҫ��ʾ�����Ϸ�׶θı�
				{
					GameProgressManager.Instance.NoticePlayerGameTarget(hitObj.collider.GetComponent<PickableItem>().gameTargetTipState);
				}

				if (hitObj.collider.GetComponent<PickableItem>().itemType == ItemType.Key) // �񵽵��ǽ��չؼ���Ʒ
				{
					playerController.SwitchState(playerController.playerViewState); // ����鿴��Ʒ״̬
					playerInventory.AddNewKeyToList(hitObj.collider.GetComponent<PickableItem>().itemID);

					// �ڵ�һ�˳������չʾ��Ӧ��Կ��ģ��
					// ע�⣺�ھ�ͷǰչʾ��Կ��ģ������Ӧ�ú�Կ�� ID ����һ�£������Ҳ�����Ӧģ�ͣ�
					playerViewItem.ShowItemGameObject(hitObj.collider.GetComponent<PickableItem>().itemID);

					playerPickupItemSound.PlayPickupSound(ItemType.Key, hitObj.collider.GetComponent<PickableItem>().itemID); // ������Ч

					// ��󣬹رյ��ϵ���Ϸ������ʾ
					hitObj.collider.gameObject.SetActive(false);
				}

				if (hitObj.collider.GetComponent<PickableItem>().itemType == ItemType.Supply)
				{
					if (hitObj.collider.GetComponent<PickableItem>().itemID == playerController.layerAndTagCollection_Player.pistolAmmoItemID)
					{
						playerController.weaponManager.playerPistolShooting.SupplementBullet(hitObj.collider.GetComponent<PickableItem>().supplies);
						playerPickupItemSound.PlayPickupSound(ItemType.Supply, hitObj.collider.GetComponent<PickableItem>().itemID); // ������Ч
						hitObj.collider.gameObject.SetActive(false);
					}

					if (hitObj.collider.GetComponent<PickableItem>().itemID == playerController.layerAndTagCollection_Player.medicineItemID)
					{
						playerController.weaponManager.playerSyringeUsing.SupplementMedicine(hitObj.collider.GetComponent<PickableItem>().supplies);
						playerPickupItemSound.PlayPickupSound(ItemType.Supply, hitObj.collider.GetComponent<PickableItem>().itemID); // ������Ч
						hitObj.collider.gameObject.SetActive(false);
					}
				}
			}
		}

	}

	#endregion
}
