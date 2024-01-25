using UnityEngine;
using System.Collections;

public class ActionManager : MonoBehaviour {

	protected int actionID = 0;
	protected GameManager managerRef;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void AssignManager (int action, GameManager manager)
	{
		actionID = action;
		managerRef = manager;
	}
}
