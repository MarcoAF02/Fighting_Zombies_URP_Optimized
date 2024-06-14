using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ��Һ��ŵĽ�����
/// </summary>
public class PlayerDoorInteractive : MonoBehaviour
{
	#region ��������ͱ���

	private FPS_Play_Action fpsPlayAction;

	[Header("�����ʾ��Ϣ������")]
	[SerializeField] private TipMessageController tipMessageController;
	[Header("��ҿ�����")]
	[SerializeField] private PlayerController playerController;
	[Header("�����Ʒ��")]
	[SerializeField] private PlayerInventory playerInventory;
	[Header("����ܺ��Ž����ľ���")]
	[SerializeField] private float interactiveDistance;
	[Header("��Һ��Ž����� CD ʱ��")]
	[SerializeField] private float interactiveCDTime;
	[Header("������Ž������·���ʾ��Ϣ����ʾʱ��")]
	[SerializeField] private float interTipMessageDisplayTime;
	[Header("������Ž������·���ʾ��Ϣ�ı���")]
	[SerializeField] private Color interTipMessageBGColor;
	[Header("��Ҷ��ŵļ�⣺����Լ����ŵ�ǰ�����Ǻ�")]
	[Header("������ĵ�")]
	[SerializeField] private Transform doorCheckPoint;
	[Header("��ⷶΧ")]
	[SerializeField] private float doorCheckRadius;
	[Header("���� Layer Index")]
	[SerializeField] private int checkLayerIndex;

	private Dictionary<string, float> distanceDic = new Dictionary<string, float>();
	[HideInInspector] public float interactiveTotalTime;
	private float checkPlayerPos;

	#endregion

	#region �����������ں���

	private void Awake()
	{
		fpsPlayAction = new FPS_Play_Action();
		fpsPlayAction.Enable();

		interactiveTotalTime = interactiveCDTime;
	}

	private void OnDestroy()
	{
		fpsPlayAction?.Disable();
	}

	#endregion

	#region ��Ҷ��ŵĲ���

	/// <summary>
	/// ��Ҳ�����
	/// </summary>
	public void PlayerOperateDoor()
	{
		interactiveTotalTime = interactiveTotalTime + Time.deltaTime;
		if (interactiveTotalTime > 32768f) interactiveTotalTime = 32768f;

		if (fpsPlayAction.GamePlay_Keyboard_And_Mouse.Player_Interactive.WasPressedThisFrame() &&
			interactiveTotalTime > interactiveCDTime)
		{
			if (playerController.playerCurrentState != playerController.playerControlState) return;
			PlayerInteractiveWithDoor();
		}
	}

	// ���Ž������Ĺ���
	private void PlayerInteractiveWithDoor()
	{
		// ע�⣺�����ú��� Layer ������ѡ�� Layer ʵ�֣���Ϊ�˲�Ҫ�˷� Layer ������
		int ignoreLayerIndex = (1 << playerController.layerAndTagCollection_Player.playerLayerIndex) | (1 << playerController.layerAndTagCollection_Player.airWallLayerIndex) | (1 << playerController.layerAndTagCollection_Player.checkDoorDirLayerIndex) | (1 << playerController.layerAndTagCollection_Player.enemyLayerIndex);
		ignoreLayerIndex = ~(ignoreLayerIndex);

		RaycastHit hitObj;

		if (Physics.Raycast(playerController.playerCameraController.playerCamera.transform.position, playerController.playerCameraController.playerCamera.transform.forward, out hitObj, interactiveDistance, ignoreLayerIndex))
		{
			if (hitObj.collider.CompareTag(playerController.layerAndTagCollection_Player.doorTag))
			{
				interactiveTotalTime = 0f;
				checkPlayerPos = 0f;

				hitObj.collider.GetComponentInParent<MainDoorController>().SetCheckDirCollider(true);
				checkPlayerPos = CheckDoorDir();
				hitObj.collider.GetComponentInParent<MainDoorController>().SetCheckDirCollider(false);

				// Debug.Log(checkPlayerPos);

				// �������ҪԿ�ף�������ұ������Ƿ���Կ��
				if (hitObj.collider.GetComponentInParent<MainDoorController>().keyID != string.Empty &&
					hitObj.collider.GetComponentInParent<MainDoorController>().isOnceOpen)
				{
					if (playerInventory.CheckPlayerHaveKey(hitObj.collider.GetComponentInParent<MainDoorController>().keyID))
					{
						hitObj.collider.GetComponentInParent<MainDoorController>().locked = false;
					}
					else // ����������ˣ�����ҷ�����ʾ��Ϣ
					{
						tipMessageController.ShowInteractiveMessage(hitObj.collider.GetComponentInParent<MainDoorController>().tipLockedMessage, interTipMessageDisplayTime, interTipMessageBGColor);

						if (hitObj.collider.GetComponentInParent<MainDoorController>().canNoticePlayer)
						{
							if (GameProgressManager.Instance != null)
							{
								GameProgressManager.Instance.NoticePlayerGameTarget(hitObj.collider.GetComponentInParent<MainDoorController>().gameTargetTipState);

								// �̳���Ϣ��ʾ��Ҽ����ϷĿ�꣨Tab �����Ʒ����
								playerController.tutorialTrigger.TutorialPlayerCheckGameTarget();
							}
						}
					}

					if (!hitObj.collider.GetComponentInParent<MainDoorController>().locked)
					{
						playerInventory.RemoveKey(hitObj.collider.GetComponentInParent<MainDoorController>().keyID); // �����Կ�׿��ź��Ƴ����Կ��
					}
				}
		
				hitObj.collider.GetComponentInParent<MainDoorController>().InteractiveWithDoor(checkPlayerPos);

				// ���������ҪԿ�ף��ں��Ž���һ�κ��ŵĵ�һ�ο���״̬����Ϊ false
				if (hitObj.collider.GetComponentInParent<MainDoorController>().keyID != string.Empty)
				{
					if (hitObj.collider.GetComponentInParent<MainDoorController>().locked == false) // ִ�е������ʾ�����Կ�׳ɹ����Ŵ���
					{
						hitObj.collider.GetComponentInParent<MainDoorController>().isOnceOpen = false;
					}
				}
			}
		}
	}

	/// <summary>
	/// ���������ŵ�ǰ�����Ǻ�
	/// </summary>
	private float CheckDoorDir()
	{
		// ע�⣺�ŵķ�λ����һ��Ҫ���� Door_In �� Door_Out Tag��Layer һ��Ҫ����Ϊ CheckDoorDir�����Ϊ 8��

		distanceDic.Clear();

		Collider[] doorCheckColliderArray = Physics.OverlapSphere(doorCheckPoint.position, doorCheckRadius, 1 << playerController.layerAndTagCollection_Player.checkDoorDirLayerIndex);

		if (doorCheckColliderArray.Length != 2)
		{
			Debug.LogWarning("���û����ȷ��⵽�ŷ�λ�㣨���ǽ��������б���ţ����������Һ��ŵĲ���");
			return -2;
		}

		// ������� Tag �� ����ҵľ��� �������ֵ����ݽṹ��
		for (int i = 0; i < doorCheckColliderArray.Length; i ++)
		{
			if (!distanceDic.ContainsKey(doorCheckColliderArray[i].tag))
			{
				distanceDic.Add(doorCheckColliderArray[i].tag, Vector3.Distance(doorCheckColliderArray[i].transform.position, transform.position));
			}
			else
			{
				Debug.LogWarning("�ż���� Tag δ������ȷ��������Ϸ����");
				return -2;
			}
		}

		if (distanceDic["Door_In"] < distanceDic["Door_Out"])
		{
			return 2;
		}
		else
		{
			return -2;
		}
	}

	#endregion

	#region DEBUG �ú���

	private void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.blue;
		Gizmos.DrawWireSphere(doorCheckPoint.position, doorCheckRadius);
	}

	#endregion
}
