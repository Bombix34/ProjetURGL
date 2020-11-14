using UnityEngine;
using UnityEngine.Audio;

namespace FafaTools.Audio
{
	public abstract class AudioEvent : ScriptableObject
	{
		public AudioMixerGroup m_AudioMixerGroup;
		public bool m_IsLooping;
		public AudioClip[] m_Clips;
		public abstract void Play(AudioSource source);
	}
}
