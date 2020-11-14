using UnityEngine;

public class MusicManager : MonoBehaviour
{
    private PedestalManager pedestalManager;

    private void Start()
    {
        this.pedestalManager = GetComponent<PedestalManager>();
        if (true)
        {
            pedestalManager.onItemStolen += PlayMusicOnItemStolen;
            pedestalManager.onItemRetrieve += PlayMusicOnItemRetrieve;
        }
    }

    private void PlayMusicOnItemStolen()
    {

    }

    private void PlayMusicOnItemRetrieve()
    {

    }
}
