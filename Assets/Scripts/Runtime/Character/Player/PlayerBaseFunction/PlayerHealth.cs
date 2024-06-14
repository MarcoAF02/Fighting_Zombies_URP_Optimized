using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 玩家生命值管理类
/// </summary>
public class PlayerHealth : MonoBehaviour
{
	#region 基本组件和变量

	[Header("基本组件和变量")]
	[Header("玩家角色控制器")]
	[SerializeField] private PlayerController playerController;
	[Header("玩家相机动画控制器")]
	[SerializeField] private PlayerCameraAnim playerCameraAnim;

	[Header("玩家的嘴")]
	[SerializeField] private PlayerBreathSound playerBreathSound;
	[Header("玩家身体受击音效")]
	[SerializeField] private PlayerHitSound playerHitSound;

	[Header("生命值相关变量")]
	public float maxHealth;
	public float currentHealth;
	[HideInInspector] public bool isDead;

	[Header("玩家 UI 组件")]
	[SerializeField] private HealthBGController healthBGController;

	#endregion

	#region 基本生命周期函数

	private void Awake()
	{
		SetupValue();
	}

	#endregion

	#region 玩家生命值管理功能

	/// <summary>
	/// 初始化玩家生命状态
	/// </summary>
	private void SetupValue()
	{
		currentHealth = maxHealth;
		isDead = false;
	}

	/// <summary>
	/// 玩家受伤功能
	/// </summary>
	/// <param name="damage"></param>
	public void TakeDamage(float damage)
	{
		currentHealth = currentHealth - damage;

		playerController.tutorialTrigger.TutorialPlayerUseMedicine();
		playerController.eventHandler_Player.InvokeHealthChange(true, currentHealth); // 发布生命值改变事件

		if (currentHealth <= 0f)
		{
			currentHealth = 0f;
			isDead = true;
			playerBreathSound.PlayDeadRoarSound();
			playerController.SwitchState(playerController.playerDeadState);
			// Debug.Log(currentHealth);
		}
		else
		{
			playerBreathSound.PlayGetHurtRoarSound();
			// Debug.Log(currentHealth);
		}

		playerHitSound.PlayGetHitSound();
	}

	/// <summary>
	/// 玩家生命值恢复功能
	/// </summary>
	/// <param name="health"></param>
	public void RecoverHealth(float health)
	{
		currentHealth = currentHealth + health;

		playerController.eventHandler_Player.InvokeHealthChange(false, currentHealth); // 发布生命值改变事件

		if (currentHealth >= maxHealth)
		{
			currentHealth = maxHealth;
		}

		playerBreathSound.PlayRecoverBreathSound();
		// Debug.Log(currentHealth);
	}

	#endregion
}
