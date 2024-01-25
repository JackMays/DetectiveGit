using UnityEngine;
using System.Collections;

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

	public void DisplayText()
	{
		gameManagerRef.TypeEventTextWithWipe(textToDisplay);
	}
}
