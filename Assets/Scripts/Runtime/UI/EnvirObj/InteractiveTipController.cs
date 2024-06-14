using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractiveTipController : MonoBehaviour
{
	#region ��������ͱ���

	[Header("��ʾͼ�������")]
	[SerializeField] private Transform interactiveTipObj;
	[Header("������߼���")]
	[SerializeField] private Transform checkPointTrans;
	[Header("������߼�ⷶΧ")]
	[Tooltip("��Ҿ�����ʱ��ʾ������ʾ")]
	[SerializeField] private float checkRadius;
	[Header("��ҵ� Layer")]
	[SerializeField] private int playerLayerIndex;
	[Header("��Ҿ�����ʱ���ؽ�����ʾ")]
	[SerializeField] private float hideDistance;
	[Header("������ʱ̧����ֵ")]
	[SerializeField] private float lookupValue;

	#endregion

	#region �����������ں���

	private void Update()
	{
		ShowInteractiveTip();

		// �޸��쳣��ת
		interactiveTipObj.localEulerAngles = new Vector3(0f, interactiveTipObj.localEulerAngles.y, 0f);

		// TODO: ���¼��ַ�ʽ�����Կ�����ת������������ʹ����ά��������ŷ���ǣ�����ֱ���޸���Ԫ����
		// transform.localEulerAngles = new Vector3(x, y, z);
		// transform.eulerAngles = new Vector3(x, y, z);

		// transform.localRotation = Quaternion.Eular(x, y, z);
		// transform.rotation = Quaternion.Eular(x, y, z);
	}

	#endregion

	#region ��ʾ�����ؽ�����ʾ

	private void ShowInteractiveTip()
	{
		Collider[] playerArray = Physics.OverlapSphere(checkPointTrans.position, checkRadius, 1 << playerLayerIndex);
		
		if (playerArray.Length == 0)
		{
			interactiveTipObj.gameObject.SetActive(false);
		}

		if (playerArray.Length == 1)
		{
			if (Vector3.Distance(playerArray[0].transform.position, interactiveTipObj.position) < hideDistance + lookupValue)
			{
				interactiveTipObj.gameObject.SetActive(false);
			}
			else
			{
				interactiveTipObj.gameObject.SetActive(true);
			}

			Vector3 lookDir = (playerArray[0].transform.position - interactiveTipObj.position).normalized + new Vector3(0, lookupValue, 0);
			interactiveTipObj.rotation = Quaternion.LookRotation(lookDir);
		}

		if (playerArray.Length > 1)
		{
			Debug.LogWarning("��ǰ�ǵ�����Ϸ����Ӧ���ж������");
		}
	}

	#endregion

}
