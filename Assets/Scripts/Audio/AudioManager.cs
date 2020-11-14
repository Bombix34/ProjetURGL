using FafaTools.Audio;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(PedestalManager))]
public class AudioManager : MonoBehaviour
{
    [SerializeField]
    [NotNull]
    private AudioDatabase audioDatabase;
    private PedestalManager pedestalManager;
    private AudioSource audioSource;

    private void Start()
    {
        this.pedestalManager = GetComponent<PedestalManager>();
        this.audioSource = GetComponent<AudioSource>();
        if (RoomPlayerData.LocalPlayer.PlayerType != PlayerType.THIEF)
        {
            pedestalManager.onItemStolen += PlaySoundOnItemStolen;
            pedestalManager.onItemRetrieve += PlaySoundOnItemRetrieve;
        }
    }

    private void PlaySoundOnItemStolen()
    {
        audioDatabase.PlaySound(this.audioSource, AudioFieldEnum.ALARME_ON);
    }

    private void PlaySoundOnItemRetrieve()
    {
        audioDatabase.PlaySound(this.audioSource, AudioFieldEnum.ALARME_OFF);
    }
}
