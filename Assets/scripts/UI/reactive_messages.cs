using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class reactive_messages : MonoBehaviour
{
    
    Image background;
    TMPro.TextMeshProUGUI text_box;
    public float message_period;
    void Start()
    {
        background = transform.parent.gameObject.GetComponent<Image>();
        text_box = GetComponent<TMPro.TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator show_message(string message){
        background.color = new Color(1f, 1f, 1f, 0.5f);
        text_box.text = message;
        yield return new WaitForSeconds(message_period);
        text_box.text = "";
        background.color = new Color(1f, 1f, 1f, 0f);
    }
}
