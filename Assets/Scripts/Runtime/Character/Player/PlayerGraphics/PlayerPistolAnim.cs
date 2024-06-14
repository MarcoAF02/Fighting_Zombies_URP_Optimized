using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 手枪的动画控制器
/// </summary>
public class PlayerPistolAnim : MonoBehaviour
{
	#region 基本组件和变量

	[Header("基本组件和变量")]
	[Header("手枪第一人称角色模型")]
	[SerializeField] private GameObject pistolArm;
	[Header("手枪动画控制器")]
	[SerializeField] private Animator pistolAnimator;
	[Header("角色控制器")]
	[SerializeField] private PlayerController playerController;

	#endregion

	#region 基本生命周期函数

	private void Update()
	{
		MoveAnimation();
		AimStateAnimation();
	}

	#endregion

	#region 动画更新函数

	private void MoveAnimation()
	{
		if (playerController.playerIsSliding)
		{
			pistolAnimator.SetFloat("moveSpeed", 0f);
		}
		else
		{
			pistolAnimator.SetFloat("moveSpeed", playerController.moveVelocity);
		}
	}

	private void AimStateAnimation()
	{
		pistolAnimator.SetBool("shootState", playerController.isAiming);
	}

	public void PlayHideAnim()
	{
		pistolAnimator.SetTrigger("hide");
	}

	public void PlayShootAnim()
	{
		pistolAnimator.SetTrigger("shoot");
	}

	public void PlayReloadAnim()
	{
		pistolAnimator.SetTrigger("reload");
	}

	#endregion
}
