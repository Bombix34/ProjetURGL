using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActionUI : MonoBehaviour
{
    [SerializeField]
    [NotNull]
    private Button actionButton = null;

    public void EnableButton(bool isEnable)
    {
        actionButton.interactable = isEnable;
    }

    public void Init(PlayerClickInput playerClickInput)
    {
        this.actionButton.onClick.AddListener(playerClickInput.TryPerformInteraction);
    }
}
