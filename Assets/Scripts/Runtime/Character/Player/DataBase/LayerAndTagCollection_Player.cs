using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class LayerAndTagCollection_Player
{
	#region TAG 集合

	[Header("地面材质")]
	public string concrete = "Concrete";
	public string concreteStaircase = "ConcreteStaircase";
	public string brick = "Brick";

	[Header("环境物体")]
	public string doorTag = "Door";
	public string itemTag = "Item";
	public string furnitureTag = "Furniture";
	public string envirPhysicsObj = "EnvirPhysicsObj";
	public string bulletShell = "BulletShell";

	[Header("可拾取物品 ID")]
	public string pistolAmmoItemID = "Pistol_Ammo"; // 手枪弹药
	public string medicineItemID = "Medicine";

	[Header("武器命中标签")]
	[Header("环境物体")]
	[Header("泥土")]
	public string envirDirt = "EnvirDirt";
	[Header("木头")]
	public string envirWood = "EnvirWood";
	[Header("玻璃")]
	public string envirGlass = "EnvirGlass";

	[Header("一般僵尸")]
	[Header("头部")]
	public string zombieHeadTag = "Zombie_Head";
	[Header("躯干")]
	public string zombieBodyTag = "Zombie_Body";
	[Header("四肢")]
	public string zombieFourLimbsTag = "Zombie_FourLimbs";

	[Header("敌人通用")]
	public string enemyTag = "Enemy";

	#endregion

	#region Layer 编号集合（int 类型）

	[Header("地面检测的 LayerIndex")]
	public int groundLayerIndex = 3;

	[Header("玩家武器应该忽略的 LayerIndex")]
	public int playerLayerIndex = 7;
	public int airWallLayerIndex = 10;
	public int doorInteractiveLayerIndex = 11;
	public int checkDoorDirLayerIndex = 8;
	public int enemyLayerIndex = 9;

	#endregion
}
