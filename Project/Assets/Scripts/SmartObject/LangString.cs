
using UnityEngine;

[System.Serializable]
public class LangString
{
	public string english;
	public string portugues;

	public LangString()
	{
		portugues = "";
		english = "";
	}

	public string GetString()
	{
		switch (PlayerPrefs.GetString("Lang"))
		{
			case "Portugues":
				return portugues;
			case "English":
				return english;
			default:
				return english;
		}
	}
}
