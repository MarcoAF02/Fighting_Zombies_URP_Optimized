using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �̳���ʾ��Ϣ��������������ҽ̳���Ϣ�ĳ���ʱ����
/// </summary>
public class TutorialTipTrigger : MonoBehaviour
{
	#region ��������ͱ���

	[Header("�̳���Ϣ�Ƿ�Ҫ�������ñ�����Դ��������ã�")]
	public bool openTutorial;

	[Header("�����ƶ�����೤ʱ�����ұ��ܺ��¶�")]
	[SerializeField] private float tutorialRunDelayTime;
	[Header("һ��̵̳���ʾʱ��")]
	[SerializeField] private float tutorialDisplayTime = 6;
	[Header("��ʾ��Ҽ����Ʒ�����ӳ�ʱ��")]
	[SerializeField] private float tutorialCheckGameTargetDelayTime;
	[Header("��ʾ���ʹ��ҽ�ư����Ƶ��ӳ�ʱ��")]
	[SerializeField] private float tutorialUseMedicineDelayTime;
	[Header("�̳���Ұ����")]
	[SerializeField] private float tutorialRayDistance;

	[Header("��ҽ�ѧ��ֱ����Ұ")]
	[Header("�����������")]
	[SerializeField] private PlayerController playerController;

	// �̳���Ϣ
	private bool teachMove; // �Ƿ��Ѿ��̹������ô�ƶ�
	private bool teachUseEquipItem; // �Ƿ�̹������Ʒ����ʹ��
	private bool teachUseGun; // �Ƿ�̹����ʹ��ǹе
	private bool teachUseMedicine; // �Ƿ�̹��������
	private bool teachCheckGameTarget; // �Ƿ�̹���Ҽ����ϷĿ��
	private bool firstViewEnemy; // �Ƿ��ǵ�һ�ο�������

	// Э��
	private Coroutine tutorialPlayerMove_IECor;
	private Coroutine tutorialPlayerCheckGameTarget_IECor;
	private Coroutine tutorialPlayerUseMedicine_IE_TECor;

	#endregion

	#region �̳���Ϣ��������

	/// <summary>
	/// �������ô�ƶ�
	/// </summary>
	public void TutorialPlayerMove()
	{
		if (!openTutorial) return;
		if (teachMove) return;

		tutorialPlayerMove_IECor = StartCoroutine(TutorialPlayerMove_IE());
		teachMove = true;
	}

	// �������ô�ƶ�
	private IEnumerator TutorialPlayerMove_IE()
	{
		playerController.tipMessageController.ShowTutorialMessage("FirstMove", tutorialDisplayTime);
		yield return new WaitForSeconds(tutorialRunDelayTime);
		TutorialPlayerRunAndCrouch();
	}

	// ����ұ��ܺ��¶�
	private void TutorialPlayerRunAndCrouch()
	{
		if (!openTutorial) return;
		playerController.tipMessageController.ShowTutorialMessage("FirstRunAndCrouch", tutorialDisplayTime);
	}

	/// <summary>
	/// ����Ұ��� Tab �����ϷĿ�꣨�����Ʒ����
	/// </summary>
	public void TutorialPlayerCheckGameTarget()
	{
		if (!openTutorial) return;
		if (teachCheckGameTarget) return;
		tutorialPlayerCheckGameTarget_IECor = StartCoroutine(TutorialPlayerCheckGameTarget_IE());
	}

	// ����Ұ��� Tab �����ϷĿ�꣨�����Ʒ����
	private IEnumerator TutorialPlayerCheckGameTarget_IE()
	{
		yield return new WaitForSeconds(tutorialCheckGameTargetDelayTime);

		playerController.tipMessageController.ShowTutorialMessage("FirstUseInventory", tutorialDisplayTime);
		teachCheckGameTarget = true;
	}

	/// <summary>
	/// ����Ұ� 4 ��������
	/// </summary>
	public void TutorialPlayerUseMedicine()
	{
		if (!openTutorial) return;
		if (teachUseMedicine) return;
		tutorialPlayerUseMedicine_IE_TECor = StartCoroutine(TutorialPlayerUseMedicine_IE());
	}

	// �����������ˣ���ʾ�����ҽ����Ʒ����
	private IEnumerator TutorialPlayerUseMedicine_IE()
	{
		yield return new WaitForSeconds(tutorialUseMedicineDelayTime);
		playerController.tipMessageController.ShowTutorialMessage("FirstUseMedicine", tutorialUseMedicineDelayTime);
		teachUseMedicine = true;
	}

	// �����ҿ����˵�һ�����ˣ�������Ʒ�������̡̳�ͬʱ����е�����ǹ��������������̳�
	private void TutorialPlayerUseEquipItem()
	{
		if (!openTutorial) return;
		playerController.tipMessageController.ShowTutorialMessage("FirstUseEquipItem", tutorialDisplayTime);
		teachUseEquipItem = true;
	}

	// �����ҿ��������˲����л�������ǹе
	public void TutorialPlayerFirstUseGun()
	{
		if (!openTutorial) return;
		if (!teachUseEquipItem) return;
		if (teachUseGun) return;

		playerController.tipMessageController.ShowTutorialMessage("FirstUseGun", tutorialDisplayTime);
		teachUseGun = true;
	}

	#endregion

	#region �̳���Ϣ��Ұ����

	/// <summary>
	///	�̳���Ϣ���������������ˣ�
	/// </summary>
	public void PlayerFirstViewEnemy_Tutorial()
	{
		if (!openTutorial) return;
		if (firstViewEnemy) return;

		RaycastHit hitObj;

		if (Physics.Raycast(playerController.playerCameraController.playerCamera.transform.position, playerController.playerCameraController.playerCamera.transform.forward, out hitObj, tutorialRayDistance))
		{
			if (hitObj.collider.CompareTag(playerController.layerAndTagCollection_Player.enemyTag))
			{
				firstViewEnemy = true;
				TutorialPlayerUseEquipItem();
			}
		}
	}

	#endregion
}
