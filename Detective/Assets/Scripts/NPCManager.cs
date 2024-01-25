using UnityEngine;
using System.Collections;

public class NPCManager : ObjectManager {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void PassTalkTarget()
	{
		managerRef.ExecuteTalkTarget(assignedObjectID);
	}
}
