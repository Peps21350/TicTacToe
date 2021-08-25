using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameMechanics : MonoBehaviour
{
    public static GameMechanics instance = null;
    [SerializeField] private GameObject choose_buttons;
    [SerializeField] private GameObject game_buttons;
    [SerializeField] private GameObject menu_buttons;
    [SerializeField] private GameObject curtain;
    public GameObject[] game_elements;
    public Sprite[] sprites_for_fields;
    public List<int> marked_space = new List<int>();

    [NonSerialized] public static bool player_turn = true;

    public static int Player = 0;//якщо обрано нулик, тоді 0. Якщо хрестик, тоді 1.
    private static int PC = 0;

    public static bool state_turn = false;
    private bool end_game = false;

    public void ChooseCross()
    {
        Player = 1;
        choose_buttons.SetActive(false);
        game_buttons.SetActive(true);
        curtain.SetActive(false);
    }

    private void Start()
    {
        if (instance == null)
            instance = this;
        
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


    private void PCMove(bool state)
    {
        // можна генерити рандомне число і перевіряти, чи дане місце не зайняте
        int position = 0;
        if (state)
        {
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

        state_turn = false;
        player_turn = true;
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

        var variants = new int[]
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
                state_turn = false;
                //end_game = true;
                break;
            }

            if (variants[i] == PC)
            {
                Debug.Log("PC won");
                curtain.SetActive(true);
                //end_game = true;
                break;
            }
            state_turn = true;
            
        }
        DrawCheck();
    }

    public void DrawCheck()
    {
        int counter = 0;
        //if (end_game != false) return;
        foreach (var marked_fields in marked_space)
        {
            if (marked_fields == -20 ) 
                break;
            if (marked_fields != -20 && counter == 8)
                Debug.Log("Draw");
            counter++;
        }
    }



    // Update is called once per frame
    void Update()
    {
        if(!player_turn)
            PCMove(state_turn);
    }
}
