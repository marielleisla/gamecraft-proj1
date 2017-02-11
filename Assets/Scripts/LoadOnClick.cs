using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadOnClick : MonoBehaviour {

    GameObject UI_Canvas;
    GameObject Title_Screen;

    public void Start() {
        Scene currentScene = SceneManager.GetActiveScene();
        string currentSceneName = currentScene.name;
        if (currentSceneName == "Main Scene")
        {
            SceneManager.LoadScene("Menu Scene");
            SceneManager.UnloadSceneAsync("Main Scene");
        }
        UI_Canvas = GameObject.Find("UI_Canvas");
        Title_Screen = GameObject.Find("Title Screen");
        DontDestroyOnLoad(UI_Canvas);
    }

    /* The function called when the '1Player' button is pressed. 
     It should load the 'Main Scene' and deactivate the 
     'Title_Screen' GameObject. */
    public void LoadScene()
    {
        /*
         * 
         * YOUR CODE HERE
         * 
         */ 
    }

}
