using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �ӵ��ǿ�����
/// </summary>
public class BulletShellController : MonoBehaviour
{
	#region ��������ͱ���

	[Header("��������������ʧ")]
	[SerializeField] private float destroyBulletShellTime;

	[Header("������ת�ٶ�")]
	[Tooltip("�ӵ��׿���ת�ŷɳ�ȥ�ģ����������������ת�ٶ�")]
	[SerializeField] private float rotateSpeed;

	[Header("��Ч������")]
	[SerializeField] private BulletShellSound bulletShellSound;

	private bool isGrounded; // �����Ƿ��������

	#endregion

	#region �����������ں���

	private void Start()
	{
		Destroy(gameObject, destroyBulletShellTime); // ��һ��ʱ���ɾ���Լ�
	}

	private void Update()
	{
		RotateBulletShell();
	}

	#endregion

	#region ���Ƿ��й���

	private void RotateBulletShell()
	{
		// if (isGrounded) return;
		transform.Rotate(new Vector3(0f, rotateSpeed * Time.deltaTime, 0f));
	}

	#endregion

	#region ������ײ��غ���

	private void OnTriggerEnter(Collider other)
	{
		if (!isGrounded) // ��ֹ�����Ч��������
		{
			bulletShellSound.PlayPistolBulletShellSound(); // ���ŵ��������Ч
		}

		isGrounded = true;
	}

	#endregion
}
