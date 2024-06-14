using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ���˵����ֿ�����
/// </summary>
public class MainMenuMusic : MonoBehaviour
{
	#region ��������ͱ���

	[Header("������Դ���")]
	[SerializeField] private AudioSource mainMenuBGMSource;

	#endregion

	#region �����������ں���

	private void Start()
	{
		mainMenuBGMSource.ignoreListenerPause = true;
		mainMenuBGMSource.Play();
	}

	#endregion
}
