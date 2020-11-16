using System;
using UnityEngine;

[Serializable]
public class ChargeActionConfigurationParameter : BaseActionConfigurationParameter
{
    [SerializeField]
    private uint charges = 0;
    private uint currentCharges;

    public uint CurrentCharges { get => currentCharges; }

    public override void Init()
    {
        this.currentCharges = this.charges;
    }

    protected override bool CanDoActionIfEnabled()
    {
        return this.currentCharges > 0;
    }

    protected override void OnActionIfEnabled()
    {
        this.currentCharges--;
    }
}
