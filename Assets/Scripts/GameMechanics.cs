using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class GameMechanics : MonoBehaviour
{
    [SerializeField] private GameObject choose_button;
    [SerializeField] private GameObject game_button;
    [SerializeField] private GameObject menu_button;
    [SerializeField] private GameObject curtain;
    [SerializeField] private GameObject[] game_elements;
    [SerializeField] private Sprite[] sprites_for_fields;
    private List<int> _markedSpace = new List<int>();
    [SerializeField] private GameUI game_ui;

    private static bool s_playerTurn = true;

    public static bool IsRestart;
    private bool _gameOver = false;
    public static int Player;
    private static int s_pc;
    
    public static bool IsDraw;
    private static bool s_victoryStatus = false;
    private const int _CountGameFields = 9;
    private const int _NumberToWriteForFields = -20;
    private const int _NumberLapsCycle = 100;
    private const int _CountFieldsForWin = 3;
    
    
    public void RestartGame()
    {
        menu_button.SetActive(false);
        choose_button.SetActive(true);
    }

    private void Start()
    {
        Player = 0;
        s_pc = 0;
        s_victoryStatus = false;
        
        if (IsRestart)
        {
            RestartGame();
            IsRestart = false;
        }
         
        if (game_elements != null)
        {
            foreach (var fields in game_elements)
            {
                fields.GetComponent<Image>().sprite = sprites_for_fields[2];
            }
        }
        
        for (int i = 0; i < _CountGameFields; i++)
        {
            _markedSpace.Add(_NumberToWriteForFields);
        }
    }
    
    public void ChooseCross()
    {
        Player = 1;
        choose_button.SetActive(false);
        game_button.SetActive(true);
        curtain.SetActive(false);
        s_playerTurn = false;
    }

    public void ChooseZero()
    {
        s_pc = 1;
        choose_button.SetActive(false);
        game_button.SetActive(true);
        curtain.SetActive(false);
        s_playerTurn = false;
    }

    public void LaunchGame()
    {
        menu_button.SetActive(false);
        choose_button.SetActive(true);
    }

    public void ClosingGame()
    {
        Application.Quit();
    }
    
    private void PCMove(bool stateTurn)
    {
        if (stateTurn == false && _gameOver == false)
        {
            for (int i = 0; i < _NumberLapsCycle; i++)
            {
                int randPositions = Random.Range(0, _CountGameFields);
                if (game_elements[randPositions].GetComponent<Image>().sprite == sprites_for_fields[2])
                {
                    game_elements[randPositions].GetComponent<Image>().sprite = sprites_for_fields[s_pc];
                    game_elements[randPositions].GetComponent<Button>().interactable = false;
                    _markedSpace.RemoveAt(randPositions);
                    _markedSpace.Insert(randPositions,s_pc);
                    WinCheck();
                    break;
                }
            }
        }
        s_playerTurn = true;
    }
    
    public void ChangeImageOnFieldsByPlayer(Button button)
    {
        int positionButton = 0;
        int counter = 0;
        Image myImage = button.GetComponent<Image>();
        if (s_playerTurn)
        {
            switch (Player)
            {
                case 0: myImage.sprite = sprites_for_fields[0]; break; 
                case 1: myImage.sprite = sprites_for_fields[1]; break;
            }
            foreach (var fields in game_elements)
            {
                if (fields.name == button.name)
                {
                    positionButton = counter;
                }
                counter++;
            }
            _markedSpace.RemoveAt(positionButton);
            _markedSpace.Insert(positionButton,Player);
            button.interactable = false;
            WinCheck();
            s_playerTurn = false;
        }
    }
    
    public void WinCheck()
    {
        int winVariant1 = _markedSpace[0] + _markedSpace[1] + _markedSpace[2];
        int winVariant2 = _markedSpace[3] + _markedSpace[4] + _markedSpace[5];
        int winVariant3 = _markedSpace[6] + _markedSpace[7] + _markedSpace[8];
        int winVariant4 = _markedSpace[0] + _markedSpace[4] + _markedSpace[8];
        int winVariant5 = _markedSpace[2] + _markedSpace[4] + _markedSpace[6];
        int winVariant6 = _markedSpace[0] + _markedSpace[3] + _markedSpace[6];
        int winVariant7 = _markedSpace[1] + _markedSpace[4] + _markedSpace[7];
        int winVariant8 = _markedSpace[2] + _markedSpace[5] + _markedSpace[8];

        var winVariants = new[]
        {
            winVariant1, winVariant2, winVariant3, winVariant4, winVariant5, winVariant6, winVariant7,
            winVariant8
        };
        for (int i = 0; i < winVariants.Length; i++)
        {
            if (winVariants[i] == _CountFieldsForWin*Player)
            {
                Debug.Log("Player won");
                curtain.SetActive(true);
                s_playerTurn = false;
                _gameOver = true;
                game_ui.OpenGUI(true);
                s_victoryStatus = true;
                IsDraw = false;
                break;
            }

            if (winVariants[i] == _CountFieldsForWin * s_pc)
            {
                Debug.Log("PC won");
                curtain.SetActive(true);
                game_ui.OpenGUI(false);
                s_victoryStatus = true;
                IsDraw = false;
                break;
            }
        }
        DrawCheck(s_victoryStatus);
    }

    private void DrawCheck(bool victoryStatus)
    {
        int counterMarkedFields = 0;
        foreach (var markedFields in _markedSpace)
        {
            if (markedFields != _NumberToWriteForFields)
            {
                counterMarkedFields++;
            }
        }

        if (counterMarkedFields == _CountGameFields && victoryStatus == false)
        {
            Debug.Log("Draw");
            IsDraw = true;
            game_ui.OpenGUI(false);
            s_playerTurn = false;
        }
    }

    private void FixedUpdate()
    {
        if(s_playerTurn == false)
            PCMove(s_playerTurn);
    }
}
