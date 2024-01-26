using UnityEngine;
using System.Collections;

// Class for handling buttons in Deduction Scene with deduced suspects
// Deduced suspects only have a descriptor for GameManager to display in the deduction phase where their use appears when the player accuses them after deduction phase
public class DeductSuspDisplayManager : MonoBehaviour {

	GameManager gameManagerRef;

	string textToDisplay;

	// Use this for initialization
	void Start () {

		gameManagerRef = GameObject.FindGameObjectWithTag("Game Manager").GetComponent<GameManager>();
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void AssignManager (string text)
	{
		textToDisplay = text;
	}
	// Giving GameManager suspect text to display
	// Function for Button Press
	public void DisplayText()
	{
		gameManagerRef.TypeEventTextWithWipe(textToDisplay);
	}
}
