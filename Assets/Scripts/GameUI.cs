using System;
using UnityEngine.SceneManagement;
using UnityEngine;

public class GameUI : MonoBehaviour
{

    public static GameUI instance_UI = null;
    private Rect windowRect = new Rect((Screen.width - 400) / 2, (Screen.height - 600) / 2, 400, 600);
    private bool show = false;
   
    private bool win = false;

    public GUIStyle[] labelStyle;

    private void Start()
    {
        if (instance_UI == null)
            instance_UI = this;
    }

    void OnGUI()
    {
        if(show && !win)
        {
            windowRect = GUI.Window(0, windowRect, DialogWindow, ""); 
        }
        if (show && win) 
        {
            windowRect = GUI.Window(1, windowRect, DialogWindow, ""); 
        }
    }
    
    private GUIStyle NewGuiStyle()
    {
        GUIStyle guiStyle = new GUIStyle(GUI.skin.button);
        guiStyle.fontSize = 28;
        return guiStyle;
    }

    void DialogWindow(int windowID)
    {
        string text = windowID == 1 ? "New game" : "Restart";

        
        if (GameMechanics.is_draw == true)
        {
            GUI.Label(new Rect(5, 5, windowRect.width, 360), "DRAW", NewGuiStyle());
        }
        else
        {
            GUI.Label(new Rect(5, 5, windowRect.width, 360),"",labelStyle[windowID]);
        }
        
        if (GUI.Button(new Rect(5, 380, windowRect.width - 10, 70), text, NewGuiStyle()))
        {
            GameMechanics.is_restart = true;
            SceneManager.LoadScene("SampleScene");
            show = false;
        }
        
        if (GUI.Button(new Rect(5, 460, windowRect.width - 10, 70), "Exit to menu", NewGuiStyle()))
        {
            SceneManager.LoadScene("SampleScene");
            show = false;
        }
    }
    
    public void OpenGUI( bool win)
    {
        show = true;
        this.win = win;
    }
    
}
