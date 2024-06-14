using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �������ֵ������
/// </summary>
public class PlayerHealth : MonoBehaviour
{
	#region ��������ͱ���

	[Header("��������ͱ���")]
	[Header("��ҽ�ɫ������")]
	[SerializeField] private PlayerController playerController;
	[Header("����������������")]
	[SerializeField] private PlayerCameraAnim playerCameraAnim;

	[Header("��ҵ���")]
	[SerializeField] private PlayerBreathSound playerBreathSound;
	[Header("��������ܻ���Ч")]
	[SerializeField] private PlayerHitSound playerHitSound;

	[Header("����ֵ��ر���")]
	public float maxHealth;
	public float currentHealth;
	[HideInInspector] public bool isDead;

	[Header("��� UI ���")]
	[SerializeField] private HealthBGController healthBGController;

	#endregion

	#region �����������ں���

	private void Awake()
	{
		SetupValue();
	}

	#endregion

	#region �������ֵ������

	/// <summary>
	/// ��ʼ���������״̬
	/// </summary>
	private void SetupValue()
	{
		currentHealth = maxHealth;
		isDead = false;
	}

	/// <summary>
	/// ������˹���
	/// </summary>
	/// <param name="damage"></param>
	public void TakeDamage(float damage)
	{
		currentHealth = currentHealth - damage;

		playerController.tutorialTrigger.TutorialPlayerUseMedicine();
		playerController.eventHandler_Player.InvokeHealthChange(true, currentHealth); // ��������ֵ�ı��¼�

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
	/// �������ֵ�ָ�����
	/// </summary>
	/// <param name="health"></param>
	public void RecoverHealth(float health)
	{
		currentHealth = currentHealth + health;

		playerController.eventHandler_Player.InvokeHealthChange(false, currentHealth); // ��������ֵ�ı��¼�

		if (currentHealth >= maxHealth)
		{
			currentHealth = maxHealth;
		}

		playerBreathSound.PlayRecoverBreathSound();
		// Debug.Log(currentHealth);
	}

	#endregion
}
