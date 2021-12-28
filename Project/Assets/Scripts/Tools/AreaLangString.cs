
using UnityEngine;

[System.Serializable]
public class AreaLangString
{
	[TextArea] public string english;
	[TextArea] public string portugues;

	public AreaLangString()
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
