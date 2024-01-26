using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

// skeleton for what defines an Item
// from its examine/pick up description, if it has any use requirement between two items to be unlocked, any potential move or clue unlocks
public class Item {

	List<string> useReqs = new List<string>();

	List<string> muscleKeywords = new List<string>();

	List<string> examineChainList = new List<string>();

	List<string> unlockExMoves = new List<string>();

	List<string> unlockExClues = new List<string>();

	string itemFilePath;

	string itemName;

	string interactText;
	string examineText;
	string pickUpText;



	int itemID;

	bool isTakeable;
	bool isActive;
	bool IsPickedUp = false;



	public Item (string na, string itemSub, int id)
	{
		itemName = na;
		itemID = id;

		itemFilePath = itemSub;

		string boolPath = itemFilePath + itemName + " Bools";
		string useReqPath = itemFilePath + itemName + " UseReq";
		string moveExPath = itemFilePath + itemName + " Examine Moves";
		string clueExPath = itemFilePath + itemName + " Examine Clue";

		TextAsset boolFile = Resources.Load(boolPath) as TextAsset;
		TextAsset useReqFile = Resources.Load(useReqPath) as TextAsset;
		TextAsset exMoveFile = Resources.Load(moveExPath) as TextAsset;
		TextAsset exClueFile = Resources.Load (clueExPath) as TextAsset;

		if (useReqFile)
		{
			string [] useReqsArray = useReqFile.text.Split('\n');

			for (int i = 0; i < useReqsArray.Length; ++i)
			{
				useReqs.Add (useReqsArray[i]);
			}
		}

		string [] boolArray = boolFile.text.Split('\n');


		isActive = (boolArray[0] == "true") ? true : false;
		isTakeable = (boolArray[1] == "true") ? true : false;

		if (exMoveFile)
		{
			string [] exMoveArray = exMoveFile.text.Split('\n');

			for (int i = 0; i < exMoveArray.Length; ++i)
			{
				unlockExMoves.Add (exMoveArray[i]);
			}
		}

		if (exClueFile)
		{
			string[] clueArray = exClueFile.text.Split('\n');

			for (int i = 0; i < clueArray.Length; ++i)
			{
				unlockExClues.Add (clueArray[i]);
			}
		}



		LoadText();
	}

	public void LoadText()
	{
		string interactFileName = itemFilePath + itemName + " Interact";
		string examineFileName = itemFilePath + itemName + " Examine";
		string pickupFileName = itemFilePath + itemName + " Pickup";

		TextAsset interactFileData = Resources.Load(interactFileName) as TextAsset;
		TextAsset examineFileData = Resources.Load(examineFileName) as TextAsset;
		TextAsset pickupFileData = Resources.Load(pickupFileName) as TextAsset;
		TextAsset muscleFileData = Resources.Load(examineFileName + " Muscle") as TextAsset;

		interactText = interactFileData.text;
		examineText = examineFileData.text;
		if (pickupFileData)
		{
			pickUpText = pickupFileData.text;
		}

		int chainCount = 0;

		TextAsset examineChainFileData = Resources.Load(examineFileName + " Chain " + chainCount) as TextAsset;

		while(examineChainFileData)
		{
			examineChainList.Add (examineChainFileData.text);
			++chainCount;

			examineChainFileData = Resources.Load(examineFileName + " Chain " + chainCount) as TextAsset;
		}

		if (muscleFileData)
		{
			string[] muscleArray = muscleFileData.text.Split('\n');

			for (int i = 0; i < muscleArray.Length; ++i)
			{
				muscleKeywords.Add (muscleArray[i]);
			}

		}
		

	}

	public void ItemPickedUp()
	{
		IsPickedUp = true;
	}

	public void ItemUnlocked()
	{
		isActive = true;
	}

	public List<string> GetClueList()
	{
		return unlockExClues;
	}

	public string GetItemName()
	{
		return itemName;
	}

	public string GetUseReq(int index)
	{
		return useReqs[index];
	}

	public int GetItemID()
	{
		return itemID;
	}

	public int GetExamineChainCount()
	{
		return examineChainList.Count;
	}

	public bool CanTake()
	{
		return isTakeable;
	}

	public bool HasBeenActivated()
	{
		return isActive;
	}

	public bool HasBeenPickedUp()
	{
		return IsPickedUp;
	}

	public bool HasMuscle()
	{
		return (muscleKeywords.Count != 0);
	}

	public bool HasExamineChain()
	{
		return (examineChainList.Count != 0);
	}

	public bool HasExamineMoveUnlocks()
	{
		if (unlockExMoves.Count > 0)
		{
			return true;
		}
		else
		{
			return false;
		}
	}

	public bool HasExamineClueUnlocks()
	{
		if (unlockExClues.Count > 0)
		{
			return true;
		}
		else
		{
			return false;
		}
	}

	public bool IsMoveInExamineUnlock(string move)
	{

		for (int i = 0; i < unlockExMoves.Count; ++i)
		{

			if (unlockExMoves[i] == move)
			{
				return true;
			}
		}

		return false;
	}




	public bool UseReqFilled()
	{
		return (useReqs.Count == 3);
	}

	public string GetInteractText()
	{
		return interactText;
	}

	public string GetExamineText()
	{
		return examineText;
	}

	public string GetExamineChainText(int index)
	{
		return examineChainList[index];
	}

	public string GetPickupText()
	{
		if (isTakeable)
		{
			return pickUpText;
		}
		else
		{
			return "You cannot pick that up.";
		}
	}

	public string GetUseText()
	{
		if (UseReqFilled())
		{
			return useReqs[2];
		}
		else
		{
			return "";
		}
	}

	public List<string> GetMuscleKeywords()
	{
		return muscleKeywords;
	}


}
