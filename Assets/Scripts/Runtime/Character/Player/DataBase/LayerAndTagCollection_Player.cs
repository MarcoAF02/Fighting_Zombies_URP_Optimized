using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class LayerAndTagCollection_Player
{
	#region TAG ����

	[Header("�������")]
	public string concrete = "Concrete";
	public string concreteStaircase = "ConcreteStaircase";
	public string brick = "Brick";

	[Header("��������")]
	public string doorTag = "Door";
	public string itemTag = "Item";
	public string furnitureTag = "Furniture";
	public string envirPhysicsObj = "EnvirPhysicsObj";
	public string bulletShell = "BulletShell";

	[Header("��ʰȡ��Ʒ ID")]
	public string pistolAmmoItemID = "Pistol_Ammo"; // ��ǹ��ҩ
	public string medicineItemID = "Medicine";

	[Header("�������б�ǩ")]
	[Header("��������")]
	[Header("����")]
	public string envirDirt = "EnvirDirt";
	[Header("ľͷ")]
	public string envirWood = "EnvirWood";
	[Header("����")]
	public string envirGlass = "EnvirGlass";

	[Header("һ�㽩ʬ")]
	[Header("ͷ��")]
	public string zombieHeadTag = "Zombie_Head";
	[Header("����")]
	public string zombieBodyTag = "Zombie_Body";
	[Header("��֫")]
	public string zombieFourLimbsTag = "Zombie_FourLimbs";

	[Header("����ͨ��")]
	public string enemyTag = "Enemy";

	#endregion

	#region Layer ��ż��ϣ�int ���ͣ�

	[Header("������� LayerIndex")]
	public int groundLayerIndex = 3;

	[Header("�������Ӧ�ú��Ե� LayerIndex")]
	public int playerLayerIndex = 7;
	public int airWallLayerIndex = 10;
	public int doorInteractiveLayerIndex = 11;
	public int checkDoorDirLayerIndex = 8;
	public int enemyLayerIndex = 9;

	#endregion
}
