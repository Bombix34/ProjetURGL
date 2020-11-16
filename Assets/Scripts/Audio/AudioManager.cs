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

    private void PlaySoundOnItemStolen(ItemScriptableObject item)
    {
        switch (item.Type)
        {
            case ItemType.VALUABLE_ITEM:
                audioDatabase.PlaySound(this.audioSource, AudioFieldEnum.ALARME_ON);
                break;
            case ItemType.NORMAL_ITEM:
            default:
                break;
        }
    }

    private void PlaySoundOnItemRetrieve(ItemScriptableObject item)
    {
        switch (item.Type)
        {
            case ItemType.VALUABLE_ITEM:
                audioDatabase.PlaySound(this.audioSource, AudioFieldEnum.ALARME_OFF);
                break;
            case ItemType.NORMAL_ITEM:
            default:
                break;
        }
    }
}
