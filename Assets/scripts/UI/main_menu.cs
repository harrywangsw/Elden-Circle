using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
 using UnityEngine.SceneManagement;
using TMPro;

public class main_menu : MonoBehaviour
{
    public List<stats> stat; 
    public List<world_details> worlds; 
    public GameObject buttons, saves, current_obj, your_name;
    world_details world;
    bool loaded = false, title = true;
    AsyncOperation asyncLoad;
    void Start()
    {
        worlds = new List<world_details>();
        stat = new List<stats>();
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
        }
        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(loaded.ToString());
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

    public IEnumerator LoadYourAsyncScene(int index){
        Debug.Log(index.ToString());
        if(worlds[index].world_name==""){
            worlds[index].world_name = "start";
        }
        asyncLoad = SceneManager.LoadSceneAsync(worlds[index].world_name);
        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
        saves.SetActive(false);

        //Debug.Log(GameObject.Find("player").transform.position.z);
        player_control p = GameObject.Find("player").GetComponent<player_control>();
        p.transform.position = new Vector2(worlds[index].player_pos_x, worlds[index].player_pos_y);
        Debug.Log(stat[index].inv.inv[0].Item1);
        p.player_stat = stat[index];
        p.update_stats();
        GameObject.Find("inventory_content").GetComponent<inventory_manager>().refresh_inv_menu();
        Debug.Log("finished loading scene?");
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(worlds[index].world_name));
        asyncLoad.allowSceneActivation = true;
        Destroy(gameObject);
    }

    public void new_player(){
        your_name.SetActive(true);
        current_obj = your_name;
        buttons.SetActive(false);
        world_details new_world = new world_details();
        worlds.Add(new_world);
        stat.Add(new stats());
        your_name.GetComponent<TMP_InputField>().onEndEdit.AddListener(delegate{start_new(your_name.GetComponent<TMP_InputField>().text);});
    }

    public void start_new(string name){
        stat[worlds.Count-1].name = name;
        StartCoroutine(LoadYourAsyncScene(worlds.Count-1));
    }

    public void show_saves(){
        int i;
        current_obj = saves;
        buttons.SetActive(false);
        saves.SetActive(true);
        for(i=0; i<worlds.Count; i++){
            int tmp = i;
            Debug.Log(worlds[i].world_name);
            saves.transform.GetChild(i).gameObject.name = i.ToString();
            saves.transform.GetChild(i).GetChild(0).gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = stat[i].name;
            saves.transform.GetChild(i).gameObject.GetComponent<Button>().onClick.AddListener(delegate{StartCoroutine(LoadYourAsyncScene(tmp));});
        }
    }

    public void continue_game(){
        
    }
}
