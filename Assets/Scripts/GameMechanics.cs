using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameMechanics : MonoBehaviour
{
    public static GameMechanics instance;
    [SerializeField] private GameObject choose_buttons;
    [SerializeField] private GameObject game_buttons;
    [SerializeField] private GameObject menu_buttons;
    [SerializeField] private GameObject curtain;
    public GameObject[] game_elements;
    public Sprite[] sprites_for_fields;
    public List<int> marked_space = new List<int>();

    [NonSerialized] public static bool player_turn;
    
    public static bool is_restart;
    public static int Player;//якщо обрано нулик, тоді 0. Якщо хрестик, тоді 1.
    private static int PC;

    public static bool state_move_PC = true;
    public static bool is_draw;
    private static bool victory_status;
    
    private bool end_game = false;
    
    public void RestartGame()
    {
        menu_buttons.SetActive(false);
        choose_buttons.SetActive(true);
    }

    private void Start()
    {
        Player = 0;
        PC = 0;
        
        if (instance == null)
             instance = this;

        if (is_restart)
         {
             RestartGame();
             is_restart = false;
         }
         
        if (game_elements != null)
        {
            foreach (var fields in game_elements)
            {
                fields.GetComponent<Image>().sprite = sprites_for_fields[2];
            }
        }
        
        for (int i = 0; i < 9; i++)
        {
            marked_space.Add(-20);
        }
    }
    
    public void ChooseCross()
    {
        Player = 1;
        choose_buttons.SetActive(false);
        game_buttons.SetActive(true);
        curtain.SetActive(false);
    }

    public void ChooseZero()
    {
        PC = 1;
        choose_buttons.SetActive(false);
        game_buttons.SetActive(true);
        curtain.SetActive(false);
    }

    public void LaunchGame()
    {
        menu_buttons.SetActive(false);
        choose_buttons.SetActive(true);
    }

    public void ClosingGame()
    {
        Application.Quit();
    }
    
    private void PCMove(bool state_turn)
    {
        int position = 0;
        if (state_turn)
        {
            /*for (int i = 0; i < marked_space.Count; i++)
            {
                int rand_positions = Random.Range(0, 9);
                if (game_elements[rand_positions].GetComponent<Image>().sprite != sprites_for_fields[Player] && game_elements[rand_positions].GetComponent<Image>().sprite != sprites_for_fields[PC])
                {
                    game_elements[rand_positions].GetComponent<Image>().sprite = sprites_for_fields[PC];
                    game_elements[rand_positions].GetComponent<Button>().interactable = false;
                    marked_space.RemoveAt(rand_positions);
                    marked_space.Insert(rand_positions,PC);
                    WinCheck();
                    break;
                }
            }*/
            foreach (var fields in game_elements)
            {
                if (fields.GetComponent<Image>().sprite != sprites_for_fields[Player] && fields.GetComponent<Image>().sprite != sprites_for_fields[PC])
                {
                    fields.GetComponent<Image>().sprite = sprites_for_fields[PC];
                    fields.GetComponent<Button>().interactable = false;
                    marked_space.RemoveAt(position);
                    marked_space.Insert(position,PC);
                    WinCheck();
                    break;
                }
                position++;
            }
        }

        state_move_PC = false;
        player_turn = true;
    }
    
    public void ChangeImageOnFieldsByPlayer(Button button)
    {
        int position_button = 0;
        int counter = 0;
        Image my_image = button.GetComponent<Image>();
        if (player_turn)
        {
            switch (Player)
            {
                case 0: my_image.sprite = sprites_for_fields[0]; break; 
                case 1: my_image.sprite = sprites_for_fields[1]; break;
            }
        }

        foreach (var fields in game_elements)
        {
            if (fields.name == button.name)
            {
                position_button = counter;
            }

            counter++;
        }
        marked_space.RemoveAt(position_button);
        marked_space.Insert(position_button,Player);
        button.interactable = false;
        WinCheck();
        player_turn = false;
    }
    
    public void WinCheck()
    {
        int win_variant1 = marked_space[0] + marked_space[1] + marked_space[2];
        int win_variant2 = marked_space[3] + marked_space[4] + marked_space[5];
        int win_variant3 = marked_space[6] + marked_space[7] + marked_space[8];
        int win_variant4 = marked_space[0] + marked_space[4] + marked_space[8];
        int win_variant5 = marked_space[2] + marked_space[4] + marked_space[6];
        int win_variant6 = marked_space[0] + marked_space[3] + marked_space[6];
        int win_variant7 = marked_space[1] + marked_space[4] + marked_space[7];
        int win_variant8 = marked_space[2] + marked_space[5] + marked_space[8];

        var variants = new[]
        {
            win_variant1, win_variant2, win_variant3, win_variant4, win_variant5, win_variant6, win_variant7,
            win_variant8
        };
        for (int i = 0; i < variants.Length; i++)
        {
            if (variants[i] == 3*Player)
            {
                Debug.Log("Player won");
                curtain.SetActive(true);
                state_move_PC = false;
                GameUI.instance_UI.OpenGUI(true);
                victory_status = true;
                is_draw = false;
                break;
            }

            if (variants[i] == 3 * PC)
            {
                Debug.Log("PC won");
                curtain.SetActive(true);
                GameUI.instance_UI.OpenGUI(false);
                victory_status = true;
                is_draw = false;
                break;
            }
            state_move_PC = true;
        }
        DrawCheck(victory_status);
    }

    public void DrawCheck(bool victory_status)
    {
        int counter_marked_fields = 0;
        foreach (var marked_fields in marked_space)
        {
            if (marked_fields == -20 ) 
                break;
            if (marked_fields != -20 && counter_marked_fields == 8 && victory_status == false)
            {
                //WinCheck();
                Debug.Log("Draw");
                is_draw = true;
                GameUI.instance_UI.OpenGUI(false);
                break;
            }
            counter_marked_fields++;
        }
    }
    
    void Update()
    {
        if(player_turn == false)
            PCMove(state_move_PC);
    }
}
