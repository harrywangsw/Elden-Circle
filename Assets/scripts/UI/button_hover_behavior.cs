using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class button_hover_behavior : MonoBehaviour
{
    public bool increase;
    public TMPro.TextMeshProUGUI message;
    public bool wait_for_input;
    public Button b;
    void Start(){
        message = GameObject.Find("UI_control").GetComponent<TMPro.TextMeshProUGUI>();
        b = GetComponent<Button>();
    }

    public void show_instructions(){
        if(increase) message.text = "press left mouse button/B to level-up";
        else message.text = "press left mouse button/B to level-down";
        wait_for_input = true;
    }

    public void clear_instructions(){
        message.text = "";
        wait_for_input = false;
    }

    void Update(){
        if(wait_for_input&&Input.GetButtonDown("confirm")&&b.interactable){
            b.onClick.Invoke();
        }
    }
}
