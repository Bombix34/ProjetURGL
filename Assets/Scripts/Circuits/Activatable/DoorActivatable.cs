using Mirror;
using UnityEngine;

public class DoorActivatable : BaseActivatable
{
    private const string animatorIsOpen = "IsOpen";
    [SerializeField]
    [NotNull]
    private Animator animator = null;
    internal override void OnActivate(NetworkConnectionToClient sender)
    {
        this.animator.SetBool(animatorIsOpen, true);
    }

    internal override void OnDeactivate(NetworkConnectionToClient sender)
    {
        this.animator.SetBool(animatorIsOpen, false);
    }

    [ClientRpc]
    public override void RpcActivateClient()
    {
        this.animator.SetBool(animatorIsOpen, true);
    }

    [ClientRpc]
    public override void RpcDeactivateClient()
    {
        this.animator.SetBool(animatorIsOpen, false);
    }
}
