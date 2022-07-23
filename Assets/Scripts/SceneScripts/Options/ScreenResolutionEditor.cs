using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


/*
 * **********************************************************************
 * Author   : Laurent Montreuil
 * Date     : 27/02/2022

 * Brief    : Modifie la résolution du jeu
 * **********************************************************************
*/

public class ScreenResolutionEditor : MonoBehaviour
{
    public TextMeshProUGUI resLabel;
    int selectedResolution = 0;
    TMP_Dropdown _dropDown;
    Resolution[] resolutions;

    void Start()
    {
        resolutions = Screen.resolutions;

        _dropDown = GetComponent<TMP_Dropdown>();
        _dropDown.onValueChanged.AddListener(delegate
        {
            DropdownItemSelect(_dropDown);
        });



        var items = new List<string>();
        for (int i = 0; i < resolutions.Length; i++)
        {
            if (resolutions[i].Equals(Screen.currentResolution))
            {
                selectedResolution = i;
                _dropDown.value = i;

            }
            _dropDown.options.Add(new TMP_Dropdown.OptionData() { text = resolutions[i].ToString() });
        }
    }

    void DropdownItemSelect(TMP_Dropdown dropdown)
    {
        SetResolution(dropdown);
        resLabel.text = dropdown.options[dropdown.value].text;
    }

    public void SetResolution(TMP_Dropdown dropdown)
    {
        selectedResolution = dropdown.value;
        Resolution res = resolutions[selectedResolution];
        Screen.SetResolution(res.width, res.height, Screen.fullScreen);
    }
}
