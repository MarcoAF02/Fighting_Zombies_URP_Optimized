using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// �������ֵ����������
/// </summary>
public class HealthBGController : MonoBehaviour
{
	#region ��������ͱ���

	// ���˻��Ѫ��˲�䣬UI �����ı仯�� Animation ���ơ�����Ѫ״̬�ı���ͼƬ�ɳ������

	[Header("��ҽ�ɫ������")]
	[SerializeField] private PlayerController playerController;
	[Header("�������ֵ������")]
	[SerializeField] private PlayerHealth playerHealth;

	// ==================== ���˱���ͼƬ ==================== //

	[Header("���˱���ͼƬ")]
	[Tooltip("����Ѫ״̬һֱ��ʾ����Ļ�ϵ�ͼƬ")]
	[SerializeField] private Image takeDamageBG;

	[Header("���˱���ͼƬ�����͸���ȣ���ֹ�������Ļ��")]
	[Range(0, 1)]
	public float takeDamageBGMaxOpacity;

	[Header("�ı�����ֵʱ�����˱��� Alpha ֵ�ĸı������ֵԽ��ı��ٶ�Խ����")]
	[Range(0.01f, 0.02f)]
	public float takeDamageBGChangeProportion;

	// ==================== �ܻ���ָ������ı���ͼƬ ==================== //

	[Header("���˱��� UI �仯�Ķ���")]
	[SerializeField] private Animator takeDamageImageAnimator;

	[Header("�ָ����� UI �仯�Ķ���")]
	[SerializeField] private Animator recoverHealthImageAnimator;

	#endregion

	#region �����������ں���

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

	#region ����ֵ�ı�ʱ�ı䱳��ͼƬ��ʾ

	/// <summary>
	/// ���±���ͼƬ UI ��ʾ�����ڵ������ú������
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
	/// ����ֵ�����仯ʱ���ı䱳��ͼƬ��ʾ
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
	/// ����ܻ���ָ�������˲����ʾһ������
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
