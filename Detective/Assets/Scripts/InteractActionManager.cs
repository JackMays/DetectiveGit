using UnityEngine;
using System.Collections;

// class for handling buttons used to execute Interact Actions such as Examine
public class InteractActionManager : ActionManager {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}


	// function for button press
	public void ExecuteInteractAction()
	{
		managerRef.ExecuteInteractAction (actionID);
	}
}
