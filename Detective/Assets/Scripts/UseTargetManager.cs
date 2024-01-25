using UnityEngine;
using System.Collections;

public class UseTargetManager : ActionManager {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void PassUseLocation()
	{
		managerRef.ExecuteUseLocation(actionID);
	}

	public void ExecuteUseAction()
	{
		managerRef.ExecuteUseTarget(actionID);
	}
}
