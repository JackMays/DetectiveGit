using UnityEngine;
using System.Collections;

public class InventoryActionManager : ActionManager {
	

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void ExecuteInventoryAction()
	{
		managerRef.ExecuteInventoryAction (actionID);
	}
}
