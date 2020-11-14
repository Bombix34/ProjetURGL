using UnityEngine;

public class Map : MonoBehaviour
{
    [SerializeField]
    [NotNull]
    private GameObject mapUI;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            this.DisplayMap();
        }
        if (Input.GetKeyUp(KeyCode.Tab))
        {
            this.HideMap();
        }
    }

    private void DisplayMap()
    {
        mapUI.SetActive(true);
    }

    private void HideMap()
    {
        mapUI.SetActive(false);
    }


}
