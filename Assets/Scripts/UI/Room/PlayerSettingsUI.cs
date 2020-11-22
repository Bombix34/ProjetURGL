using System;
using TMPro;
using UnityEngine;

public class PlayerSettingsUI : MonoBehaviour
{
    [SerializeField]
    [NotNull]
    private TMP_InputField speedInput;
    private PlayerSettings playerSettings;

    public void Init(PlayerSettings playerSettings)
    {
        this.playerSettings = playerSettings;
        this.InitSpeedInput();
    }

    private void InitSpeedInput()
    {
        speedInput.text = playerSettings.MovementSpeed.ToString();
        speedInput.onValueChanged.AddListener(OnSpeedChange);
    }

    private void OnSpeedChange(string speed)
    {
        playerSettings.MovementSpeed = float.Parse(speed);
    }
}
