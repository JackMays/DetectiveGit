using UnityEngine;
using System.Collections;

public class InventoryClueManager : ObjectManager {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void ExecuteClueDisplayTarget()
	{
		managerRef.ExecuteClueDisplayAction(assignedObjectID);
	}
}
