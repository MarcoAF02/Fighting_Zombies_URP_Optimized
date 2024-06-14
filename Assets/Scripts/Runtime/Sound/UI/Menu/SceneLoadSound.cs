using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ����������Ч����ϵͳ��Ч
// ���� 3D ��Ч

public class SceneLoadSound : MonoBehaviour
{
	#region ��������ͱ���

	[Header("����������Դ���")]
	[SerializeField] private AudioSource sceneLoadAudioSource;

	#endregion

	#region �����������ں���

	private void Start()
	{
		sceneLoadAudioSource.ignoreListenerPause = true; // ����Ч������ͣӰ��
	}

	#endregion

	#region ��Ч���Ź���

	/// <summary>
	/// ���ų���������Ч
	/// </summary>
	public void PlaySceneLoadSound()
	{
		sceneLoadAudioSource.Play();
	}

	#endregion
}
