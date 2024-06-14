using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 用动画曲线控制的开关家具交互

/// <summary>
/// 记录交互家具的类型（是开关门还是推拉抽屉）
/// </summary>
public enum FurnitureType
{
	RotateDoor,
	PushPull
}

/// <summary>
/// 记录家具当前的状态
/// </summary>
public enum FurnitureState
{
	Close,
	Open,
	Moving
}

/// <summary>
/// 家具交互控制器，主要是家具门，抽屉开关等
/// </summary>
public class FurnitureInterController : MonoBehaviour
{
	#region 基本组件和变量

	[Header("该家具门是否有附属门")]
	[SerializeField] private bool haveSubsidiaryFurnObj;
	[Header("家具附属门控制器")]
	[SerializeField] private SubsidiaryFurnObj subsidiaryFurnObj;
	[Header("家具音效控制器")]
	[SerializeField] private InterFurnitureSound interFurnitureSound;

	[Header("动画曲线")]
	[SerializeField] private AnimationCurve interCurve;
	[Header("动画曲线的定义域")]
	[SerializeField] private float interCurveDef;
	private float moveTime; // 动画曲线当前的移动时间

	[Header("交互家具的类型")]
	public FurnitureType furnitureType;
	public FurnitureState furnitureState; // 交互家具目前所处状态

	[Header("需要移动的交互物体")]
	[SerializeField] private Transform interDoorTrans;

	[Header("交互门 Collider 列表")]
	[Tooltip("开关家具门的时候需要关闭门的碰撞，否则家具会推着玩家走")]
	[SerializeField] private List<Collider> furnDoorCollider = new List<Collider>();

	// 协程
	private Coroutine rotateFurnitureDoor_IECor;
	private Coroutine moveFurnitureDoor_IECor;

	#endregion

	#region 家具交互开关功能

	/// <summary>
	/// 玩家与家具交互的主要功能
	/// </summary>
	public void InteractiveWithFurniture()
	{
		if (furnitureType == FurnitureType.RotateDoor)
		{
			// Debug.Log("开始旋转家具门");

			if (haveSubsidiaryFurnObj)
			{
				subsidiaryFurnObj.RotateFurnitureDoor_Subsidiary(interCurve, furnitureState, interCurveDef);
			}

			rotateFurnitureDoor_IECor = StartCoroutine(RotateFurnitureDoor_IE());
		}

		if (furnitureType == FurnitureType.PushPull)
		{
			if (haveSubsidiaryFurnObj)
			{
				subsidiaryFurnObj.MoveFurnitureDoor_Subsidiary(interCurve, furnitureState, interCurveDef);
			}

			moveFurnitureDoor_IECor = StartCoroutine(MoveFurnitureDoor_IE());
		}
	}

	// 移动交互家具门（可以是抽屉一类）
	private IEnumerator MoveFurnitureDoor_IE()
	{
		if (furnitureState == FurnitureState.Close)
		{
			interFurnitureSound.PlayFurnSound(furnitureType, furnitureState); // 播放音效
			furnitureState = FurnitureState.Moving;
			moveTime = 0f;

			// 关闭家具门的碰撞
			for (int i = 0; i < furnDoorCollider.Count; i++)
			{
				furnDoorCollider[i].isTrigger = true;
			}

			while (true)
			{
				moveTime += Time.deltaTime;

				interDoorTrans.localPosition = new Vector3()
				{
					x = interCurve.Evaluate(moveTime),
					y = interDoorTrans.localPosition.y,
					z = interDoorTrans.localPosition.z
				};

				if (moveTime >= interCurveDef)
				{
					moveTime = interCurveDef;
					furnitureState = FurnitureState.Open;
					yield break;
				}

				yield return null;
			}
		}

		if (furnitureState == FurnitureState.Open)
		{
			interFurnitureSound.PlayFurnSound(furnitureType, furnitureState); // 播放音效
			furnitureState = FurnitureState.Moving;
			moveTime = interCurveDef;

			while (true)
			{
				moveTime -= Time.deltaTime;

				interDoorTrans.localPosition = new Vector3()
				{
					x = interCurve.Evaluate(moveTime),
					y = interDoorTrans.localPosition.y,
					z = interDoorTrans.localPosition.z
				};

				if (moveTime <= 0f)
				{
					moveTime = 0f;
					furnitureState = FurnitureState.Close;

					// 重新启动家具门的碰撞
					for (int i = 0; i < furnDoorCollider.Count; i++)
					{
						furnDoorCollider[i].isTrigger = false;
					}

					yield break;
				}

				yield return null;
			}
		}
	}

	// 旋转交互家具门
	private IEnumerator RotateFurnitureDoor_IE()
	{
		if (furnitureState == FurnitureState.Close)
		{
			interFurnitureSound.PlayFurnSound(furnitureType, furnitureState); // 播放音效
			furnitureState = FurnitureState.Moving;
			moveTime = 0f;

			// 关闭家具门的碰撞
			for (int i = 0; i < furnDoorCollider.Count; i ++)
			{
				furnDoorCollider[i].isTrigger = true;
			}

			while (true)
			{
				moveTime += Time.deltaTime;

				interDoorTrans.localEulerAngles = new Vector3()
				{
					x = interDoorTrans.localEulerAngles.x,
					y = interCurve.Evaluate(moveTime),
					z = interDoorTrans.localEulerAngles.z
				};

				if (moveTime >= interCurveDef)
				{
					moveTime = interCurveDef;
					furnitureState = FurnitureState.Open;
					yield break;
				}

				yield return null;
			}
		}
		
		if (furnitureState == FurnitureState.Open)
		{
			furnitureState = FurnitureState.Moving;
			moveTime = interCurveDef;

			while (true)
			{
				moveTime -= Time.deltaTime;

				interDoorTrans.localEulerAngles = new Vector3()
				{
					x = interDoorTrans.localEulerAngles.x,
					y = interCurve.Evaluate(moveTime),
					z = interDoorTrans.localEulerAngles.z
				};

				if (moveTime <= 0f)
				{
					moveTime = 0f;
					furnitureState = FurnitureState.Close;

					interFurnitureSound.PlayFurnSound(furnitureType, FurnitureState.Open); // 播放音效

					// 重新启动家具门的碰撞
					for (int i = 0; i < furnDoorCollider.Count; i++)
					{
						furnDoorCollider[i].isTrigger = false;
					}

					yield break;
				}

				yield return null;
			}
		}
	}

	#endregion
}
