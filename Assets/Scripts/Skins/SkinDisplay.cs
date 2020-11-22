using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class SkinDisplay : NetworkBehaviour
{
    [SyncVar]
    private int colorId;
    [SyncVar]
    private int animatorID;
    [SerializeField]
    [NotNull]
    private SkinDatas datas = null;
    [SerializeField]
    [NotNull]
    private SpriteRenderer bodySprite = null;
    [SerializeField]
    [NotNull]
    private Animator animator = null;

    public override void OnStartClient()
    {
        base.OnStartClient();
        bodySprite.color = datas.colors[colorId];
        animator.runtimeAnimatorController = datas.animatorController[animatorID];
    }

    public override void OnStartServer()
    {
        this.colorId = Random.Range(0, datas.colors.Count);
        this.animatorID = Random.Range(0, datas.animatorController.Count);
    }
}
