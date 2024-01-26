using UnityEngine;
using System.Collections;

// skeleton for what defines a Muscle Option
// A often physical type of action like a punch or barging down a door
// from its unique ID, option text for its button, success display string (with any follow up text chains) and whether it unlocks a move location or item
// Like breaking a locked door allowing the player to go through it to a new location
public class MuscleOption {

	string keywordID = "";
	string successAction = "";
	string optionText = "";
	string successText = "";

	string itemUnlock = "";
	string moveUnlock = "";

	bool isChain = false;



	public MuscleOption(string folder, string keyword, string action, string option, string success, string chain)
	{
		keywordID = keyword;
		successAction = action;
		optionText = option;
		successText = success;
		isChain = (chain == "true") ? true : false;

		TextAsset itemFile = Resources.Load ( folder + keyword + " Items") as TextAsset;
		TextAsset moveFile = Resources.Load ( folder + keyword + " Move") as TextAsset;

		if (itemFile)
		{
			itemUnlock = itemFile.text;
		}

		if (moveFile)
		{
			moveUnlock = moveFile.text;
		}
	}

	public string GetKeyword()
	{
		return keywordID;
	}

	public string GetOptionDisplay()
	{
		return optionText;
	}

	public string GetSuccess()
	{
		return successText;
	}

	public string GetItemUnlock()
	{
		return itemUnlock;
	}

	public string GetMoveUnlock()
	{
		return moveUnlock;
	}

	public bool HasSucceeded(string chosenAction)
	{
		Debug.Log (chosenAction + " + " + successAction);

		return (successAction == chosenAction);
	}

	public bool HasChain()
	{
		return isChain;
	}

	public bool HasItemUnlock()
	{
		return (itemUnlock != "");
	}

	public bool HasMoveUnlock()
	{
		return (moveUnlock != "");
	}

}
