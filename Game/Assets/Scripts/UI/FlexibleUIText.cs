using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
[RequireComponent(typeof(Shadow))]
public class FlexibleUIText : FlexibleUI
{

    Text txt;
    public string Text;
    public int size;

    /* 
     * Attached text components
     * and set the skin parameters
     */
    protected override void OnSkinUI()
    {

        base.OnSkinUI();
        txt = GetComponent<Text>();


		txt = GetComponent<Text>();

		txt.font = skinData.m_font;

        if (size > 0)
            txt.fontSize = size;
        else 
		    txt.fontSize = skinData.m_fontsize;

		txt.alignByGeometry = skinData.m_alignByText;
		txt.alignment = skinData.m_Alignment;
		txt.alignment = TextAnchor.MiddleCenter;
		txt.text = Text;
        txt.verticalOverflow = skinData.verticalOverflow;
        txt.horizontalOverflow = skinData.horizontalOverflow;
    }
}
