using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class SceneContent : MonoBehaviour {

	//List<Item> itemsInLocation = new List<Item>();
	List<List<Item>> itemsAcrossScene = new List<List<Item>>();
	List<Item> useItemList = new List<Item>();
	//List<NPC> npcsInLocation = new List<NPC>();
	List<List<NPC>> npcsAcrossScene = new List<List<NPC>>();
	//List<MoveLocation> availableMoveLocs = new List<MoveLocation>();
	List<List<int>> moveLocsAcrossScene = new List<List<int>>();
	List<MoveLocation> masterMoveLocationList = new List<MoveLocation>();
	List<string> deductionItemReqList = new List<string>();
	List<string> deductionClueReqList = new List<string>();
	List<string> masterLocationNameList = new List<string>();
	List<string> introChainList = new List<string>();

	TextAsset introText;

	int locationID = 0;

	string txtSubFolder = "";
	string introSubFolder = "";
	string npcSubFolder = "";
	string itemSubFolder = "";
	string moveSubFolder = "";
	string deductSubFolder = "";


	// Use this for initialization
	void Start () 
	{
		string caseName = SceneManager.GetActiveScene().name;

		txtSubFolder = "Text/";
		introSubFolder = "Intros/" + caseName + "/";
		npcSubFolder = "NPCs/" + caseName + "/";
		itemSubFolder = "Items/" + caseName + "/";
		moveSubFolder = "Move/" + caseName + "/";
		deductSubFolder = "Deductions/" + caseName + "/";

		// item path requires extra case name to access
		string itemPath = txtSubFolder + itemSubFolder;
		string movePath = txtSubFolder + moveSubFolder;
		string npcPath = txtSubFolder + npcSubFolder;
		string deductPath = txtSubFolder + deductSubFolder;

		// Initliae master locaton list before using it to load the text assets
		InitialiseMasterList(movePath);

		InitialiseItemLists(itemPath);

		InitialiseDeductionList(deductPath);

		InitialiseNPCList(npcPath);

		// initialise travel locations of each master location
		InitialiseMoveList(movePath);

		SetCurrentLocation(0);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void InitialiseMasterList(string masterPath)
	{
		TextAsset masterList = Resources.Load (masterPath + "Master Location List") as TextAsset;
		string[] masterArray = masterList.text.Split('\n');

		for (int i = 0; i < masterArray.Length; ++i)
		{
			masterLocationNameList.Add(masterArray[i]);
		}

		for (int j = 0; j < masterLocationNameList.Count; ++j)
		{
			TextAsset travelFile = Resources.Load (masterPath + masterLocationNameList[j] + " Travel") as TextAsset;

			bool travelBool = (travelFile.text == "true") ? true : false;

			masterMoveLocationList.Add (new MoveLocation(masterLocationNameList[j], j, travelBool));
		}
	}
	// includes Items used in bboth In vestigation and Deduction
	void InitialiseItemLists(string itemPath)
	{
		int totalItemIndex = 0;

		for (int i = 0; i < masterLocationNameList.Count; ++i)
		{
			List<Item> itemsInLocation = new List<Item>();

			TextAsset itemList = Resources.Load (itemPath + masterLocationNameList[i] + " Item List") as TextAsset;

			// if there is list elements, add them, if nto just submit an empty list to avoid errors if its accessed
			if (itemList != null)
			{
				string[] itemArray = itemList.text.Split('\n');

				for (int j = 0; j < itemArray.Length; ++j)
				{
					itemsInLocation.Add(new Item(itemArray[j], itemPath, totalItemIndex));
					++totalItemIndex;
				}
			}
			
			itemsAcrossScene.Add (itemsInLocation);
		}

		/*itemsInScene.Add(new Item("Knife", txtSubFolder + itemSubFolder, 0));
		itemsInScene.Add(new Item("Bottle", txtSubFolder + itemSubFolder, 1));*/

		TextAsset useList = Resources.Load (itemPath + "Use List") as TextAsset;
		// if there is list elements, add them, if nto just submit an empty list to avoid errors if its accessed 
		//i.e in a level where no use items needed
		if (useList != null)
		{
			string[] useArray = useList.text.Split('\n');

			for (int i = 0; i < useArray.Length; ++i)
			{
				Debug.Log(useArray[i]);
				useItemList.Add (new Item(useArray[i], txtSubFolder + itemSubFolder, totalItemIndex));
				++totalItemIndex;
			}
		}

	}

	void InitialiseDeductionList(string deductPath)
	{
		TextAsset deductionItemList = Resources.Load (deductPath + "Deduction Item List") as TextAsset;
		TextAsset deductionClueList = Resources.Load (deductPath + "Deduction Clue List") as TextAsset;

		// While thgere will always be a deduction list, this is used to error check
		if (deductionItemList != null)
		{
			string[] deductionItemReqArray = deductionItemList.text.Split('\n');

			for (int i = 0; i < deductionItemReqArray.Length; ++i)
			{
				deductionItemReqList.Add(deductionItemReqArray[i]);
			}
		}

		if (deductionClueList != null)
		{
			string[] deductionClueReqArray = deductionClueList.text.Split('\n');

			for (int i = 0; i < deductionClueReqArray.Length; ++i)
			{
				deductionClueReqList.Add(deductionClueReqArray[i]);
			}
		}
	}

	void InitialiseNPCList(string npcPath)
	{
		for (int i = 0; i < masterLocationNameList.Count; ++i)
		{
			List<NPC> npcsInLocation = new List<NPC>();

			TextAsset npcFile = Resources.Load (npcPath + masterLocationNameList[i] + " NPC List") as TextAsset;
			// if there is list elements, add them, if nto just submit an empty list to avoid errors if its accessed
			if (npcFile != null)
			{
				string[] npcArray = npcFile.text.Split('\n');

				for (int j = 0; j < npcArray.Length; ++j)
				{
					npcsInLocation.Add (new NPC(npcArray[j], npcPath));
				}
			}

			npcsAcrossScene.Add(npcsInLocation);
		}

	}

	void InitialiseMoveList(string movePath)
	{
		for (int i = 0; i < masterLocationNameList.Count; ++i)
		{
			//List<MoveLocation> availableMoveLocs = new List<MoveLocation>();
			List<int> availableMoveLocIDs = new List<int>();

			TextAsset moveList = Resources.Load (movePath + masterLocationNameList[i] + " Move List") as TextAsset;
			// if there is list elements, add them, if nto just submit an empty list to avoid errors if its accessed
			if (moveList != null)
			{
				string[] moveArray = moveList.text.Split('\n');

				for (int j = 0; j < moveArray.Length; ++j)
				{
					availableMoveLocIDs.Add (GetLocationIDByName(moveArray[j]));
				}
			}

			moveLocsAcrossScene.Add (availableMoveLocIDs);
		}
	}

	int GetLocationIDByName(string location)
	{
		for (int i = 0; i < masterMoveLocationList.Count; ++i)
		{
			if (masterMoveLocationList[i].GetMoveName() == location)
			{
				return masterMoveLocationList[i].GetLocationID();
			}
		}

		return 0;
	}

	public void SetCurrentLocation(int locID)
	{
		locationID = locID;

		string introFilePath = txtSubFolder + introSubFolder + masterLocationNameList[locationID];
		
		introText = Resources.Load (introFilePath + " Intro") as TextAsset;

		int chainCount = 0;
		
		TextAsset introChainFile = Resources.Load(introFilePath + " Intro Chain " + chainCount) as TextAsset;
		
		while (introChainFile)
		{
			introChainList.Add(introChainFile.text);
			
			++chainCount;
			
			introChainFile = Resources.Load(introFilePath + " Intro Chain " + chainCount) as TextAsset;
		}
	}

	public void WipeIntroChainList()
	{
		introChainList.Clear();
	}


	public void RemoveItemByID(int itemID)
	{
		for (int i = 0; i < itemsAcrossScene[locationID].Count; ++i)
		{
			if (itemsAcrossScene[locationID][i].GetItemID() == itemID)
			{
				//itemsInLocation.RemoveAt(i);
				itemsAcrossScene[locationID].RemoveAt(i);
				return;
			}
		}
	}

	public void UnlockItems(DialogueChoice dialogue)
	{
		for (int i = 0; i < itemsAcrossScene[locationID].Count; ++i)
		{
			if (dialogue.IsItemInUnlock(itemsAcrossScene[locationID][i].GetItemName()))
			{
				//itemsInLocation[i].ItemUnlocked();
				itemsAcrossScene[locationID][i].ItemUnlocked();
			}
		}
	}

	public void UnlockItems(string item)
	{
		for (int i = 0; i < itemsAcrossScene[locationID].Count; ++i)
		{
			if (itemsAcrossScene[locationID][i].GetItemName() == item)
			{
				//itemsInLocation[i].ItemUnlocked();
				itemsAcrossScene[locationID][i].ItemUnlocked();
			}
		}
	}

	public void UnlockMoves(DialogueChoice dialogue)
	{
		for (int i = 0; i < moveLocsAcrossScene[locationID].Count; ++i)
		{
			int currentIdentifier = moveLocsAcrossScene[locationID][i]; 

			if (dialogue.IsMoveInUnlock(masterMoveLocationList[currentIdentifier].GetMoveName()))
			{
				//availableMoveLocs[i].TravelUnlocked();
				masterMoveLocationList[currentIdentifier].TravelUnlocked();
			}
		}
	}

	public void UnlockMoves(Item item)
	{
		for (int i = 0; i < moveLocsAcrossScene[locationID].Count; ++i)
		{
			int currentIdentifier = moveLocsAcrossScene[locationID][i];

			if (item.IsMoveInExamineUnlock(masterMoveLocationList[currentIdentifier].GetMoveName()))
			{
				//availableMoveLocs[i].TravelUnlocked();
				masterMoveLocationList[currentIdentifier].TravelUnlocked();
			}
		}
	}

	public void UnlockMoves(string move)
	{
		for (int i = 0; i < moveLocsAcrossScene[locationID].Count; ++i)
		{
			int currentIdentifier = moveLocsAcrossScene[locationID][i];

			if (masterMoveLocationList[currentIdentifier].GetMoveName() == move)
			{
				//availableMoveLocs[i].TravelUnlocked();
				masterMoveLocationList[currentIdentifier].TravelUnlocked();
			}
		}
	}

	public void UnlockUseItem(Item useItem)
	{
		itemsAcrossScene[locationID].Add(useItem);
	}

	public string GetTxtSubFolder()
	{
		return txtSubFolder;
	}

	public string GetSceneIntro()
	{
		return introText.text;
	}

	public string GetIntroChain(int chain)
	{
		return introChainList[chain];
	}

	public string GetItemAction(int item, int action)
	{
		// 0: Interact | 1: Examine | 2: Pickup
		if (action == 0)
		{
			//return itemsInLocation[item].GetInteractText();
			return itemsAcrossScene[locationID][item].GetInteractText();
		}
		else if (action == 1)
		{
			//return itemsInLocation[item].GetExamineText();
			return itemsAcrossScene[locationID][item].GetExamineText();
		}
		else if (action == 2)
		{
			//return itemsInLocation[item].GetPickupText();
			return itemsAcrossScene[locationID][item].GetPickupText();
		}
		else
		{
			return "";
		}
	}

	public Item GetItem(int index)
	{
		//return itemsInLocation[index];
		return itemsAcrossScene[locationID][index];
	}
	public Item GetItemByID(int id)
	{
		for (int i = 0; i < itemsAcrossScene.Count; ++i)
		{
			for (int j = 0; j < itemsAcrossScene[i].Count; ++j)
			{
				if (itemsAcrossScene[i][j].GetItemID() == id)
				{
					//return itemsInLocation[i];
					return itemsAcrossScene[i][j];
				}
			}
		}

		return null;
	}

	public Item GetUseItem(int index)
	{
		return useItemList[index];
	}

	public string GetDeductionRequirment(int req)
	{
		return deductionItemReqList[req];
	}

	public NPC GetNPC(int index)
	{
		//return npcsInLocation[index];
		return npcsAcrossScene[locationID][index];
	}

	public NPC GetSpecificNPC(int iIndex, int jIndex)
	{
		return npcsAcrossScene[iIndex][jIndex];
	}

	public MoveLocation GetMove(int index)
	{

		int currentIdentifier = moveLocsAcrossScene[locationID][index];

		//return availableMoveLocs[index];
		return masterMoveLocationList[currentIdentifier];
	}

	public int GetMasterLocationNameCount()
	{
		return masterLocationNameList.Count;
	}


	public int GetMoveCountInLocation()
	{
		//return availableMoveLocs.Count;
		return moveLocsAcrossScene[locationID].Count;
	}

	public int GetItemCountInLocation()
	{
		//return itemsInLocation.Count;
		return itemsAcrossScene[locationID].Count;
	}

	public int GetItemCountInScene()
	{
		return itemsAcrossScene.Count;
	}

	public int GetUseItemCount()
	{
		return useItemList.Count;
	}

	public int GetDeductionItemReqCount()
	{
		return deductionItemReqList.Count;
	}

	public int GetDeductionClueReqCount()
	{
		return deductionClueReqList.Count;
	}

	public int GetNPCCountInScene()
	{
		//return npcsInLocation.Count;
		return npcsAcrossScene[locationID].Count;
	}

	public int GetNPCCountSpecific (int index)
	{
		return npcsAcrossScene[index].Count;
	}

	public int GetIntroChainCount()
	{
		return introChainList.Count;
	}

	public bool VerifyItem(int inventoryInt)
	{
		string inventItem = GetItemByID(inventoryInt).GetItemName();

		foreach (string deductEvidence in deductionItemReqList)
		{
			if (inventItem == deductEvidence)
			{
				return true;
			}
		}

		return false;
	}

	public bool VerifyClue(string clueName)
	{
		foreach (string deductEvidence in deductionClueReqList)
		{
			if (clueName == deductEvidence)
			{
				return true;
			}
		}

		return false;
	}

	public bool HasIntroChain()
	{
		return (introChainList.Count > 0);
	}


}
