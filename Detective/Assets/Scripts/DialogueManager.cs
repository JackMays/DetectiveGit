using UnityEngine;
using System.Collections;

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

	public void PassDialogueChoice()
	{
		managerRef.ExecuteDialogueTarget(assignedDialogue);
	}
}
