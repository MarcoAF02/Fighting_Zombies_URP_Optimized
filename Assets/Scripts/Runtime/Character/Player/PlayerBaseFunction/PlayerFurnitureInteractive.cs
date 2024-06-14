using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ��ҺͼҾߵĽ�����
/// </summary>
public class PlayerFurnitureInteractive : MonoBehaviour
{
	#region ��������ͱ���

	private FPS_Play_Action fpsPlayAction;

	[Header("��ҿ�����")]
	[SerializeField] private PlayerController playerController;
	[Header("����ܺͼҾ߽����ľ���")]
	[SerializeField] private float interactiveDistance;

	[Header("��ҺͼҾ߽����� CD ʱ��")]
	[SerializeField] private float interactiveCDTime;
	[HideInInspector] public float interactiveTotalTime;

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

	#region ��ҺͼҾߵĽ���

	/// <summary>
	/// ��ҺͼҾ߽���
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
		// ע�⣺�����ú��� Layer ������ѡ�� Layer ʵ�֣���Ϊ�˲�Ҫ�˷� Layer ������
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
