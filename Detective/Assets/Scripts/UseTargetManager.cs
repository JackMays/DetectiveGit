using UnityEngine;
using System.Collections;

// Class for handling buttons that select Use Location (Selecting tgo Use an Item with another from Inventory or one from Scene)
// Also handles the execution of the Use Action after a first and second item have been selected to use together
public class UseTargetManager : ActionManager {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	// function for button press when the button is a Use Location Button
	// passing the action ID to define which location is chosen
	public void PassUseLocation()
	{
		managerRef.ExecuteUseLocation(actionID);
	}

	// function for button press when the button is a Inventory Use or Scene Use Button
	// passing the action ID to execute the action
	public void ExecuteUseAction()
	{
		managerRef.ExecuteUseTarget(actionID);
	}
}
