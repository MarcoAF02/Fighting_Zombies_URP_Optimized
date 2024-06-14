using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 以门 X 轴正方向面对玩家为基准，向前开门为 Open_IN ，向后开门为 Open_Out
// 钥匙开门动画只会出现在门的正面

// 主门控制器挂载在门的根物体上，附属门控制器挂载在附属门游戏物体上

/// <summary>
/// 枚举类型：判断当前门的状态
/// </summary>
public enum DoorState
{
	Moving,
	Open,
	Close
}

/// <summary>
/// 主门控制器
/// </summary>
public class MainDoorController : MonoBehaviour
{
	#region 基本组件和变量

	[Header("该门需要通知给玩家的信息")]
	[Header("门锁住了")]
	public string tipLockedMessage = "锁住了";

	[Header("提示新游戏目标")]
	public bool canNoticePlayer;
	public GameTargetTipState gameTargetTipState;

	[Header("门游戏物体")]
	[SerializeField] private GameObject doorObject;
	[Header("该门是否有附属门")]
	[SerializeField] private bool haveSubsidiaryDoor;
	[Header("附属门控制器")]
	[SerializeField] private SubsidiaryDoorController subsidiaryDoorController;

	[Header("门音效控制器")]
	[SerializeField] private DoorSound doorSound;

	[Header("如果该门需要钥匙开锁，这个 ID 记录了钥匙的类型")]
	public string keyID;
	[HideInInspector] public bool locked;

	[Header("玩家方位检测点集合")]
	[Tooltip("玩家开门时检测玩家在门前面还是后面")]
	[SerializeField] private List<Collider> playerCheckDirCollider = new List<Collider>();

	[Header("门开关的位置信息")]
	[SerializeField] private Vector3 openInTrans = new Vector3(0, 90, 0);
	[SerializeField] private Vector3 openOutTrans = new Vector3(0, -90, 0);
	[SerializeField] private Vector3 closeTrans = new Vector3(0, 0, 0);

	[Header("当门锁住的时候，播放动画需要用的位置信息")]
	[SerializeField] private Vector3 lockInTrans = new Vector3(0, 2, 0);
	[SerializeField] private Vector3 lockOutTrans = new Vector3(0, -2, 0);

	[Header("门旋转时的最小角度修正")]
	[Tooltip("当门插值旋转十分接近目标时，如果与目标的差距小于这个值则强行给门的旋转信息赋值。可以决定单位时间内玩家和门的交互频率")]
	[SerializeField] private float minRotateValue = 0.5f;

	[Header("开关门的速度")]
	[SerializeField] private float doorMoveSpeed = 4f;
	[Header("门锁上的时候，动画播放的速度")]
	[SerializeField] private float doorLockedMoveSpeed = 18f;
	[Header("门锁上的时候，动画播放的最大次数")]
	[SerializeField] private int maxLockedDoorMoveCount = 2;
	[HideInInspector] public bool isOnceOpen; // 这个锁住的门是否是第一次打开
	private int lockedDoorMoveCount;

	[Header("如果门需要用钥匙打开，钥匙的参数")]
	[SerializeField] private GameObject keyObj;
	[SerializeField] private Transform keyInTrans;
	[SerializeField] private Transform keyOutTrans;
	[SerializeField] private Vector3 keyScale = new Vector3(0.15f, 0.15f, 0.15f);
	[SerializeField] private float keyMoveSpeed = 0.4f;
	[SerializeField] private float keyRotateSpeed = 5f;

	[Header("如果玩家离门很远且很长时间没有和门交互，就把门关上")]
	[SerializeField] private Transform playerCheckTrans;
	[SerializeField] private float playerCheckRadius = 6f;
	[SerializeField] private LayerMask playerLayer;
	[SerializeField] private float maxOpenTime = 3f;
	[HideInInspector] public bool playerIsNear;
	private float openTotalTime;

	[Header("门当前所处的状态")]
	public DoorState doorState = DoorState.Close;

	#endregion

	#region 基本生命周期函数

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
			isOnceOpen = true; // 玩家在使用钥匙开门前，“第一次交互” 设置为 true
		}
	}

	private void Update()
	{
		CheckDoorToBeClose();
		CheckPlayerNearby();
	}

	#endregion

	#region 门开关功能

	// 检查门附近是否有玩家
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

	// 如果玩家太久没和门交互，就把门关上
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
		else // 玩家在门附近，就把时间计数归零
		{
			openTotalTime = 0f;
		}
	}

	/// <summary>
	/// 在玩家开门前后启用或禁用玩家检测点
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
	/// 开关门操作；传入的参数含义为：玩家在门的前面或后面（以门把手在左边为基准）
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
				Debug.LogWarning("这扇门没有指定钥匙模型，请检查游戏设计的正确性");
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
		if (doorState == DoorState.Close) // 开门的情况
		{
			doorState = DoorState.Moving;
            //处理Portal
            GetComponent<OcclusionPortal>().open = true;

            doorSound.PlayDoorOpenSound();

			if (haveSubsidiaryDoor) // 有附属的门，就和附属的门一起操作
			{
				subsidiaryDoorController.SubsidiaryDoorPerformRotate(checkPlayerPos, true);
			}

			if (checkPlayerPos > 0) // 玩家在门的前方
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
			else // 玩家在门的后方
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

		if (doorState == DoorState.Open) // 关门的情况
		{
			doorState = DoorState.Moving;
			openTotalTime = 0f;

			if (haveSubsidiaryDoor) // 有附属的门，就和附属的门一起操作
			{
				subsidiaryDoorController.SubsidiaryDoorPerformRotate(checkPlayerPos, false);
			}

			while (true)
			{
				if (Quaternion.Angle(doorObject.transform.localRotation, Quaternion.Euler(closeTrans)) < minRotateValue)
				{
					doorObject.transform.localRotation = Quaternion.Euler(closeTrans);
					doorState = DoorState.Close;

					//处理Portal
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
	/// 门带锁并且没打开的时候，要播放这个动画
	/// </summary>
	/// <param name="checkPlayerPos"></param>
	/// <returns></returns>
	private IEnumerator DoorLocked(float checkPlayerPos)
	{
		doorState = DoorState.Moving;

		lockedDoorMoveCount++;

		if (checkPlayerPos > 0) // 玩家在门的前方
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
	/// 锁上的门播放完动画后，回到关闭的状态
	/// </summary>
	/// <returns></returns>
	private IEnumerator LockedDoorClose(float checkPlayerPos)
	{
		while (true)
		{
			if (Quaternion.Angle(doorObject.transform.localRotation, Quaternion.Euler(closeTrans)) < minRotateValue)
			{
				doorObject.transform.localRotation = Quaternion.Euler(closeTrans);

				if (lockedDoorMoveCount >= maxLockedDoorMoveCount) // 多推几下门...
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

	// 钥匙插进门的动画
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

	// 钥匙旋转的动画
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

	#region DEBUG 用函数

	private void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.yellow;
		Gizmos.DrawWireSphere(playerCheckTrans.position, playerCheckRadius);
	}

	#endregion
}
