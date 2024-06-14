using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �Ҿ���Ч������Ϸ��Ч
// �� 3D ��Ч

/// <summary>
/// �����Ҿߵ���Ч����Ҫ�ǿ����ź��������룩
/// </summary>
public class InterFurnitureSound : MonoBehaviour
{
	#region ��������ͱ���

	[Header("�Ҿ���Դ���")]
	[SerializeField] private AudioSource furnitureAudioSource;

	[Header("��ת����ƵƬ��")]
	[Header("����")]
	[SerializeField] private AudioClip rotateDoorOpenClip;
	[Header("����")]
	[SerializeField] private AudioClip rotateDoorCloseClip;

	[Header("����������ƵƬ��")]
	[Header("����")]
	[SerializeField] private AudioClip moveFurnOpenClip;
	[Header("�ر�")]
	[SerializeField] private AudioClip moveFurnCloseClip;

	[Header("��ת����������")]
	[SerializeField] private float rotateDoorVolume;
	[Header("����������������")]
	[SerializeField] private float moveFurnVolume;

	#endregion

	#region �Ҿ߻�����Ч����

	// Close ʱ�� Open ��Ч��Open ʱ�� Close ��Ч

	/// <summary>
	/// ���ݼҾ����ͺͼҾ�״̬���Ŷ�Ӧ��Ч
	/// </summary>
	/// <param name="_type"></param>
	/// <param name="_state"></param>
	public void PlayFurnSound(FurnitureType _type, FurnitureState _state)
	{
		if (_type == FurnitureType.RotateDoor)
		{
			if (_state == FurnitureState.Close)
			{
				furnitureAudioSource.volume = rotateDoorVolume;
				furnitureAudioSource.clip = rotateDoorOpenClip;
				furnitureAudioSource.Play();
			}

			if (_state == FurnitureState.Open)
			{
				furnitureAudioSource.volume = rotateDoorVolume;
				furnitureAudioSource.clip = rotateDoorCloseClip;
				furnitureAudioSource.Play();
			}
		}

		if (_type == FurnitureType.PushPull)
		{
			if (_state == FurnitureState.Close)
			{
				furnitureAudioSource.volume = moveFurnVolume;
				furnitureAudioSource.clip = moveFurnOpenClip;
				furnitureAudioSource.Play();
			}

			if (_state == FurnitureState.Open)
			{
				furnitureAudioSource.volume = moveFurnVolume;
				furnitureAudioSource.clip = moveFurnCloseClip;
				furnitureAudioSource.Play();
			}
		}
	}

	#endregion
}
