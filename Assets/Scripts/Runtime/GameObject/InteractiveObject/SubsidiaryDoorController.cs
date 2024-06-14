using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ���Ÿ������ſ�����������ʵ��˫���ţ�
/// </summary>
public class SubsidiaryDoorController : MonoBehaviour
{
	#region ��������ͱ���

	[Header("�ſ��ص�λ����Ϣ")]
	[Tooltip("���÷����б�ʱ��Ӧ�ö�̬�޸��������ֵ")]
	[SerializeField] private Vector3 openInTrans = new Vector3(0, 90, 0);
	[SerializeField] private Vector3 openOutTrans = new Vector3(0, -90, 0);
	[SerializeField] private Vector3 closeTrans = new Vector3(0, 0, 0);

	[Header("������ס��ʱ�򣬲��Ŷ�����Ҫ�õ�λ����Ϣ")]
	[SerializeField] private Vector3 lockInTrans = new Vector3(0, 2, 0);
	[SerializeField] private Vector3 lockOutTrans = new Vector3(0, -2, 0);

	[Header("����תʱ����С�Ƕ�����")]
	[Tooltip("���Ų�ֵ��תʮ�ֽӽ�Ŀ��ʱ�������Ŀ��Ĳ��С�����ֵ��ǿ�и��ŵ���ת��Ϣ��ֵ�����Ծ�����λʱ������Һ��ŵĽ���Ƶ��")]
	[SerializeField] private float minRotateValue = 0.5f;

	[Header("�����ŵ��ٶ�")]
	[SerializeField] private float doorMoveSpeed = 5f;
	[Header("�����ϵ�ʱ�򣬶������ŵ��ٶ�")]
	[SerializeField] private float doorLockedMoveSpeed = 18f;
	[Header("�����ϵ�ʱ�򣬶������ŵ�������")]
	[SerializeField] private int maxLockedDoorMoveCount = 2;
	[HideInInspector] public bool isOnceOpen; // �����ס�����Ƿ��ǵ�һ�δ�
	private int lockedDoorMoveCount;

	#endregion

	#region �����ŵĿ��ع���

	public void SubsidiaryDoorPerformRotate(float checkPlayerPos, bool toOpen)
	{
		StartCoroutine(DoorRotate(checkPlayerPos, toOpen));
	}

	public void SubsidiaryDoorClosed(float checkPlayerPos)
	{
		StartCoroutine(DoorLocked(checkPlayerPos));
	}

	private IEnumerator DoorRotate(float checkPlayerPos, bool toOpen)
	{
		if (toOpen) // ��Ҫ����
		{
			if (checkPlayerPos > 0)
			{
				while (true)
				{
					if (Quaternion.Angle(transform.localRotation, Quaternion.Euler(openInTrans)) < minRotateValue)
					{
						transform.localRotation = Quaternion.Euler(openInTrans);
						yield break;
					}

					transform.localRotation = Quaternion.Slerp(transform.localRotation, Quaternion.Euler(openInTrans), doorMoveSpeed * Time.deltaTime);
					yield return null;
				}
			}
			else
			{
				while (true)
				{
					if (Quaternion.Angle(transform.localRotation, Quaternion.Euler(openOutTrans)) < minRotateValue)
					{
						transform.localRotation = Quaternion.Euler(openOutTrans);
						yield break;
					}

					transform.localRotation = Quaternion.Slerp(transform.localRotation, Quaternion.Euler(openOutTrans), doorMoveSpeed * Time.deltaTime);
					yield return null;
				}
			}
		}
		else // ��Ҫ����
		{
			while (true)
			{
				if (Quaternion.Angle(transform.localRotation, Quaternion.Euler(closeTrans)) < minRotateValue)
				{
					transform.localRotation = Quaternion.Euler(closeTrans);
					yield break;
				}

				transform.localRotation = Quaternion.Slerp(transform.localRotation, Quaternion.Euler(closeTrans), doorMoveSpeed * Time.deltaTime);
				yield return null;
			}
		}
	}

	private IEnumerator DoorLocked(float checkPlayerPos)
	{
		lockedDoorMoveCount++;

		if (checkPlayerPos > 0) // ������λ��
		{
			while (true)
			{
				if (Quaternion.Angle(transform.localRotation, Quaternion.Euler(lockInTrans)) < minRotateValue)
				{
					transform.localRotation = Quaternion.Euler(lockInTrans);
					StartCoroutine(LockedDoorClose(checkPlayerPos));
					yield break;
				}

				transform.localRotation = Quaternion.Slerp(transform.localRotation, Quaternion.Euler(lockInTrans), doorLockedMoveSpeed * Time.deltaTime);
				yield return null;
			}
		}
		else
		{
			while (true)
			{
				if (Quaternion.Angle(transform.localRotation, Quaternion.Euler(lockOutTrans)) < minRotateValue)
				{
					transform.localRotation = Quaternion.Euler(lockOutTrans);
					StartCoroutine(LockedDoorClose(checkPlayerPos));
					yield break;
				}

				transform.localRotation = Quaternion.Slerp(transform.localRotation, Quaternion.Euler(lockOutTrans), doorLockedMoveSpeed * Time.deltaTime);
				yield return null;
			}
		}
	}

	private IEnumerator LockedDoorClose(float checkPlayerPos)
	{
		while (true)
		{
			if (Quaternion.Angle(transform.localRotation, Quaternion.Euler(closeTrans)) < minRotateValue)
			{
				transform.localRotation = Quaternion.Euler(closeTrans);

				if (lockedDoorMoveCount >= maxLockedDoorMoveCount) // ���Ƽ�����...
				{
					lockedDoorMoveCount = 0;
					yield break;
				}
				else
				{
					StartCoroutine(DoorLocked(checkPlayerPos));
					yield break;
				}
			}

			transform.localRotation = Quaternion.Slerp(transform.localRotation, Quaternion.Euler(closeTrans), doorLockedMoveSpeed * Time.deltaTime);
			yield return null;
		}
	}

	#endregion
}
