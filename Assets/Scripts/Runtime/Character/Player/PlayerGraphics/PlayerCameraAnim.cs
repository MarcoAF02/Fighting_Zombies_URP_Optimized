using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 玩家 FPS 相机动画控制器
/// </summary>
public class PlayerCameraAnim : MonoBehaviour
{
	#region 基本组件和变量

	[Header("玩家角色控制器")]
	[SerializeField] private PlayerController playerController;
	[Header("动画控制器")]
	[SerializeField] private Animator fpsCameraAnimator;

	[Header("是否要开启视角摇晃，游戏开始时由 SettingLoader 读取")]
	public bool turnOnThePerspectiveShake;

	#endregion

	#region 基本生命周期函数

	private void OnEnable()
	{
		playerController.eventHandler_Player.HealthChangeEvent += TakeDamageVibrateAnim;
	}

	private void Update()
	{
		MoveAnim();
	}

	private void OnDisable()
	{
		playerController.eventHandler_Player.HealthChangeEvent -= TakeDamageVibrateAnim;
	}

	#endregion

	#region 各种动画功能

	// 移动视角摇晃动画更新
	private void MoveAnim()
	{
		if (!turnOnThePerspectiveShake) return;
		if (playerController.playerIsSliding)
		{
			fpsCameraAnimator.SetFloat("moveSpeed", 0f);
		}
		else
		{
			fpsCameraAnimator.SetFloat("moveSpeed", playerController.moveVelocity);
		}
	}

	// 手枪射击时相机震动
	public void PistolShootVibrateAnim()
	{
		fpsCameraAnimator.SetTrigger("pistolShoot");
	}
	
	/// <summary>
	/// 小刀攻击时相机震动
	/// </summary>
	public void KinfeAttackVibrateAnim()
	{
		fpsCameraAnimator.SetTrigger("kinfeAttack");
	}

	/// <summary>
	/// 受击受伤时相机震动
	/// </summary>
	/// <param name="isDamage"></param>
	/// <param name="health"></param>
	public void TakeDamageVibrateAnim(bool isDamage, float health)
	{
		if (isDamage)
		{
			fpsCameraAnimator.SetTrigger("hit");
		}
	}

	#endregion
}
