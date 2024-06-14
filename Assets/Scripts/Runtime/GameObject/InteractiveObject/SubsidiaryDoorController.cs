using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 主门附属的门控制器（用于实现双开门）
/// </summary>
public class SubsidiaryDoorController : MonoBehaviour
{
	#region 基本组件和变量

	[Header("门开关的位置信息")]
	[Tooltip("放置方向有变时，应该动态修改这里的数值")]
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
	[SerializeField] private float doorMoveSpeed = 5f;
	[Header("门锁上的时候，动画播放的速度")]
	[SerializeField] private float doorLockedMoveSpeed = 18f;
	[Header("门锁上的时候，动画播放的最大次数")]
	[SerializeField] private int maxLockedDoorMoveCount = 2;
	[HideInInspector] public bool isOnceOpen; // 这个锁住的门是否是第一次打开
	private int lockedDoorMoveCount;

	#endregion

	#region 附属门的开关功能

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
		if (toOpen) // 需要开门
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
		else // 需要关门
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

		if (checkPlayerPos > 0) // 检查玩家位置
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

				if (lockedDoorMoveCount >= maxLockedDoorMoveCount) // 多推几下门...
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
