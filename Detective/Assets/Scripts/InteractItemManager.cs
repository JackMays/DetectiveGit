using UnityEngine;
using System.Collections;

// Class for handling buttons which are items to be selected in the Interact Action
public class InteractItemManager : ObjectManager {



	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	// function for Button Press to pass the target selected
	public void PassInteractTarget()
	{
		// call back to gamemanager to persistantly hold selected item target
		// even when this button is cleared
		managerRef.ExecuteInteractTarget(assignedObjectID);
	}
}
