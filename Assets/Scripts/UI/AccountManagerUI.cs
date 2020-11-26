using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AccountManagerUI : MonoBehaviour
{

    [SerializeField]
    [NotNull]
    private TMP_InputField pseudoInput;

    private void Start()
    {
        this.InitPseudoInput();
    }

    private void InitPseudoInput()
    {
        pseudoInput.text = AccountManager.Instance.Account.Pseudo;
        pseudoInput.onValueChanged.AddListener(OnPseudoChange);
    }
    private void OnPseudoChange(string pseudo)
    {
        AccountManager.Instance.Account.Pseudo = pseudo;
    }
}
