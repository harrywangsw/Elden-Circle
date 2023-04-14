using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class edit_stat : MonoBehaviour
{
    public float increment, stat;
    string start_text;
    void Start()
    {
        transform.GetChild(0).gameObject.GetComponent<Button>().onClick.AddListener(delegate{change_stat(false);});
        transform.GetChild(1).gameObject.GetComponent<Button>().onClick.AddListener(delegate{change_stat(true);});
        start_text = transform.parent.GetChild(0).gameObject.GetComponent<TMPro.TextMeshProUGUI>().text;
    }

    public void change_stat(bool decrease){
        if(decrease) stat-=increment;
        else stat+=increment;
    }

    void Update()
    {
        transform.parent.GetChild(0).gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = start_text+stat.ToString();
    }
}
