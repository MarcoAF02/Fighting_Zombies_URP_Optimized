using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 场景加载音效属于系统音效
// 不是 3D 音效

public class SceneLoadSound : MonoBehaviour
{
	#region 基本组件和变量

	[Header("场景加载音源组件")]
	[SerializeField] private AudioSource sceneLoadAudioSource;

	#endregion

	#region 基本生命周期函数

	private void Start()
	{
		sceneLoadAudioSource.ignoreListenerPause = true; // 该音效不收暂停影响
	}

	#endregion

	#region 音效播放功能

	/// <summary>
	/// 播放场景加载音效
	/// </summary>
	public void PlaySceneLoadSound()
	{
		sceneLoadAudioSource.Play();
	}

	#endregion
}
