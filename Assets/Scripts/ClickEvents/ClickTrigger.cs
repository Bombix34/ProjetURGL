using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickTrigger : MonoBehaviour
{
    public TriggerClickType clickType;
    [SerializeField]
    private SpriteRenderer spriteRenderer;

    [SerializeField]
    private HighlightSettings highlightSettings;


    [SerializeField]
    private Material interactMaterial;
    private Material baseMaterial;

    private void Start()
    {
        if (spriteRenderer != null)
        {
            baseMaterial = spriteRenderer.material;
        }
    }


    public void OnClickObject(PlayerManager currentPlayer)
    {
        if (PlayerCanInteract(currentPlayer))
        {
            Debug.Log("CLIQUE CLIQUE");
            spriteRenderer.material.SetFloat("_Thickness", highlightSettings.size);
            spriteRenderer.material.SetColor("_SolidOutline", highlightSettings.onMouseClickColor);
        }
    }

    public void OnMouseOverTrigger(PlayerManager currentPlayer)
    {
        if(PlayerCanInteract(currentPlayer))
        {
            spriteRenderer.material = interactMaterial;
            spriteRenderer.material.SetFloat("_Thickness", highlightSettings.size);
            spriteRenderer.material.SetColor("_SolidOutline", highlightSettings.onMouseOverColor);
        }
    }

    public void OnMouseExitTrigger()
    {
        spriteRenderer.material.SetFloat("_Thickness", 0f);
        spriteRenderer.material.SetFloat("_OutlineMode", 0);
        spriteRenderer.material.SetFloat("_Angle", 0f);
        spriteRenderer.material.SetColor("_SolidOutline", highlightSettings.onMouseOverColor);
        spriteRenderer.material = baseMaterial;
    }

    private bool PlayerCanInteract(PlayerManager currentPlayer)
    {
        FieldOfView fieldViewManager = Camera.main.GetComponent<CameraManager>().FieldOfView;
        if (!fieldViewManager.IsPlayerCanInteractWithObject(currentPlayer.gameObject, this.transform.parent.gameObject))
            return false;
        bool isPlayerVigil = currentPlayer.IsVigil;
        bool result = false;
        switch (clickType)
        {
            case TriggerClickType.PNJ:
                if (isPlayerVigil)
                    result = true;
                break;
            case TriggerClickType.VIGIL:
                break;
            case TriggerClickType.THIEF:
                if (isPlayerVigil)
                    result = true;
                break;
        }
        return result;
    }
}

public enum TriggerClickType
{
    PNJ,
    THIEF,
    VIGIL
}
