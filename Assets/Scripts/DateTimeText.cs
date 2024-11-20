using System;
using UnityEngine;
using UnityEngine.UI;

public class DateTimeText : MonoBehaviour
{
    private Text dateText;
    void Start()
    {
        dateText = GetComponent<Text>();
        InvokeRepeating(nameof(SetDate), 0.0f, 60.0f);
    }

    private void SetDate()
    {
        var now = DateTime.Now;
        var formattedDate = now.ToString("HH:mm dd:MM:yyyy");
        dateText.text = formattedDate;
    }
}
