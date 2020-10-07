﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class SkinDisplay : NetworkBehaviour
{
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
        bodySprite.color = datas.colors[colorId];
        headSprite.sprite = datas.heads[headsId];
    }

    public override void OnStartServer()
    {
        this.colorId = Random.Range(0, datas.colors.Count);
        this.headsId = Random.Range(0, datas.heads.Count);
    }
}