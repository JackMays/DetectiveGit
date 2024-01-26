using UnityEngine;
using System.Collections;

// Class for handling the buttons when the player selects a suspect to accuse
// Passes suspect assigned to a button this is attached to
// Assists in starting the final sequence if accusation is successful from Game Manager
public class AccusationManager : MonoBehaviour {

	GameManager gameManagerRef;
	DeductionSceneManager deductionManagerRef;

	int assignedSuspect;

	public void AssignManager(int suspID, GameManager manager)
	{
		assignedSuspect = suspID;

		gameManagerRef = manager;
	}
	// If Accusation is correct (suspect matches murderer)
	// communicate with relevant managers to set up final scene
	// Function for button push
	public void CheckAccusation()
	{
		bool isRight = gameManagerRef.ExecuteAccusation(assignedSuspect);

		
		//  If the accusation is correct and theres no conclusion text chain to be had
		// give deductionManager the go ahead to start final muscle sequence immediately
		// prepping the Ui for a state that cant be switched out of like normal
		if (isRight && !gameManagerRef.HasConclusionChainActive())
		{
			deductionManagerRef.StartFinalMuscle();
		}
	}

	// Use this for initialization
	void Start () 
	{
		deductionManagerRef = GameObject.FindGameObjectWithTag("DeductionSceneManager").GetComponent<DeductionSceneManager>();
	}

	// Update is called once per frame
	void Update () {
	
	}
}
