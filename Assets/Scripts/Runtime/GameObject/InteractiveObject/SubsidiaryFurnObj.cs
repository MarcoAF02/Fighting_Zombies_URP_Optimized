using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 是动画曲线控制附属家具门

/// <summary>
/// 附属的家具控制器，被 FurnitureInterController 类所控制
/// </summary>
public class SubsidiaryFurnObj : MonoBehaviour
{
	#region 基本组件和变量

	// 动画曲线，定义域，家具类型等由主控制器传入

	[Header("需要移动的交互物体")]
	[SerializeField] private Transform interDoorTrans;

	private float moveTime; // 门移动的时间（和主控制器一样的逻辑）

	// 协程
	private Coroutine rotateFurnitureDoor_Subsidiary_IECor;
	private Coroutine moveFurnitureDoor_Subsidiary_IECor;

	#endregion

	#region 家具交互功能

	/// <summary>
	/// 旋转交互家具附属门
	/// </summary>
	/// <param name="_mainCurve"></param>
	/// <param name="_furnitureType"></param>
	/// <param name="_furnitureState"></param>
	/// <param name="_interCurveDef"></param>
	public void RotateFurnitureDoor_Subsidiary(AnimationCurve _mainCurve, FurnitureState _furnitureState, float _interCurveDef)
	{
		rotateFurnitureDoor_Subsidiary_IECor = StartCoroutine(RotateFurnitureDoor_Subsidiary_IE(_mainCurve, _furnitureState, _interCurveDef));
	}

	/// <summary>
	/// 移动交互家具门（抽屉一类）
	/// </summary>
	/// <param name="_mainCurve"></param>
	/// <param name="_furnitureState"></param>
	/// <param name="_interCurveDef"></param>
	public void MoveFurnitureDoor_Subsidiary(AnimationCurve _mainCurve, FurnitureState _furnitureState, float _interCurveDef)
	{
		moveFurnitureDoor_Subsidiary_IECor = StartCoroutine(MoveFurnitureDoor_Subsidiary_IE(_mainCurve, _furnitureState, _interCurveDef));
	}

	private IEnumerator RotateFurnitureDoor_Subsidiary_IE(AnimationCurve _mainCurve, FurnitureState _furnitureState, float _interCurveDef)
	{
		if (_furnitureState == FurnitureState.Close)
		{
			moveTime = 0f;

			while (true)
			{
				moveTime += Time.deltaTime;

				interDoorTrans.localEulerAngles = new Vector3()
				{
					x = interDoorTrans.localEulerAngles.x,
					y = -_mainCurve.Evaluate(moveTime),
					z = interDoorTrans.localEulerAngles.z
				};

				if (moveTime >= _interCurveDef)
				{
					moveTime = _interCurveDef;
					yield break;
				}

				yield return null;
			}
		}

		if (_furnitureState == FurnitureState.Open)
		{
			moveTime = _interCurveDef;

			while (true)
			{
				moveTime -= Time.deltaTime;

				interDoorTrans.localEulerAngles = new Vector3()
				{
					x = interDoorTrans.localEulerAngles.x,
					y = -_mainCurve.Evaluate(moveTime),
					z = interDoorTrans.localEulerAngles.z
				};

				if (moveTime <= 0f)
				{
					moveTime = 0f;
					yield break;
				}

				yield return null;
			}
		}
	}

	private IEnumerator MoveFurnitureDoor_Subsidiary_IE(AnimationCurve _mainCurve, FurnitureState _furnitureState, float _interCurveDef)
	{
		if (_furnitureState == FurnitureState.Close)
		{
			moveTime = 0f;

			while (true)
			{
				moveTime += Time.deltaTime;

				interDoorTrans.localPosition = new Vector3()
				{
					x = _mainCurve.Evaluate(moveTime),
					y = interDoorTrans.localPosition.y,
					z = interDoorTrans.localPosition.z
				};

				if (moveTime >= _interCurveDef)
				{
					moveTime = _interCurveDef;
					yield break;
				}

				yield return null;
			}
		}

		if (_furnitureState == FurnitureState.Open)
		{
			moveTime = _interCurveDef;

			while (true)
			{
				moveTime -= Time.deltaTime;

				interDoorTrans.localPosition = new Vector3()
				{
					x = _mainCurve.Evaluate(moveTime),
					y = interDoorTrans.localPosition.y,
					z = interDoorTrans.localPosition.z
				};

				if (moveTime <= 0f)
				{
					moveTime = 0f;
					yield break;
				}

				yield return null;
			}
		}
	}

	#endregion
}
