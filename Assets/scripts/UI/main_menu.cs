using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
 using UnityEngine.SceneManagement;

public class main_menu : MonoBehaviour
{
    public List<inventory> inventorys; 
    public List<stats> stat; 
    public List<world_details> worlds; 
    public GameObject buttons, saves, current_obj, player_editor;
    world_details world;
    bool loaded = false, title = true;
    AsyncOperation asyncLoad;
    void Start()
    {
        worlds = new List<world_details>();
        stat = new List<stats>();
        inventorys = new List<inventory>();
        //string path = "P:/GitHub/saves"+"/";
        string path = save_load.save_path;
        foreach (string file in System.IO.Directory.GetFiles(path)){
            Debug.Log(file);
            if(file.Split(".")[1]=="wor"){
                worlds.Add(save_load.Loadworld(file));
            }
            if(file.Split(".")[1]=="pl"){
                stat.Add(save_load.LoadPlayer(file));
            }
            if(file.Split(".")[1]=="inv"){
                inventorys.Add(save_load.LoadPlayerItem(file));
            }
        }
        StartCoroutine(LoadYourAsyncScene());
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.anyKey){
            if(title){
                buttons.SetActive(true);
                title = false;
                transform.GetChild(1).gameObject.SetActive(false);
                buttons.transform.GetChild(0).gameObject.GetComponent<Button>().onClick.AddListener(delegate{continue_game();});
                buttons.transform.GetChild(1).gameObject.GetComponent<Button>().onClick.AddListener(delegate{show_saves();});
                buttons.transform.GetChild(2).gameObject.GetComponent<Button>().onClick.AddListener(delegate{new_player();});
            }
            if(Input.GetKeyDown("backspace")&&!title){
                current_obj.SetActive(false);
                buttons.SetActive(true);
            }
        }
    }

    IEnumerator LoadYourAsyncScene(){
        asyncLoad = SceneManager.LoadSceneAsync("main_scene");
        asyncLoad.allowSceneActivation = false;
        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
        Debug.Log("finished loading scene");
        loaded = true;
    }

    public void new_player(){
        player_editor.SetActive(true);
        current_obj = player_editor;
        buttons.SetActive(false);
    }

    public void show_saves(){
        int i;
        current_obj = saves;
        buttons.SetActive(false);
        saves.SetActive(true);
        for(i=0; i<worlds.Count; i++){
            saves.transform.GetChild(i).GetChild(0).gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = stat[i].name;
            saves.transform.GetChild(i).gameObject.GetComponent<Button>().onClick.AddListener(delegate{StartCoroutine(load_game(i));});
        }
    }

    public IEnumerator load_game(int index){
        Debug.Log("started loading game");
        asyncLoad.allowSceneActivation = true;
        saves.SetActive(false);
        while(!loaded){
            yield return null;
        }
        player_control p = GameObject.Find("player").GetComponent<player_control>();
        p.transform.position = new Vector2(worlds[index].player_pos_x, worlds[index].player_pos_y);
        p.player_stat = stat[index];
        p.player_items = inventorys[index];
        Debug.Log("finished?");
        SceneManager.SetActiveScene(SceneManager.GetSceneByName("main_scene"));
    }

    public void continue_game(){
        
    }
}
