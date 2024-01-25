using UnityEngine;
using UnityEngine.UI;
using System.Collections;

// Typewriter takes a Text string given to it and types it character by character into the text component on a GameObject it accompanies
// Drives the main text display above the Buttons in Mind & Muscle
public class Typewriter : MonoBehaviour {
	

	// implemented this way so that modifying pause during runtime can be added in the future
	// such as to add slower typing on words for emphasis and then returning it to default
	float pause;
	float defaultPause = 0.1f;

	
	string fullText = "";
	string formattedText = "";
	string highlightTextColour = "yellow";

	char startHighlightFormatMarker = '_';
	char endHighlightFormatMarker = '|';

	Text textComponent;

	bool hasTyped = false;
	bool isTypeStateChanged = false;

	// Use this for initialization
	void Awake () 
	{

		pause = defaultPause;


		textComponent = GetComponent<Text>();

		textComponent.text = "";
	}

	// Update is called once per frame
	void Update () 
	{
		// Player has skipped typing effect, so fill the text box with the full text and stop typing effect
		if (Input.GetMouseButtonDown(0) && !hasTyped)
		{
			StopAllCoroutines();
			hasTyped = true;
			isTypeStateChanged = true;
			textComponent.text = fullText;
		}
	}
    // Changes all yellow format markers to yellow text or whichever colour is in the highlight colour variable
    // End Format Markers show when to close the colour command and return to white text
    void FormatFullText()
	{
		fullText = fullText.Replace(startHighlightFormatMarker.ToString(), "<color=" + highlightTextColour + ">");;
		fullText = fullText.Replace(endHighlightFormatMarker.ToString(), "</color>");
	}

	public bool IsTypewriterComplete()
	{
		return hasTyped;
	}

	// Allows for functionality to happen as typing begins or ends (alongside IsTypewriterComplete deciding which)
	// such as hiding UI and buttons when typing starts and making them reappear when it ends and the text box is full/complete
	public bool HasTypingChanged()
	{
		return isTypeStateChanged;
	}

	// returns state change to false usually at the end of state change functionality
	// ensures isTypeStateChanged and code that utilises it is only evaluated once
	public void StateChangeAddressed()
	{
		isTypeStateChanged = false;
	}

	public void HideText(bool hide)
	{
		textComponent.gameObject.SetActive(hide);
	}

	// Start Coroutine with passed text
	public void TypeInTypewriter (string textToType)
	{
		hasTyped = false;
		isTypeStateChanged = true;
		
		// Stop any typing coroutines so they dont clash
		StopAllCoroutines();
		
		// Make sure this is blank
		// set FullText and format it in case player skips the Typewriter effect
		textComponent.text = "";
		fullText = textToType;
		FormatFullText();
		
		// Start coroutine with text sent in when everything is initialised
		StartCoroutine(TypeText (textToType));

	}

	public void ManualClear()
	{
		textComponent.text = "";
	}

	// Type out text letter by letter
	IEnumerator TypeText(string textToType)
	{
		foreach (char letter in textToType.ToCharArray())
		{
			if (letter != startHighlightFormatMarker && letter != endHighlightFormatMarker)
			{
				// if no special character requirements such as highlights
				// Add the next letter
				textComponent.text += letter;
				formattedText += letter;
			}
			else
			{
                // Changes all yellow format markers to yellow text or whichever colour is in the highlight colour variable
                // End Format Markers show when to close the colour command and return to white text
                if (letter == startHighlightFormatMarker)
				{
					formattedText += "<color=" + highlightTextColour + ">";
				}
				else if (letter == endHighlightFormatMarker)
				{
					formattedText += "</color>";
					textComponent.text = formattedText;

				}
			}
			yield return new WaitForSeconds(pause);
		}

		hasTyped = true;
		isTypeStateChanged = true;
	}
}
