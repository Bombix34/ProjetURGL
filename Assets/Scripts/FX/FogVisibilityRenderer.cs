using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FogVisibilityRenderer : MonoBehaviour
{
    [SerializeField]
    private GameObject fogRenderer, noFogRenderer;

    private bool IsVisible = false;

    public void SwitchVisibility(bool isVisible)
    {
        //PASSAGE A LETAT INVISIBLE
        if (!isVisible && this.IsVisible)
        {
            fogRenderer.SetActive(true);
            noFogRenderer.GetComponent<FogOfWarSprite>().enabled = true;
            noFogRenderer.GetComponent<SpriteRenderer>().enabled = false;
            this.IsVisible = isVisible;
        }
        //PASSAGE A LETAT VISIBLE
        else if (isVisible && !this.IsVisible)
        {
            fogRenderer.SetActive(false);
            noFogRenderer.GetComponent<FogOfWarSprite>().enabled = false;
            noFogRenderer.GetComponent<SpriteRenderer>().enabled = true;

            this.IsVisible = isVisible;
        }
    }
}
