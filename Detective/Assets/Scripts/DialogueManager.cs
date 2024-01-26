using UnityEngine;
using System.Collections;

// Class for buttons in dialogue sequences with attached dialogue choices
// passing the dialogue choice to GameManager to handle and then display with Typewriter Effect
public class DialogueManager : MonoBehaviour {

	DialogueChoice assignedDialogue;

	GameManager managerRef;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void AssignObject(DialogueChoice dialogue, GameManager manager)
	{
		assignedDialogue = dialogue;
		managerRef = manager;
	}
	//passing dialogue to GameManager to handle
	//Function for Button press
	public void PassDialogueChoice()
	{
		managerRef.ExecuteDialogueTarget(assignedDialogue);
	}
}
