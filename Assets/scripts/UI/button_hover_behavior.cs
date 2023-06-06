using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class button_hover_behavior : MonoBehaviour
{
    public bool increase, dont_show_instruction;
    public TMPro.TextMeshProUGUI message;
    public bool wait_for_input;
    public Button b;
    void Start(){
        if(!dont_show_instruction) message = GameObject.Find("UI_control").GetComponent<TMPro.TextMeshProUGUI>();
        b = GetComponent<Button>();
    }

    public void show_instructions(){
        wait_for_input = true;
        if(dont_show_instruction) return;
        if(increase) message.text = "press left mouse button/B to level-up";
        else message.text = "press left mouse button/B to level-down";
    }

    public void clear_instructions(){
        wait_for_input = false;
        if(dont_show_instruction) return;
        message.text = "";
    }

    void Update(){
        if(wait_for_input&&Input.GetButtonDown("confirm")&&b.interactable){
            b.onClick.Invoke();
        }
    }
}
