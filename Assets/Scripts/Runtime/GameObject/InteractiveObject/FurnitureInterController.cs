using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �ö������߿��ƵĿ��ؼҾ߽���

/// <summary>
/// ��¼�����Ҿߵ����ͣ��ǿ����Ż����������룩
/// </summary>
public enum FurnitureType
{
	RotateDoor,
	PushPull
}

/// <summary>
/// ��¼�Ҿߵ�ǰ��״̬
/// </summary>
public enum FurnitureState
{
	Close,
	Open,
	Moving
}

/// <summary>
/// �Ҿ߽�������������Ҫ�ǼҾ��ţ����뿪�ص�
/// </summary>
public class FurnitureInterController : MonoBehaviour
{
	#region ��������ͱ���

	[Header("�üҾ����Ƿ��и�����")]
	[SerializeField] private bool haveSubsidiaryFurnObj;
	[Header("�Ҿ߸����ſ�����")]
	[SerializeField] private SubsidiaryFurnObj subsidiaryFurnObj;
	[Header("�Ҿ���Ч������")]
	[SerializeField] private InterFurnitureSound interFurnitureSound;

	[Header("��������")]
	[SerializeField] private AnimationCurve interCurve;
	[Header("�������ߵĶ�����")]
	[SerializeField] private float interCurveDef;
	private float moveTime; // �������ߵ�ǰ���ƶ�ʱ��

	[Header("�����Ҿߵ�����")]
	public FurnitureType furnitureType;
	public FurnitureState furnitureState; // �����Ҿ�Ŀǰ����״̬

	[Header("��Ҫ�ƶ��Ľ�������")]
	[SerializeField] private Transform interDoorTrans;

	[Header("������ Collider �б�")]
	[Tooltip("���ؼҾ��ŵ�ʱ����Ҫ�ر��ŵ���ײ������Ҿ߻����������")]
	[SerializeField] private List<Collider> furnDoorCollider = new List<Collider>();

	// Э��
	private Coroutine rotateFurnitureDoor_IECor;
	private Coroutine moveFurnitureDoor_IECor;

	#endregion

	#region �Ҿ߽������ع���

	/// <summary>
	/// �����Ҿ߽�������Ҫ����
	/// </summary>
	public void InteractiveWithFurniture()
	{
		if (furnitureType == FurnitureType.RotateDoor)
		{
			// Debug.Log("��ʼ��ת�Ҿ���");

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

	// �ƶ������Ҿ��ţ������ǳ���һ�ࣩ
	private IEnumerator MoveFurnitureDoor_IE()
	{
		if (furnitureState == FurnitureState.Close)
		{
			interFurnitureSound.PlayFurnSound(furnitureType, furnitureState); // ������Ч
			furnitureState = FurnitureState.Moving;
			moveTime = 0f;

			// �رռҾ��ŵ���ײ
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
			interFurnitureSound.PlayFurnSound(furnitureType, furnitureState); // ������Ч
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

					// ���������Ҿ��ŵ���ײ
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

	// ��ת�����Ҿ���
	private IEnumerator RotateFurnitureDoor_IE()
	{
		if (furnitureState == FurnitureState.Close)
		{
			interFurnitureSound.PlayFurnSound(furnitureType, furnitureState); // ������Ч
			furnitureState = FurnitureState.Moving;
			moveTime = 0f;

			// �رռҾ��ŵ���ײ
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

					interFurnitureSound.PlayFurnSound(furnitureType, FurnitureState.Open); // ������Ч

					// ���������Ҿ��ŵ���ײ
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
