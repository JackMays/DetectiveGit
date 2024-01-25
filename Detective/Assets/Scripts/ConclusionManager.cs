using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class ConclusionManager : MonoBehaviour {

	List<string> muscleKeywords = new List<string>();
	List<string> successChainList = new List<string>();
	List<string> endingChainList = new List<string>();

	string whoDoneIt = "";
	string successText = "";
	string endingText = "";

	// Use this for initialization
	void Start () 
	{
		DontDestroyOnLoad(this.gameObject);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void InitialiseConclusion(string filePath)
	{
		TextAsset murdererFile = Resources.Load(filePath + "Murderer") as TextAsset;
		TextAsset successFile = Resources.Load(filePath + "Success") as TextAsset;
		TextAsset endingFile = Resources.Load(filePath + "Ending") as TextAsset;
		TextAsset muscleFileData = Resources.Load(filePath + "Success Muscle") as TextAsset;


		whoDoneIt = murdererFile.text;
		successText = successFile.text;
		endingText = endingFile.text;

		if (muscleFileData)
		{
			string[] muscleArray = muscleFileData.text.Split('\n');

			for (int i = 0; i < muscleArray.Length; ++i)
			{
				muscleKeywords.Add(muscleArray[i]);
			}
		}

		int chainCount = 0;

		TextAsset successChainFile = Resources.Load(filePath + "Success Text Chain " + chainCount) as TextAsset;

		while (successChainFile)
		{
			successChainList.Add(successChainFile.text);
			++chainCount;
			successChainFile = Resources.Load(filePath + "Success Text Chain " + chainCount) as TextAsset;
		}

		chainCount = 0;

		TextAsset endingChainFile = Resources.Load(filePath + "Ending Text Chain " + chainCount) as TextAsset;

		while (endingChainFile)
		{
			endingChainList.Add(endingChainFile.text);
			++chainCount;
			endingChainFile = Resources.Load(filePath + "Ending Text Chain " + chainCount) as TextAsset;

		}
	}

	public List<string> GetMuscleKeywords()
	{
		return muscleKeywords;
	}

	public List<string> GetSuccessChainList()
	{
		return successChainList;
	}

	public List<string> GetEndingChainList()
	{
		return endingChainList;
	}

	public string GetMurderer()
	{
		return whoDoneIt;
	}

	public string GetSuccess()
	{
		return successText;
	}

	public string GetEnding()
	{
		return endingText;
	}

	public bool HasMuscle()
	{
		return (muscleKeywords.Count != 0);
	}

	public bool HasSuccessChain()
	{
		return (successChainList.Count != 0);
	}
	public bool HasEndingChain()
	{
		return (endingChainList.Count != 0);
	}
}
