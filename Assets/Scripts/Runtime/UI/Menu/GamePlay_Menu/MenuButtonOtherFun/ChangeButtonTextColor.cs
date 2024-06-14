using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class ChangeButtonTextColor : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
	#region ��������ͱ���

	[Header("��Ҫ�ı���ɫ����")]
	[SerializeField] private TextMeshProUGUI buttonText;

	[Header("�������ʱ���ֵ���ɫ")]
	[SerializeField] private Color mouseEnterColor;
	[Header("����˳�ʱ���ֵ���ɫ")]
	[SerializeField] private Color mouseExitColor;

	#endregion

	#region �����������ں���

	private void OnEnable()
	{
		buttonText.color = mouseExitColor;
	}

	#endregion

	#region �¼�ϵͳ����ʵ��

	public void OnPointerEnter(PointerEventData eventData)
	{
		buttonText.color = mouseEnterColor;
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		buttonText.color = mouseExitColor;
	}

	#endregion

}
