using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ��Һͻ����е�С���巢����������
/// </summary>
public class PlayerHitEnvirObj : MonoBehaviour
{
	#region ��������ͱ���

	[Header("��ҽ�ɫ������")]
	[SerializeField] private PlayerController playerController;

	[Header("����ƶ��������������")]
	[SerializeField] private float playerHitObjForce;

	private Vector3 pushDir = Vector3.zero;

	#endregion

	#region ������ײ����

	private void OnControllerColliderHit(ControllerColliderHit hit)
	{
		Rigidbody body = hit.collider.attachedRigidbody;

		if (body == null || body.isKinematic) return;
		if (hit.moveDirection.y < -0.3) return; // ���������Ƶ�����ȥ

		pushDir = new Vector3(hit.moveDirection.x, 0f, hit.moveDirection.z);
		body.linearVelocity = pushDir * playerHitObjForce;
	}

	#endregion

}
