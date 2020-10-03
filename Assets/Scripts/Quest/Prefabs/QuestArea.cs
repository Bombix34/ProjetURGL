using Mirror;
using UnityEngine;

public class QuestArea : NetworkBehaviour
{
    public Quest Quest { get; set; }
    private bool questOngoing = false;
    private int playerInsideCollider = 0;
    private float timer = 0.0f;
    // Start is called before the first frame update
    [ServerCallback]
    void Start()
    {
        this.transform.position = this.Quest.Position;
        this.transform.localScale = new Vector2(this.Quest.Radius, this.Quest.Radius);
    }

    // Update is called once per frame
    [ServerCallback]
    void Update()
    {
        if (this.questOngoing)
        {
            timer += Time.deltaTime;
            if(timer >= this.Quest.Duration)
            {
                QuestManager.Instance.FinishQuest(Quest.Guid);
                Destroy(this.gameObject);
            }
        }
    }

    [ServerCallback]
    void OnTriggerEnter2D(Collider2D col)
    {
        if (!col.CompareTag(Tags.STUDENT_TAG))
        {
            return;
        }
        this.playerInsideCollider++;
        this.questOngoing = true;
    }

    [ServerCallback]
    void OnTriggerExit2D(Collider2D col)
    {
        if (!col.CompareTag(Tags.STUDENT_TAG))
        {
            return;
        }
        this.playerInsideCollider--;
        if(this.playerInsideCollider == 0)
        {
            this.timer = 0;
            this.questOngoing = false;
        }
    }
}
