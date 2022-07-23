using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionHandlerTest : MonoBehaviour
{
    public bool test;
    public InfoBoxManager<TextBox, TextBoxContent> boxMaker;

    private void Update()
    {
        if (!test || !Application.isPlaying)
            return;

        boxMaker.RegenerateBox(new TextBoxContent("this be test"));
        test = false;
    }


}
