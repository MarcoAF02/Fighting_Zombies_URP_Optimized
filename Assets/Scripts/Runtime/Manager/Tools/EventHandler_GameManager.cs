using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 游戏进度改变的事件发布器
/// </summary>
public class EventHandler_GameManager : MonoBehaviour
{
	#region 游戏进度推进时所发生的事件

	// 游戏进程推进事件
	public delegate void ChangeGameProgress(GameProgress gameProgress); // 委托
	public event ChangeGameProgress ChangeGameProgressEvent; // 基于上面的委托定义事件

	/// <summary>
	/// 响应游戏进度改变事件的函数
	/// </summary>
	/// <param name="gameProgress"></param>
	public void InvokeChangeGameProgress(GameProgress gameProgress)
	{
		ChangeGameProgressEvent(gameProgress);
	}

	#endregion

	#region 玩家每击杀任意一名敌人发生的事件

	public delegate void KillEnemy();

	public event KillEnemy OnKillEnemyEvent;

	/// <summary>
	/// 响应玩家击杀敌人的事件
	/// </summary>
	public void InvokeKillEnemyEvent()
	{
		OnKillEnemyEvent();
	}

	#endregion

}
