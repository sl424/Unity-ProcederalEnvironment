using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScoreManager : MonoBehaviour
{
	public static int waveInfo;        // The current wave.
	public static int foesLeft;       // Foes Left to defeat before wave ends.

	Text text;                      // Reference to the Text component.


	void Awake ()
	{
		// Set up the reference.
		text = GetComponent <Text> ();

		// Reset the score.
		waveInfo = 1;
		foesLeft = 1;
	}


	void Update ()
	{
		// Set the displayed text to be the word "Score" followed by the score value.
		text.text = "Wave: " + waveInfo + " Foes: " + foesLeft;
	}
}
