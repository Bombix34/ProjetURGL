using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FafaTools.Audio
{
	[CreateAssetMenu(menuName = "FafaTools/Audio/SimpleAudio")]
	public class SimpleAudio : AudioEvent
	{
		public override void Play(AudioSource source)
		{
			source.clip = m_Clips[Random.Range(0, m_Clips.Length)];
			source.loop = m_IsLooping;
			source.outputAudioMixerGroup = m_AudioMixerGroup;
			source.Play();
		}
	}
}
