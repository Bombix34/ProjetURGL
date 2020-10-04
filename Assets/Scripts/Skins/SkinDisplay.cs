using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class SkinDisplay : NetworkBehaviour
{
    [SerializeField]
    private SkinDatas datas;

    [SerializeField]
    private SpriteRenderer bodySprite, headSprite;

    public override void OnStartServer()
    {
        if(isServer || hasAuthority)
            Generate();
    }

    private void Generate()
    {
        bodySprite.color = datas.colors[Random.Range(0, datas.colors.Count)];
        headSprite.sprite = datas.heads[Random.Range(0, datas.heads.Count)];
    }
}
