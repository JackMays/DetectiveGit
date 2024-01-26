using UnityEngine;
using System.Collections;

// Class for buttons to select actions to perform on Items in the Inventory like Use
public class InventoryActionManager : ActionManager {
	

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	// function for button press, passing desired action
	public void ExecuteInventoryAction()
	{
		managerRef.ExecuteInventoryAction (actionID);
	}
}
