using UnityEngine;
using System.Collections;

// Class for handling the two buttons after selecting inventory where the player chooses to look at Items or Clues they have
public class InventoryTypeManager : ActionManager {


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void ExecuteInventoryTypeTarget()
	{
		managerRef.ExecuteClueItemAction(actionID);
	}
}
