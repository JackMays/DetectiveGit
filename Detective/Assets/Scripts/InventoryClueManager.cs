using UnityEngine;
using System.Collections;

// Class for handling buttons to select and display Clues in the Inventory
public class InventoryClueManager : ObjectManager {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	// function for button press to pass selected Clue to display its description
	public void ExecuteClueDisplayTarget()
	{
		managerRef.ExecuteClueDisplayAction(assignedObjectID);
	}
}
