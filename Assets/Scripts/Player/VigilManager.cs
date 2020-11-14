using UnityEngine;

public class VigilManager : PlayerManager
{
    private GameObject warningFeedback=null;
    protected override void OnStart()
    {
    }


    public GameObject WarningFeedback
    {
        get => warningFeedback;
        set
        {
            warningFeedback = value;
            if(value!=null)
                warningFeedback.transform.position = new Vector2(transform.position.x, transform.position.y + 2.6f);
        }
    }
}
