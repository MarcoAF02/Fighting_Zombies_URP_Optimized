using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 玩家所带的事件发布器
/// </summary>
public class EventHandler_Player : MonoBehaviour
{
	#region 玩家切换武器时发生的事件

	public delegate void ChangeEquipWeapon(
	bool _showEquipItem,
	bool _hideWeaponIcon,
	WeaponTypeInHand _weaponTypeInHand,
	int _currentCount,
	int _totalCount);

	public event ChangeEquipWeapon ChangeEquipWeaponEvent;

	/// <summary>
	/// 响应玩家切换武器事件的函数
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

	#region 玩家从无到有捡起装备到快捷栏时触发的事件

	public delegate void GetNewItemOnEquipUI(WeaponTypeInHand _weaponTypeInHand);

	public event GetNewItemOnEquipUI GetNewItemOnEquipUIEvent;

	/// <summary>
	/// 响应捡起新装备的事件（从无到有出现在快捷栏）
	/// </summary>
	/// <param name="_weaponTypeInHand"></param>
	public void InvokeGetNewItemOnEquipUI(WeaponTypeInHand _weaponTypeInHand)
	{
		GetNewItemOnEquipUIEvent(_weaponTypeInHand);
	}

	#endregion

	#region 玩家生命值改变时发生的事件

	public delegate void HealthChange(bool _isDamage, float _health);
	public event HealthChange HealthChangeEvent;

	/// <summary>
	/// 响应玩家生命值改变的函数
	/// </summary>
	/// <param name="_isDamage"></param>
	/// <param name="_health"></param>
	public void InvokeHealthChange(bool _isDamage, float _health)
	{
		HealthChangeEvent(_isDamage, _health);
	}

	#endregion

	#region 玩家状态改变时发生的事件（FSM 状态机改变）

	// 思路：这个事件可以被动画控制器监听，使得状态转换时动画总是对的

	public delegate void PlayerStateChange(PlayerBaseState playerState);
	public event PlayerStateChange PlayerStateChangeEvent;

	/// <summary>
	/// 响应玩家状态改变的函数
	/// </summary>
	/// <param name="playerState"></param>
	public void InvokePlayerStateChange(PlayerBaseState playerState)
	{
		PlayerStateChangeEvent(playerState);
	}

	#endregion

}
