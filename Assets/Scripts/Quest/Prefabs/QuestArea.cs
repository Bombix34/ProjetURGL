using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestArea : MonoBehaviour
{
    public Quest Quest { get; set; }
    private bool questOngoing = false;
    private float timer = 0.0f;
    // Start is called before the first frame update
    void Start()
    {
        this.transform.position = this.Quest.Position;
        this.transform.localScale = new Vector2(this.Quest.Radius, this.Quest.Radius);
    }

    // Update is called once per frame
    void Update()
    {
        if (this.questOngoing)
        {
            timer += Time.deltaTime;
            if(timer >= this.Quest.Duration)
            {
                QuestManager.Instance.FinishQuest(Quest.Guid);
            }
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        this.questOngoing = true;
    }

    void OnTriggerExit2D(Collider2D col)
    {
        this.questOngoing = false;
        this.timer = 0;
    }
}
