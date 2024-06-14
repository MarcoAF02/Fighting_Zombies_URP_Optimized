using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ��ʰȡ��Ʒ��Ϣ������
/// </summary>
public class PickableItemMessageManager : MonoSingleton<PickableItemMessageManager>
{
	#region ��������ͱ���

	[Header("��Ʒ��Ϣ�б���Ϣ��Դ��")]
	[SerializeField] private PickableItemMessageList itemMessageList;

	[Header("��Ʒ��Ϣ��Դ��ʹ����Ʒʱ��ȡ��")]
	public List<PickableItemMessage> pickableItemMessageListToUse = new List<PickableItemMessage>();

	#endregion

	#region �����������ں���

	protected override void Awake()
	{
		base.Awake();
		DontDestroyOnLoad(gameObject);
	}

	private void Start()
	{
		CopyItemMessageToUse();
	}

	#endregion

	#region ��Ϣ���ƹ���

	private void CopyItemMessageToUse()
	{
		pickableItemMessageListToUse.Clear(); // ���������Ϣ

		// �����Ϣ
		for (int i = 0; i < itemMessageList.pickableItemMessageList.Count; i ++)
		{
			pickableItemMessageListToUse.Add(itemMessageList.pickableItemMessageList[i]);
		}
	}

	#endregion
}
