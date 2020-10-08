using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class SkinDisplay : NetworkBehaviour
{
    [SerializeField]
    private bool IsAgent=false;
    [SyncVar]
    private int colorId;
    [SyncVar]
    private int headsId;
    [SerializeField]
    private SkinDatas datas;

    [SerializeField]
    private SpriteRenderer bodySprite, headSprite;

    public override void OnStartClient()
    {
        base.OnStartClient();
        if(!IsAgent)
        {
            bodySprite.color = datas.colors[colorId];
            headSprite.sprite = datas.heads[headsId];
        }
        else
        {
            Debug.Log("WOW");
            bodySprite.color = datas.agentColor;
            headSprite.sprite = datas.agentHead;
        }
    }

    public override void OnStartServer()
    {
        this.colorId = Random.Range(0, datas.colors.Count);
        this.headsId = Random.Range(0, datas.heads.Count);
    }
}
