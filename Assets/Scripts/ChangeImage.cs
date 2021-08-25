using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeImage : MonoBehaviour
{
    public void ChangeImageOnFields(Button button)
    {
        int position_button = 0;
        int counter = 0;
        Image my_image = button.GetComponent<Image>();
        if (GameMechanics.player_turn)
        {
            switch (GameMechanics.Player)
            {
                case 0: my_image.sprite = GameMechanics.instance.sprites_for_fields[0]; break; 
                case 1: my_image.sprite = GameMechanics.instance.sprites_for_fields[1]; break;
            }
        }

        foreach (var fields in GameMechanics.instance.game_elements)
        {
            if (fields.name == button.name)
            {
                position_button = counter;
            }

            counter++;
        }
        GameMechanics.instance.marked_space.RemoveAt(position_button);
        GameMechanics.instance.marked_space.Insert(position_button,GameMechanics.Player);
        button.interactable = false;
        GameMechanics.instance.WinCheck();
        GameMechanics.player_turn = false;
    }
}
