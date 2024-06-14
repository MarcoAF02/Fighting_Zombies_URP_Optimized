using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 游戏通关管理器
/// </summary>
public class GameCompleteChecker : MonoBehaviour
{
	#region 基本组件和变量

	// 检查方式：横着画一条射线在玩家的目标地点上，玩家抵达时检查玩家杀敌数是否到位

	[Header("射线的长度")]
	[SerializeField] private float checkRayDistance;
	[Header("玩家的 LayerIndex")]
	[SerializeField] private int playerLayerIndex;

	#endregion

	#region 基本生命周期函数

	private void Update()
	{
		CheckPlayerComplete();
	}

	#endregion

	#region 玩家通关检测

	private void CheckPlayerComplete()
	{
		if (GameProgressManager.Instance.CurrentGameProgress == GameProgress.GameComplete) return;

		if (Physics.Raycast(transform.position, transform.forward, checkRayDistance, 1 << playerLayerIndex))
		{
			if (GameProgressManager.Instance.isKillComplete)
			{
				GameProgressManager.Instance.playerController.SwitchState(GameProgressManager.Instance.playerController.playerCompleteState);
				Debug.Log("游戏通关");
			}
			else
			{
				Debug.Log("杀敌数未达标，还没有通关");
			}
		}
	}

	#endregion

	#region DEBUG 用函数

	private void OnDrawGizmosSelected()
	{
		Debug.DrawRay(transform.position, transform.forward * checkRayDistance, Color.green);
	}

	#endregion
}
