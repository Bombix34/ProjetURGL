using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LoadingText : MonoBehaviour
{
    [SerializeField]
    private List<string> messages = new List<string>();
    [SerializeField]
    private float everyXSeconds = 0;
    private TextMeshProUGUI textMesh;
    private int index = 1;
    private float timeSinceLastUpdate = 0;

    private void Start()
    {
        this.textMesh = GetComponent<TextMeshProUGUI>();
    }
    private void Update()
    {
        timeSinceLastUpdate += Time.deltaTime;
        if (timeSinceLastUpdate < everyXSeconds)
            return;

        timeSinceLastUpdate = 0;
        index++;
        if (index >= messages.Count)
            index = 0;
        var text = messages[index];
        this.textMesh.text = text;
    }
}
