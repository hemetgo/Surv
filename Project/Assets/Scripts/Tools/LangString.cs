using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LangString
{
    public string portugues;
	public string english;

	public LangString()
	{
		portugues = "";
		english = "";
	}

    public string GetString()
	{
        switch (PlayerPrefs.GetString("Lang", ""))
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

[System.Serializable]
public class LangStringArea
{
	[TextArea] public string portugues;
	[TextArea] public string english;

	public LangStringArea()
	{
		portugues = "";
		english = "";
	}

	public string GetString()
	{
		switch (PlayerPrefs.GetString("Lang", ""))
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
