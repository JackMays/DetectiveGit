using UnityEngine;
using System.Collections;

public class InteractActionManager : ActionManager {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}



	public void ExecuteInteractAction()
	{
		managerRef.ExecuteInteractAction (actionID);
	}
}
