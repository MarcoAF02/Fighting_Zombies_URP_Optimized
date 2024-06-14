using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ���� X ��������������Ϊ��׼����ǰ����Ϊ Open_IN �������Ϊ Open_Out
// Կ�׿��Ŷ���ֻ��������ŵ�����

// ���ſ������������ŵĸ������ϣ������ſ����������ڸ�������Ϸ������

/// <summary>
/// ö�����ͣ��жϵ�ǰ�ŵ�״̬
/// </summary>
public enum DoorState
{
	Moving,
	Open,
	Close
}

/// <summary>
/// ���ſ�����
/// </summary>
public class MainDoorController : MonoBehaviour
{
	#region ��������ͱ���

	[Header("������Ҫ֪ͨ����ҵ���Ϣ")]
	[Header("����ס��")]
	public string tipLockedMessage = "��ס��";

	[Header("��ʾ����ϷĿ��")]
	public bool canNoticePlayer;
	public GameTargetTipState gameTargetTipState;

	[Header("����Ϸ����")]
	[SerializeField] private GameObject doorObject;
	[Header("�����Ƿ��и�����")]
	[SerializeField] private bool haveSubsidiaryDoor;
	[Header("�����ſ�����")]
	[SerializeField] private SubsidiaryDoorController subsidiaryDoorController;

	[Header("����Ч������")]
	[SerializeField] private DoorSound doorSound;

	[Header("���������ҪԿ�׿�������� ID ��¼��Կ�׵�����")]
	public string keyID;
	[HideInInspector] public bool locked;

	[Header("��ҷ�λ���㼯��")]
	[Tooltip("��ҿ���ʱ����������ǰ�滹�Ǻ���")]
	[SerializeField] private List<Collider> playerCheckDirCollider = new List<Collider>();

	[Header("�ſ��ص�λ����Ϣ")]
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
	[SerializeField] private float doorMoveSpeed = 4f;
	[Header("�����ϵ�ʱ�򣬶������ŵ��ٶ�")]
	[SerializeField] private float doorLockedMoveSpeed = 18f;
	[Header("�����ϵ�ʱ�򣬶������ŵ�������")]
	[SerializeField] private int maxLockedDoorMoveCount = 2;
	[HideInInspector] public bool isOnceOpen; // �����ס�����Ƿ��ǵ�һ�δ�
	private int lockedDoorMoveCount;

	[Header("�������Ҫ��Կ�״򿪣�Կ�׵Ĳ���")]
	[SerializeField] private GameObject keyObj;
	[SerializeField] private Transform keyInTrans;
	[SerializeField] private Transform keyOutTrans;
	[SerializeField] private Vector3 keyScale = new Vector3(0.15f, 0.15f, 0.15f);
	[SerializeField] private float keyMoveSpeed = 0.4f;
	[SerializeField] private float keyRotateSpeed = 5f;

	[Header("���������ź�Զ�Һܳ�ʱ��û�к��Ž������Ͱ��Ź���")]
	[SerializeField] private Transform playerCheckTrans;
	[SerializeField] private float playerCheckRadius = 6f;
	[SerializeField] private LayerMask playerLayer;
	[SerializeField] private float maxOpenTime = 3f;
	[HideInInspector] public bool playerIsNear;
	private float openTotalTime;

	[Header("�ŵ�ǰ������״̬")]
	public DoorState doorState = DoorState.Close;

	#endregion

	#region �����������ں���

	private void Awake()
	{
		if (keyID == string.Empty)
		{
			locked = false;
			isOnceOpen = false;
		}
		else
		{
			locked = true;
			isOnceOpen = true; // �����ʹ��Կ�׿���ǰ������һ�ν����� ����Ϊ true
		}
	}

	private void Update()
	{
		CheckDoorToBeClose();
		CheckPlayerNearby();
	}

	#endregion

	#region �ſ��ع���

	// ����Ÿ����Ƿ������
	private void CheckPlayerNearby()
	{
		if (Physics.CheckSphere(playerCheckTrans.position, playerCheckRadius, playerLayer))
		{
			playerIsNear = true;
		}
		else
		{
			playerIsNear = false;
		}
	}

	// ������̫��û���Ž������Ͱ��Ź���
	private void CheckDoorToBeClose()
	{
		if (!playerIsNear)
		{
			if (doorState == DoorState.Open)
			{
				openTotalTime = openTotalTime + Time.deltaTime;

				if (openTotalTime > maxOpenTime)
				{
					StartCoroutine(DoorRotate(0));
				}
			}
		}
		else // ������Ÿ������Ͱ�ʱ���������
		{
			openTotalTime = 0f;
		}
	}

	/// <summary>
	/// ����ҿ���ǰ�����û������Ҽ���
	/// </summary>
	/// <param name="toEnable"></param>
	public void SetCheckDirCollider(bool toEnable)
	{
		for (int i = 0; i < playerCheckDirCollider.Count; i ++)
		{
			playerCheckDirCollider[i].enabled = toEnable;
		}
	}

	/// <summary>
	/// �����Ų���������Ĳ�������Ϊ��������ŵ�ǰ�����棨���Ű��������Ϊ��׼��
	/// </summary>
	/// <param name="checkPlayerPos"></param>
	public void InteractiveWithDoor(float checkPlayerPos)
	{
		if (doorState == DoorState.Moving) return;

		if (locked)
		{
			if (haveSubsidiaryDoor)
			{
				subsidiaryDoorController.SubsidiaryDoorClosed(-checkPlayerPos);
			}

			doorSound.PlayDoorLockedSound();

			StartCoroutine(DoorLocked(checkPlayerPos));
			return;
		}

		if (isOnceOpen)
		{
			if (keyObj == null)
			{
				Debug.LogWarning("������û��ָ��Կ��ģ�ͣ�������Ϸ��Ƶ���ȷ��");
				return;
			}

			doorSound.PlayDoorUnlockedSound();

			keyObj.transform.localScale = keyScale;
			keyObj.transform.localPosition = keyOutTrans.localPosition;
			keyObj.SetActive(true);
			StartCoroutine(KeyGetInTheDoor());
		}
		else
		{
			StartCoroutine(DoorRotate(checkPlayerPos));
		}
	}

	private IEnumerator DoorRotate(float checkPlayerPos)
	{
		if (doorState == DoorState.Close) // ���ŵ����
		{
			doorState = DoorState.Moving;
            //����Portal
            GetComponent<OcclusionPortal>().open = true;

            doorSound.PlayDoorOpenSound();

			if (haveSubsidiaryDoor) // �и������ţ��ͺ͸�������һ�����
			{
				subsidiaryDoorController.SubsidiaryDoorPerformRotate(checkPlayerPos, true);
			}

			if (checkPlayerPos > 0) // ������ŵ�ǰ��
			{
				while (true)
				{
					if (Quaternion.Angle(doorObject.transform.localRotation, Quaternion.Euler(openInTrans)) < minRotateValue)
					{
						doorObject.transform.localRotation = Quaternion.Euler(openInTrans);
						doorState = DoorState.Open;
						yield break;
					}

					doorObject.transform.localRotation = Quaternion.Slerp(doorObject.transform.localRotation, Quaternion.Euler(openInTrans), doorMoveSpeed * Time.deltaTime);
					yield return null;
				}
			}
			else // ������ŵĺ�
			{
				while (true)
				{
					if (Quaternion.Angle(doorObject.transform.localRotation, Quaternion.Euler(openOutTrans)) < minRotateValue)
					{
						doorObject.transform.localRotation = Quaternion.Euler(openOutTrans);
						doorState = DoorState.Open;
						yield break;
					}

					doorObject.transform.localRotation = Quaternion.Slerp(doorObject.transform.localRotation, Quaternion.Euler(openOutTrans), doorMoveSpeed * Time.deltaTime);
					yield return null;
				}
			}
		}

		if (doorState == DoorState.Open) // ���ŵ����
		{
			doorState = DoorState.Moving;
			openTotalTime = 0f;

			if (haveSubsidiaryDoor) // �и������ţ��ͺ͸�������һ�����
			{
				subsidiaryDoorController.SubsidiaryDoorPerformRotate(checkPlayerPos, false);
			}

			while (true)
			{
				if (Quaternion.Angle(doorObject.transform.localRotation, Quaternion.Euler(closeTrans)) < minRotateValue)
				{
					doorObject.transform.localRotation = Quaternion.Euler(closeTrans);
					doorState = DoorState.Close;

					//����Portal
					GetComponent<OcclusionPortal>().open = false;

					doorSound.PlayDoorCloseSound();

					yield break;
				}

				doorObject.transform.localRotation = Quaternion.Slerp(doorObject.transform.localRotation, Quaternion.Euler(closeTrans), doorMoveSpeed * Time.deltaTime);
				yield return null;
			}
		}
	}

	/// <summary>
	/// �Ŵ�������û�򿪵�ʱ��Ҫ�����������
	/// </summary>
	/// <param name="checkPlayerPos"></param>
	/// <returns></returns>
	private IEnumerator DoorLocked(float checkPlayerPos)
	{
		doorState = DoorState.Moving;

		lockedDoorMoveCount++;

		if (checkPlayerPos > 0) // ������ŵ�ǰ��
		{
			while (true)
			{
				if (Quaternion.Angle(doorObject.transform.localRotation, Quaternion.Euler(lockInTrans)) < minRotateValue)
				{
					doorObject.transform.localRotation = Quaternion.Euler(lockInTrans);
					StartCoroutine(LockedDoorClose(checkPlayerPos));
					yield break;
				}

				doorObject.transform.localRotation = Quaternion.Slerp(doorObject.transform.localRotation, Quaternion.Euler(lockInTrans), doorLockedMoveSpeed * Time.deltaTime);
				yield return null;
			}
		}
		else
		{
			while (true)
			{
				if (Quaternion.Angle(doorObject.transform.localRotation, Quaternion.Euler(lockOutTrans)) < minRotateValue)
				{
					doorObject.transform.localRotation = Quaternion.Euler(lockOutTrans);
					StartCoroutine(LockedDoorClose(checkPlayerPos));
					yield break;
				}

				doorObject.transform.localRotation = Quaternion.Slerp(doorObject.transform.localRotation, Quaternion.Euler(lockOutTrans), doorLockedMoveSpeed * Time.deltaTime);
				yield return null;
			}
		}
	}

	/// <summary>
	/// ���ϵ��Ų����궯���󣬻ص��رյ�״̬
	/// </summary>
	/// <returns></returns>
	private IEnumerator LockedDoorClose(float checkPlayerPos)
	{
		while (true)
		{
			if (Quaternion.Angle(doorObject.transform.localRotation, Quaternion.Euler(closeTrans)) < minRotateValue)
			{
				doorObject.transform.localRotation = Quaternion.Euler(closeTrans);

				if (lockedDoorMoveCount >= maxLockedDoorMoveCount) // ���Ƽ�����...
				{
					lockedDoorMoveCount = 0;
					doorState = DoorState.Close;
					yield break;
				}
				else
				{
					StartCoroutine(DoorLocked(checkPlayerPos));
					yield break;
				}
			}

			doorObject.transform.localRotation = Quaternion.Slerp(doorObject.transform.localRotation, Quaternion.Euler(closeTrans), doorLockedMoveSpeed * Time.deltaTime);
			yield return null;
		}
	}

	// Կ�ײ���ŵĶ���
	private IEnumerator KeyGetInTheDoor()
	{
		doorState = DoorState.Moving;

		while (true)
		{
			if (Vector3.Distance(keyObj.transform.localPosition, keyInTrans.localPosition) < 0.02f)
			{
				StartCoroutine(KeyRotateInTheDoor());
				keyObj.transform.localPosition = keyInTrans.localPosition;
				yield break;
			}

			keyObj.transform.localPosition = Vector3.MoveTowards(keyObj.transform.localPosition, keyInTrans.localPosition, keyMoveSpeed * Time.deltaTime);
			yield return null;
		}
	}

	// Կ����ת�Ķ���
	private IEnumerator KeyRotateInTheDoor()
	{
		while (true)
		{
			if (Quaternion.Angle(keyObj.transform.localRotation, keyInTrans.localRotation) < 1f)
			{
				keyObj.transform.localRotation = keyInTrans.localRotation;
				// keyObj.SetActive(false);
				doorState = DoorState.Close;
				yield break;
			}

			keyObj.transform.localRotation = Quaternion.Slerp(keyObj.transform.localRotation, keyInTrans.localRotation, keyRotateSpeed * Time.deltaTime);
			yield return null;
		}
	}

	#endregion

	#region DEBUG �ú���

	private void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.yellow;
		Gizmos.DrawWireSphere(playerCheckTrans.position, playerCheckRadius);
	}

	#endregion
}
