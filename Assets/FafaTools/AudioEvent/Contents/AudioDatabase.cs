using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace FafaTools.Audio
{
    [CreateAssetMenu(menuName = "FafaTools/Audio/AudioDatabase")]
    public class AudioDatabase : SingletonScriptableObject<AudioDatabase>
    {
        public List<AudioDataField> m_Datas;

        public void PlaySound(AudioSource audioSource, AudioFieldEnum audioFieldEnum)
        {
            var audioEvent = this.GetAudioEvent(audioFieldEnum);
            audioEvent.Play(audioSource);
        }

        private AudioEvent GetAudioEvent(AudioFieldEnum audioFieldEnum)
        {
            return m_Datas.Single(q => q.m_EnumFieldName == audioFieldEnum.ToString()).m_AudioEvent;
        }
    }
}