using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ��¼����ֱ۵�״̬
/// </summary>
public enum ArmState
{
	None, // ʲôҲû����
	TopWall // ��סǽ��
}

public class FPSBodyController : MonoBehaviour
{
	#region ��������ͱ���

	[Header("��һ�˽�ɫ������")]
	[SerializeField] private PlayerController playerController;

	[Header("��һ�˳��ֱ�ģ�͸�����")]
	public GameObject fpsArmRoot;

	[Header("ģ�ͳ�ʼλ��")]
	[SerializeField] private Transform originalTrans;
	[Header("ģ������Ŀ��λ��")]
	[SerializeField] private Transform targetTrans;

	[Header("��ɫǰ����ס�ϰ���ʱ���ֱ�ģ�����Ƶ�λ��")]
	[SerializeField] private Transform topWallTrans;
	[Header("�ж϶�ס�ϰ���ľ���")]
	[SerializeField] private float checkWallDistance;

	[Header("��ֵ�ƶ��ٶ�")]
	[Header("��·ʱ�Ĳ�ֵ�ٶ�")]
	[SerializeField] private float walkLerpSpeed;
	[Header("�ܲ�ʱ�Ĳ�ֵ�ٶ�")]
	[SerializeField] private float runLerpSpeed;
	[Header("�¶�ʱ�Ĳ�ֵ�ٶ�")]
	[SerializeField] private float crouchLerpSpeed;
	[Header("��׼ʱ�Ĳ�ֵ�ٶ�")]
	[SerializeField] private float aimingLerpSpeed;
	[Header("��ǽʱ�Ĳ�ֵ�ٶ�")]
	[SerializeField] private float topWallLerpSpeed;

	[Header("��¼����ֱ۵�ǰ��״̬")]
	public ArmState currentArmState = ArmState.None;
	[Header("����ֱ۶�ǽ�ĵ�λ")]
	[SerializeField] private List<Transform> checkArmTopWallTransList = new List<Transform>();

	private float moveLerpSpeed; // ʵ��ʹ�õĲ�ֵ�ٶ�

	#endregion

	#region �����������ں���

	private void Update()
	{
		if (currentArmState == ArmState.None)
		{
			CheckPlayerMoveState();
			FPSArmFollow();
		}

		ArmDownWhenTopWall();
	}

	#endregion

	#region ��һ�˳��ֱ۶�ǽ�ջع���

	/// <summary>
	/// ����ֱ۶���ǽʱ�����ֱ��ջ�
	/// </summary>
	public void ArmDownWhenTopWall()
	{
		// ���﷢��������ߣ���һ�������˾��㶥ǽ

		// ע�⣺�����ú��� Layer ������ѡ�� Layer ʵ�֣���Ϊ�˲�Ҫ�˷� Layer ������
		int ignoreLayerIndex = (1 << playerController.layerAndTagCollection_Player.playerLayerIndex) | (1 << playerController.layerAndTagCollection_Player.airWallLayerIndex) | (1 << playerController.layerAndTagCollection_Player.checkDoorDirLayerIndex) | (1 << playerController.layerAndTagCollection_Player.doorInteractiveLayerIndex) | (1 << playerController.layerAndTagCollection_Player.enemyLayerIndex);
		ignoreLayerIndex = ~(ignoreLayerIndex);

		RaycastHit hitObj;

		for (int i = 0; i < checkArmTopWallTransList.Count; i ++)
		{
			if (Physics.Raycast(checkArmTopWallTransList[i].position, checkArmTopWallTransList[i].forward, out hitObj, checkWallDistance, ignoreLayerIndex))
			{
				if (!hitObj.collider.isTrigger)
				{
					currentArmState = ArmState.TopWall;
					fpsArmRoot.transform.localPosition = Vector3.Lerp(fpsArmRoot.transform.localPosition, topWallTrans.localPosition, topWallLerpSpeed * Time.deltaTime);

					// Debug.Log(currentArmState);
					return;
				}
			}
		}

		currentArmState = ArmState.None;
		// Debug.Log(currentArmState);
	}

	#endregion

	#region ��ֵ���湦��

	/// <summary>
	/// ������ҿ������������ƶ�״̬
	/// </summary>
	private void CheckPlayerMoveState()
	{
		if (playerController.isAiming)
		{
			moveLerpSpeed = aimingLerpSpeed;
			return;
		}

		if (playerController.playerIsRun)
		{
			moveLerpSpeed = runLerpSpeed;
			return;
		}

		if (playerController.playerIsCrouch)
		{
			moveLerpSpeed = crouchLerpSpeed;
			return;
		}

		else
		{
			moveLerpSpeed = walkLerpSpeed;
		}
	}

	private void FPSArmFollow()
	{
		if (playerController.isAiming)
		{
			fpsArmRoot.transform.localPosition = Vector3.Lerp(fpsArmRoot.transform.localPosition, originalTrans.localPosition, moveLerpSpeed * Time.deltaTime);
			return;
		}

		if (playerController.moveVelocity > 0.5f)
		{
			fpsArmRoot.transform.localPosition = Vector3.Lerp(fpsArmRoot.transform.localPosition, targetTrans.localPosition, moveLerpSpeed * Time.deltaTime);
		}
		else
		{
			fpsArmRoot.transform.localPosition = Vector3.Lerp(fpsArmRoot.transform.localPosition, originalTrans.localPosition, moveLerpSpeed * Time.deltaTime);
		}
	}

	#endregion
}
