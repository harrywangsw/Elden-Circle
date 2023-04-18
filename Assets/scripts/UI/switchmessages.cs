using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class switchmessages : MonoBehaviour
{
    public List<string> messages;
    GameObject switch_guide, message_screen;
    public int current = 0;
    Image s_background, m_background;

    void Start()
    {
        switch_guide = GameObject.Find("switch message");
        message_screen = GameObject.Find("message_screen");
        s_background = switch_guide.transform.parent.gameObject.GetComponent<Image>();
        m_background = message_screen.transform.parent.gameObject.GetComponent<Image>();
        messages = new List<string>();
    }

    void Update()
    {
        if(switch_guide.activeSelf){
            s_background.color = Color.white;
        }
        else{
            s_background.color = new Color(1f, 1f, 1f, 0f);
        }
        if(message_screen.GetComponent<TMPro.TextMeshProUGUI>().text!=""){
            m_background.color = Color.white;
        }
        else{
            m_background.color = new Color(1f, 1f, 1f, 0f);
        }
        if(messages.Count>1) switch_guide.SetActive(true);
        else switch_guide.SetActive(false);
        if(messages.Count>0) message_screen.GetComponent<TMPro.TextMeshProUGUI>().text = messages[(current)%messages.Count];
        else message_screen.GetComponent<TMPro.TextMeshProUGUI>().text = "";
        if(Input.GetKeyDown(KeyCode.Y))current+=1;
    }
}
