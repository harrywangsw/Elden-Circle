using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class switchmessages : MonoBehaviour
{
    public List<string> messages;
    GameObject switch_guide;
    int current

    void Start()
    {
        switch_guide = GameObject.Find("switch messages");
        messages = new List<string>();
        messages.Add("");
    }

    void Update()
    {
        if(messages.Length()>2) switch_guide.GetComponent<RectTransform>().localScale = Vector3.one;
        else switch_guide.GetComponent<RectTransform>().localScale = Vector3.zero;
        if(Input.GetKeyUp(KeyCode.Y))
        {
            message_screen.GetComponent<TMPro.TextMeshProUGUI>().text = messages[(current+1)%messages.Length()];
        }
    }
}
