using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



[CreateAssetMenu(menuName = "Flexible UI Data")]
public class FlexibleUIData : ScriptableObject
{
	public Sprite buttonSprite;
	public SpriteState buttonSpriteState;

    public Font m_font;
    public int m_fontsize;
    public Color textColor;
    public TextAnchor m_Alignment;
    public bool m_alignByText;
public HorizontalWrapMode horizontalOverflow; 
public VerticalWrapMode verticalOverflow; 
}
