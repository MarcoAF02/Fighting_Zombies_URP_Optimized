using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class ChangeButtonTextColor : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
	#region 基本组件和变量

	[Header("需要改变颜色的字")]
	[SerializeField] private TextMeshProUGUI buttonText;

	[Header("鼠标悬浮时文字的颜色")]
	[SerializeField] private Color mouseEnterColor;
	[Header("鼠标退出时文字的颜色")]
	[SerializeField] private Color mouseExitColor;

	#endregion

	#region 基本生命周期函数

	private void OnEnable()
	{
		buttonText.color = mouseExitColor;
	}

	#endregion

	#region 事件系统函数实现

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
