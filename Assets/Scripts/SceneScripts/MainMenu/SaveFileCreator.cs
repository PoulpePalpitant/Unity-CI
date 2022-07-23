using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Text.RegularExpressions;



/*
 * **********************************************************************
 * Project  : Flask
 * Author   : Laurent Montreuil
 * DevTeam  : Antoine-Olivier Monaco, Gabriel Kirouac, Laurent Montreuil
 * Date     : 25/04/2022

 * Brief    : Créer un ficheir de sauvegarde pour le jeu
 * **********************************************************************
*/


public class SaveFileCreator : MonoBehaviour
{
    const string EMPTY_FEEDBACK = "Please write something";
    const string JUST_LETTERS_FEEDBACK = "Hey, one word, just letters, and no latin letters(no éàç etc.)!";

    [SerializeField] TextMeshProUGUI _feedback;
    [SerializeField] Button _confirm;
    [SerializeField] TMP_InputField _profileNameInput;
    [SerializeField] GameObject _blurryBackground;


    public event Action<EventArgs> OnShow;

    public void ShowFileCreator(bool show)
    {
        gameObject.SetActive(show);
        ShowFeedBack(false);    
        OnShow?.Invoke(EventArgs.Empty);
    }

    public void ConfirmSaveFileCreation()
    {
        var text = _profileNameInput.text;
        if(text == "")
        {
            ShowFeedBack(true, EMPTY_FEEDBACK);
            return;
        }

        // ref : https://stackoverflow.com/questions/1181419/verifying-that-a-string-contains-only-letters-in-c-sharp
        if (!Regex.IsMatch(text, @"^[a-zA-Z ]+$")){
            ShowFeedBack(true, JUST_LETTERS_FEEDBACK);
            return;
        }

        SaveManager.CreateNewSaveFile(text);
        ShowFileCreator(false);
    }

    void ShowFeedBack(bool show, string feedback = "")
    {
        _feedback.gameObject.SetActive(show);
        _feedback.text = feedback;
    }
}
