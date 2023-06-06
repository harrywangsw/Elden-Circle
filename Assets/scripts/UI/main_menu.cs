using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem.LowLevel;
using System.IO;
using System.Linq;

public class main_menu : MonoBehaviour
{
    public float cursor_speed;
    public List<stats> stat; 
    public List<world_details> worlds; 
    public GameObject buttons, saves, current_obj, your_name;
    world_details world;
    bool loaded = false, title = true;
    AsyncOperation asyncLoad;
    void Start()
    {
        StartCoroutine(play_music());
        worlds = new List<world_details>();
        stat = new List<stats>();
        //string path = "P:/GitHub/saves"+"/";
        string path = save_load.save_path;
        DirectoryInfo info = new DirectoryInfo(path);
        foreach (FileInfo file in info.GetFiles().OrderBy(f=>f.CreationTime).ToArray()){
            //Debug.Log(file);
            if(file.FullName.Split(".")[1]=="wor"){
                worlds.Add(save_load.Loadworld(file.FullName));
            }
            if(file.FullName.Split(".")[1]=="pl"){
                stat.Add(save_load.LoadPlayer(file.FullName));
            }
        }
        DontDestroyOnLoad(gameObject);
    }

    IEnumerator play_music(){
        yield return new WaitForSeconds(1f);
        Camera.main.gameObject.GetComponent<AudioSource>().Play();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetAxis("xboxHorizontal")!=0||Input.GetAxis("xboxVertical")!=0) Mouse.current.WarpCursorPosition((Vector2)Input.mousePosition+new Vector2(Input.GetAxis("xboxHorizontal"), -Input.GetAxis("xboxVertical"))*cursor_speed*Time.deltaTime);
        //Debug.Log(loaded.ToString());
        if(Input.anyKey){
            if((Input.GetKeyDown("backspace")||Input.GetButtonDown("xboxStart"))&&!title){
                current_obj.transform.localScale = Vector3.zero;
                buttons.transform.localScale = Vector3.one;
            }
            if(title){
                transform.GetChild(1).gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = "";
                buttons.transform.localScale = Vector3.one;
                title = false;
                buttons.transform.GetChild(0).gameObject.GetComponent<Button>().onClick.AddListener(delegate{continue_game();});
                buttons.transform.GetChild(1).gameObject.GetComponent<Button>().onClick.AddListener(delegate{show_saves();});
                buttons.transform.GetChild(2).gameObject.GetComponent<Button>().onClick.AddListener(delegate{new_player();});
            }
        }
        if(saves.transform.localScale == Vector3.zero){
            if(Input.GetAxis("xboxHorizontal")!=0||Input.GetAxis("xboxVertical")!=0) Mouse.current.WarpCursorPosition((Vector2)Input.mousePosition+new Vector2(Input.GetAxis("xboxHorizontal"), -Input.GetAxis("xboxVertical"))*cursor_speed*Time.deltaTime);
        }
    }

    public void LoadYourAsyncScene(int index){
        if(worlds[index].current_world==""){
            worlds[index].current_world = "start";
        }
        saves.transform.localScale = Vector3.zero;
        StartCoroutine(statics.load_new_world(worlds[index].current_world, worlds[index], stat[index], gameObject));
    }

    public void new_player(){
        your_name.transform.localScale = Vector3.one;
        current_obj = your_name;
        buttons.transform.localScale = Vector3.zero;
        world_details new_world = new world_details();
        worlds.Add(new_world);
        stats new_stat = new stats();
        new_stat.inv.inv.Add(new item("spear", 1, "weapon"));
        new_stat.inv.inv.Add(new item("health_potion", 3, "item"));
        stat.Add(new_stat);
        your_name.GetComponent<TMP_InputField>().onEndEdit.AddListener(delegate{start_new(your_name.GetComponent<TMP_InputField>().text);});
    }

    public void start_new(string name){
        // string path = save_load.save_path;
        // DirectoryInfo info = new DirectoryInfo(path);
        // foreach (FileInfo file in info.GetFiles().OrderBy(f=>f.CreationTime).ToArray()){
        //     //Debug.Log(file);
        //     if(file.FullName.Split(".")[0]==name){
        //         your_name.GetComponent<TMP_InputField>().placeholder.GetComponent<TMPro.TextMeshProUGUI>().text = "This name has been used already. Please come up with another.";
        //     }
        // }
        stat[worlds.Count-1].name = name;
        save_load.SavePlayer(stat[stat.Count-1]);
        save_load.Saveworld(worlds[worlds.Count-1], name);
        LoadYourAsyncScene(worlds.Count-1);
    }

    public void show_saves(){
        int i;
        transform.GetChild(1).gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = "Press backspace/B to go back";
        current_obj = saves;
        buttons.transform.localScale = Vector3.zero;
        saves.transform.localScale = Vector3.one;
        for(i=0; i<worlds.Count; i++){
            int tmp = i;
            saves.transform.GetChild(i).gameObject.name = i.ToString();
            saves.transform.GetChild(i).GetChild(0).gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = stat[i].name;
            saves.transform.GetChild(i).gameObject.GetComponent<Button>().onClick.AddListener(delegate{LoadYourAsyncScene(tmp);});
        }
    }

    public void continue_game(){
        LoadYourAsyncScene(0);
    }
}
