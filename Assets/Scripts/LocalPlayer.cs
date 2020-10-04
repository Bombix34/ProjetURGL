using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LocalPlayer : NetworkBehaviour
{
    [ClientCallback]
    public override void OnStartLocalPlayer()
    {
       // GameObject.Find("Canvas").GetComponentsInChildren<Transform>(true).Single(q => q.name == "QuestPanel").gameObject.SetActive(true);
    }
}
