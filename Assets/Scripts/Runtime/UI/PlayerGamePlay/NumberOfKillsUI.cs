using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// 剩余杀敌数控制器
/// </summary>
public class NumberOfKillsUI : MonoBehaviour
{
	#region 基本组件和变量

	[Header("剩余杀敌数文字 UI")]
	[SerializeField] private TextMeshProUGUI numberOfKillUI;

	#endregion

	#region 基本生命周期函数

	private void Start()
	{
		GameProgressManager.Instance.eventHandler_GameManager.OnKillEnemyEvent += DisplayNumberOfKill;

		DisplayNumberOfKill(); // 先直接显示一遍
	}

	private void OnDestroy()
	{
		try
		{
			GameProgressManager.Instance.eventHandler_GameManager.OnKillEnemyEvent -= DisplayNumberOfKill;
		}
		catch
		{
			Debug.Log("游戏已经结束，无需取消事件订阅");
		}
	}

	#endregion

	#region 剩余杀敌数信息显示

	public void DisplayNumberOfKill()
	{
		numberOfKillUI.text = GameProgressManager.Instance.ResidueKillCount.ToString();
	}

	#endregion
}
