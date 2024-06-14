using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class PlayerTutorialKeyword
{
	[Header("玩家教程信息的关键词定义")]
	[Header("游戏刚开始，教玩家如何移动")]
	public string firstMove = "FirstMove";
	[Header("玩家学会移动，教玩家如何奔跑和下蹲")]
	public string firstRunAndCrouch = "FirstRunAndCrouch";
	[Header("教玩家使用快捷物品栏")]
	public string firstUseEquipItem = "FirstUseEquipItem";
	[Header("教玩家枪械的使用方法")]
	public string firstUseGun = "FirstUseGun";
	[Header("教玩家使用医疗物品")]
	public string firstUseMedicine = "FirstUseMedicine";
	[Header("教玩家按下 Tab 检查游戏目标（打开物品栏）")]
	public string firstUseInventory = "FirstUseInventory";
}
