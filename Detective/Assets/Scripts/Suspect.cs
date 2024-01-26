using UnityEngine;
using System.Collections;

// a Skeleton for defining what a Suspect is, an NPC that makes it to the Deduction Phase
// with their name and description for when they are selected
public class Suspect {

	string nameIdentifier = "";
	string description = "";


	public Suspect(string id, string desc)
	{
		nameIdentifier = id;
		description = desc;
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public string GetNameID()
	{
		return nameIdentifier;
	}

	public string GetDescription()
	{
		return description;
	}
}
