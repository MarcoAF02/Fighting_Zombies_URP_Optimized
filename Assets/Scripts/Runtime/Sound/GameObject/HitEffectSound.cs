using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 击中音效属于游戏音效
// 属于 3D 立体音效

/// <summary>
/// 击中音效控制器
/// </summary>
public class HitEffectSound : MonoBehaviour
{
	#region 基本组件和变量

	// 注：击中音量是固定的，不用改变

	[Header("击中效果音源组件")]
	[SerializeField] private AudioSource hitAudioSource;

	[Header("子弹相关音频")]
	[Header("击中环境时的默认音频")]
	[SerializeField] private List<AudioClip> bulletDefHitClipList = new List<AudioClip>();
	[Header("击中泥土")]
	[SerializeField] private List<AudioClip> bulletHitDirtClipList = new List<AudioClip>();
	[Header("击中玻璃")]
	[SerializeField] private List<AudioClip> bulletHitGlassClipList = new List<AudioClip>();
	[Header("击中木头")]
	[SerializeField] private List<AudioClip> bulletHitWoodClipList = new List<AudioClip>();
	[Header("击中敌人")]
	[SerializeField] private List<AudioClip> bulletHitEnemyClipList = new List<AudioClip>();

	[Header("小刀相关音频")]
	[Header("击中环境时的默认音频")]
	[SerializeField] private List<AudioClip> kinfeDefHitClipList = new List<AudioClip>();
	[Header("击中泥土")]
	[SerializeField] private List<AudioClip> kinfeHitDirtClipList = new List<AudioClip>();
	[Header("击中玻璃")]
	[SerializeField] private List<AudioClip> kinfeHitGlassClipList = new List<AudioClip>();
	[Header("击中木头")]
	[SerializeField] private List<AudioClip> kinfeHitWoodClipList = new List<AudioClip>();
	[Header("击中敌人")]
	[SerializeField] private List<AudioClip> kinfeHitEnemyClipList = new List<AudioClip>();

	#endregion

	#region 音效播放功能

	#region 子弹音效

	/// <summary>
	/// 播放子弹击中环境的默认音效
	/// </summary>
	public void PlayBulletDefHitSound()
	{
		int randomIndex = UnityEngine.Random.Range(0, bulletDefHitClipList.Count);
		hitAudioSource.clip = bulletDefHitClipList[randomIndex];
		hitAudioSource.Play();
	}

	/// <summary>
	/// 播放子弹击中泥土的音效
	/// </summary>
	public void PlayBulletHitDirtSound()
	{
		int randomIndex = UnityEngine.Random.Range(0, bulletHitDirtClipList.Count);
		hitAudioSource.clip = bulletHitDirtClipList[randomIndex];
		hitAudioSource.Play();
	}

	/// <summary>
	/// 播放子弹击中木头的音效
	/// </summary>
	public void PlayBulletHitWoodSound()
	{
		int randomIndex = UnityEngine.Random.Range(0, bulletHitWoodClipList.Count);
		hitAudioSource.clip = bulletHitWoodClipList[randomIndex];
		hitAudioSource.Play();
	}

	/// <summary>
	/// 播放子弹击中玻璃的音效
	/// </summary>
	public void PlayBulletHitGlassSound()
	{
		int randomIndex = UnityEngine.Random.Range(0, bulletHitGlassClipList.Count);
		hitAudioSource.clip = bulletHitGlassClipList[randomIndex];
		hitAudioSource.Play();
	}

	/// <summary>
	/// 播放子弹击中敌人的音效
	/// </summary>
	public void PlayBulletHitEnemySound()
	{
		int randomIndex = UnityEngine.Random.Range(0, bulletHitEnemyClipList.Count);
		hitAudioSource.clip = bulletHitEnemyClipList[randomIndex];
		hitAudioSource.Play();
	}

	#endregion

	#region 小刀音效

	#endregion

	/// <summary>
	/// 播放小刀击中环境的默认音效
	/// </summary>
	public void PlayKinfeDefHitSound()
	{
		int randomIndex = UnityEngine.Random.Range(0, kinfeDefHitClipList.Count);
		hitAudioSource.clip = kinfeDefHitClipList[randomIndex];
		hitAudioSource.Play();
	}

	/// <summary>
	/// 播放小刀击中泥土的音效
	/// </summary>
	public void PlayKinfeHitDirtSound()
	{
		int randomIndex = UnityEngine.Random.Range(0, kinfeHitDirtClipList.Count);
		hitAudioSource.clip = kinfeHitDirtClipList[randomIndex];
		hitAudioSource.Play();
	}

	/// <summary>
	/// 播放小刀击中木头的音效
	/// </summary>
	public void PlayKinfeHitWoodSound()
	{
		int randomIndex = UnityEngine.Random.Range(0, kinfeHitWoodClipList.Count);
		hitAudioSource.clip = kinfeHitWoodClipList[randomIndex];
		hitAudioSource.Play();
	}

	/// <summary>
	/// 播放小刀击中玻璃的音效
	/// </summary>
	public void PlayKinfeHitGlassSound()
	{
		int randomIndex = UnityEngine.Random.Range(0, kinfeHitGlassClipList.Count);
		hitAudioSource.clip = kinfeHitGlassClipList[randomIndex];
		hitAudioSource.Play();
	}

	/// <summary>
	/// 播放子弹击中敌人的音效
	/// </summary>
	public void PlayKinfeHitEnemySound()
	{
		int randomIndex = UnityEngine.Random.Range(0, kinfeHitEnemyClipList.Count);
		hitAudioSource.clip = kinfeHitEnemyClipList[randomIndex];
		hitAudioSource.Play();
	}

	#endregion
}
