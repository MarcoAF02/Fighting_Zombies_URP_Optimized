using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 玩家生命值背景控制器
/// </summary>
public class HealthBGController : MonoBehaviour
{
	#region 基本组件和变量

	// 受伤或回血的瞬间，UI 背景的变化由 Animation 控制。非满血状态的背景图片由程序控制

	[Header("玩家角色控制器")]
	[SerializeField] private PlayerController playerController;
	[Header("玩家生命值管理类")]
	[SerializeField] private PlayerHealth playerHealth;

	// ==================== 受伤背景图片 ==================== //

	[Header("受伤背景图片")]
	[Tooltip("非满血状态一直显示在屏幕上的图片")]
	[SerializeField] private Image takeDamageBG;

	[Header("受伤背景图片的最大不透明度（防止挡玩家屏幕）")]
	[Range(0, 1)]
	public float takeDamageBGMaxOpacity;

	[Header("改变生命值时，受伤背景 Alpha 值的改变比例（值越大改变速度越慢）")]
	[Range(0.01f, 0.02f)]
	public float takeDamageBGChangeProportion;

	// ==================== 受击或恢复生命的背景图片 ==================== //

	[Header("受伤背景 UI 变化的动画")]
	[SerializeField] private Animator takeDamageImageAnimator;

	[Header("恢复背景 UI 变化的动画")]
	[SerializeField] private Animator recoverHealthImageAnimator;

	#endregion

	#region 基本生命周期函数

	private void OnEnable()
	{
		playerController.eventHandler_Player.HealthChangeEvent += ChangeTakeDamageBG;
		playerController.eventHandler_Player.HealthChangeEvent += ShowHealthChangeImage;
	}

	private void OnDisable()
	{
		playerController.eventHandler_Player.HealthChangeEvent -= ChangeTakeDamageBG;
		playerController.eventHandler_Player.HealthChangeEvent -= ShowHealthChangeImage;
	}

	#endregion

	#region 生命值改变时改变背景图片显示

	/// <summary>
	/// 更新背景图片 UI 显示，用于调整设置后的修正
	/// </summary>
	public void FixTakeDamageBG()
	{
		takeDamageBG.color = new Color(1f, 1f, 1f, 1f - (playerHealth.currentHealth * takeDamageBGChangeProportion));

		if (takeDamageBG.color.a >= takeDamageBGMaxOpacity)
		{
			takeDamageBG.color = new Color(1f, 1f, 1f, takeDamageBGMaxOpacity);
		}
	}

	/// <summary>
	/// 生命值发生变化时，改变背景图片显示
	/// </summary>
	/// <param name="isDamage"></param>
	/// <param name="health"></param>
	public void ChangeTakeDamageBG(bool isDamage, float health)
	{
		takeDamageBG.color = new Color(1f, 1f, 1f, 1f - (health * takeDamageBGChangeProportion));

		if (takeDamageBG.color.a >= takeDamageBGMaxOpacity)
		{
			takeDamageBG.color = new Color(1f, 1f, 1f, takeDamageBGMaxOpacity);
		}
	}


	/// <summary>
	/// 玩家受击或恢复生命的瞬间显示一个背景
	/// </summary>
	/// <param name="isDamage"></param>
	/// <param name="health"></param>
	public void ShowHealthChangeImage(bool isDamage, float health)
	{
		if (isDamage)
		{
			takeDamageImageAnimator.SetTrigger("display");
		}
		else
		{
			recoverHealthImageAnimator.SetTrigger("display");
		}
	}

	#endregion

}
