using UnityEngine;
using System.Collections;

// Class for handling buttons to selecting items in Inventory to follow up with an Interact action from Inventory
public class InventoryItemManager : ObjectManager {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	// Function for Button press and passing first item selected
	public void PassInventoryTarget()
	{
		managerRef.ExecuteInventoryTarget(assignedObjectID);
	}
}
