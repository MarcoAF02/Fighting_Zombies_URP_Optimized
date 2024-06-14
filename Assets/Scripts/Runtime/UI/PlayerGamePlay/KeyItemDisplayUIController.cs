using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// �ؼ����� UI ��Ƭ������
/// </summary>
public class KeyItemDisplayUIController : MonoBehaviour
{
	#region ��������ͱ���

	[Header("��Ʒ ID")]
	public string _itemID; // ���������ʾ����Ļ��
	[Header("��Ʒ�����ı�")]
	public TextMeshProUGUI _itemDescribe;
	[Header("��Ʒͼ��")]
	public Image _itemIcon;

	#endregion
}
