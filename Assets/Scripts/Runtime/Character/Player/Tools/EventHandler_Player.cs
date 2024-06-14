using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ����������¼�������
/// </summary>
public class EventHandler_Player : MonoBehaviour
{
	#region ����л�����ʱ�������¼�

	public delegate void ChangeEquipWeapon(
	bool _showEquipItem,
	bool _hideWeaponIcon,
	WeaponTypeInHand _weaponTypeInHand,
	int _currentCount,
	int _totalCount);

	public event ChangeEquipWeapon ChangeEquipWeaponEvent;

	/// <summary>
	/// ��Ӧ����л������¼��ĺ���
	/// </summary>
	/// <param name="_weaponTypeInHand"></param>
	public void InvokeChangeEquipWeapon(
	bool _showEquipItem,
	bool _hideWeaponIcon,
	WeaponTypeInHand _weaponTypeInHand,
	int _currentCount,
	int _totalCount)
	{
		ChangeEquipWeaponEvent(_showEquipItem, _hideWeaponIcon, _weaponTypeInHand, _currentCount, _totalCount);
	}

	#endregion

	#region ��Ҵ��޵��м���װ���������ʱ�������¼�

	public delegate void GetNewItemOnEquipUI(WeaponTypeInHand _weaponTypeInHand);

	public event GetNewItemOnEquipUI GetNewItemOnEquipUIEvent;

	/// <summary>
	/// ��Ӧ������װ�����¼������޵��г����ڿ������
	/// </summary>
	/// <param name="_weaponTypeInHand"></param>
	public void InvokeGetNewItemOnEquipUI(WeaponTypeInHand _weaponTypeInHand)
	{
		GetNewItemOnEquipUIEvent(_weaponTypeInHand);
	}

	#endregion

	#region �������ֵ�ı�ʱ�������¼�

	public delegate void HealthChange(bool _isDamage, float _health);
	public event HealthChange HealthChangeEvent;

	/// <summary>
	/// ��Ӧ�������ֵ�ı�ĺ���
	/// </summary>
	/// <param name="_isDamage"></param>
	/// <param name="_health"></param>
	public void InvokeHealthChange(bool _isDamage, float _health)
	{
		HealthChangeEvent(_isDamage, _health);
	}

	#endregion

	#region ���״̬�ı�ʱ�������¼���FSM ״̬���ı䣩

	// ˼·������¼����Ա�����������������ʹ��״̬ת��ʱ�������ǶԵ�

	public delegate void PlayerStateChange(PlayerBaseState playerState);
	public event PlayerStateChange PlayerStateChangeEvent;

	/// <summary>
	/// ��Ӧ���״̬�ı�ĺ���
	/// </summary>
	/// <param name="playerState"></param>
	public void InvokePlayerStateChange(PlayerBaseState playerState)
	{
		PlayerStateChangeEvent(playerState);
	}

	#endregion

}
