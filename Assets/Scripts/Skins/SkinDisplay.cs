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
    private SkinDatas datas = null;

    [SerializeField]
    private SpriteRenderer bodySprite = null;
    [SerializeField]
    private Animator animator;

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
