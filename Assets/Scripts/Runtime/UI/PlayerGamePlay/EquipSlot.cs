using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ������װ���������ϣ����ڹ���װ�������ӣ��������ӣ�
/// </summary>
public class EquipSlot : MonoBehaviour
{
	#region ��������ͱ���

	[Header("�ø�������Ӧ����������")]
	public WeaponTypeInHand weaponTypeInHand = WeaponTypeInHand.None;

	[Header("�ø��ӵ�����ͼ��")]
	public GameObject itemIcon;

	[Header("�ø��������еĸ�������")]
	public GameObject glowHighlight;

	#endregion
}
