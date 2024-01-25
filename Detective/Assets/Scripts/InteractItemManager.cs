using UnityEngine;
using System.Collections;

public class InteractItemManager : ObjectManager {



	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void PassInteractTarget()
	{
		// call back to gamemanager to persistantly hold selected item target
		// even when this button is cleared
		managerRef.ExecuteInteractTarget(assignedObjectID);
	}
}
