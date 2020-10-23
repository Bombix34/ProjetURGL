using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerAreaRenderer : NetworkBehaviour
{
    [SerializeField]
    private Material defaultMaterial, behindMaskMaterial;
    private GameObject[] allObjects;
    private PlayerClickInput playerClickInput;
    private FieldOfView fieldViewManager;

    private void Start()
    {
        if (!hasAuthority)
            return;
        playerClickInput = GetComponent<PlayerClickInput>();
        allObjects = GameObject.FindGameObjectsWithTag("PositionTrigger");
        fieldViewManager = Camera.main.GetComponent<CameraManager>().FieldOfView;
    }

    private void Update()
    {
        if (!hasAuthority)
            return;
        foreach (var obj in allObjects)
        {
            if (!obj.transform.parent.gameObject.activeInHierarchy || obj.transform.parent == this.transform || obj.transform.parent == playerClickInput.CurrentObjectClicked || obj.transform.parent == playerClickInput.CurrentObjectOver)
                continue;
            if (!fieldViewManager.IsObjectVisibleFromPlayer(this.transform.gameObject, obj))
                obj.transform.parent.GetComponentInChildren<CharacterRenderer>().SwitchMaterial(behindMaskMaterial, false);
            else
                obj.transform.parent.GetComponentInChildren<CharacterRenderer>().SwitchMaterial(defaultMaterial);
        }
    }
}
