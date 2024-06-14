using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType // ��¼��Ʒ�����࣬��һ����ʰȡ��Ʒ����Ĳ�ͬ������ͬ��Ӧ
{
	None,
	Key, // ���չؼ���Ʒ
	Supply, // ���ʲ���
}

// ע�⣺��Ϸ�е���Ϸ���岻Ӧ�����κ�ʵ������ɾ�� GameObject �Ĳ����������Ż���

/// <summary>
/// ��ʰȡ��Ʒ��������Ӧ�ù����ڿ�ʰȡ��Ʒ�ĸ������ϣ�
/// </summary>
public class PickableItem : MonoBehaviour
{
	#region ��������ͱ���

	[Header("ָ������Ʒ������")]
	public ItemType itemType = ItemType.None;

	[Header("��Ʒ ID")]
	[Tooltip("��������������Ʒ�����ͷ����Ӧ����Ʒ����")]
	public string itemID;

	[Header("ʰȡ����ƷʱҪ��Ҫ�ı���Ϸ�׶�")]
	public bool canChangeGameState;

	[Header("Ŀ����Ϸ�׶�")]
	[Tooltip("��������Ʒʰȡ���ı���Ϸ�׶Σ�ָ��Ŀ����Ϸ�׶�")]
	public GameProgress targetGameProgress;

	[Header("�Ƿ�Ҫ��ʾ��ϷĿ����Ϣ")]
	[Tooltip("���������Ʒʰȡ��Ӧ��Ӧ����ʾ���Ŀ��ı�")]
	public bool canNoticePlayer;
	[Header("��ʾ�ı�������ʲô��ʾ�׶�")]
	public GameTargetTipState gameTargetTipState;

	[Header("ʰȡ��Ʒʱ�᲻����ʾ˵����Ϣ")]
	public bool showTipMessage;

	[Header("�����ʾ��ʾ��Ϣ������Ϊ")]
	public string tipMessage;

	[Header("��������Ʒ�����ʲ���������������Ϊ")]
	public int supplies;

	#endregion

	#region �����������ں���

	private void OnDisable()
	{
		if (canChangeGameState)
		{
			if (GameProgressManager.Instance != null)
			{
				GameProgressManager.Instance.PerformChangeGameProgress(targetGameProgress);
			}
		}
	}

	#endregion
}
