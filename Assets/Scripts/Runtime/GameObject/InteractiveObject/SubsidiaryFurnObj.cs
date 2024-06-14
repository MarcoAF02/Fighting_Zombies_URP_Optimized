using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �Ƕ������߿��Ƹ����Ҿ���

/// <summary>
/// �����ļҾ߿��������� FurnitureInterController ��������
/// </summary>
public class SubsidiaryFurnObj : MonoBehaviour
{
	#region ��������ͱ���

	// �������ߣ������򣬼Ҿ����͵���������������

	[Header("��Ҫ�ƶ��Ľ�������")]
	[SerializeField] private Transform interDoorTrans;

	private float moveTime; // ���ƶ���ʱ�䣨����������һ�����߼���

	// Э��
	private Coroutine rotateFurnitureDoor_Subsidiary_IECor;
	private Coroutine moveFurnitureDoor_Subsidiary_IECor;

	#endregion

	#region �Ҿ߽�������

	/// <summary>
	/// ��ת�����Ҿ߸�����
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
	/// �ƶ������Ҿ��ţ�����һ�ࣩ
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
