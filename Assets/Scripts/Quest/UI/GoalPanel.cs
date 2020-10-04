using TMPro;
using UnityEngine;

public class GoalPanel : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI GoalName;
    [SerializeField]
    private TextMeshProUGUI GoalDescription;

    public Quest Goal { get; set; }
    // Start is called before the first frame update
    void Start()
    {
        this.GoalName.text = this.Goal.Name;
        this.GoalDescription.text = this.Goal.Description;
    }

    public void Finish()
    {
        this.GoalName.fontStyle = FontStyles.Strikethrough;
        this.GoalDescription.fontStyle = FontStyles.Strikethrough;
        Destroy(this.gameObject, 3f);
    }
}
