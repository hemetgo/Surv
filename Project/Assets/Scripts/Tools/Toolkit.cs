using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Toolkit
{
    public static float FloatDistance(float a, float b)
	{
		return Mathf.Abs(a - b);
	}

	public static int IntDistance(int a, int b)
	{
		return Mathf.Abs(a - b);
	}

	public static bool RandomBool()
	{
		if (Random.value > 0.5f) return true;
		else return false;
	}

	public static bool RandomBool(float trueRate)
	{
		if (Random.value > (1 - trueRate)) return true;
		else return false;
	}

	public static List<int> GetDiffIntegers(int a, int b, int results)
	{
		List<int> intList = new List<int>();

		for (int i = 0; i < results; i++)
		{
			int num = Random.Range(a, b);

			for (int z = 0; z < intList.Count; z++)
			{
				
			}

			intList.Add(num);
		}

		return intList;
	}

	public float QuarterRound(float number)
	{
		return (float)(System.Math.Round(number * 4, System.MidpointRounding.ToEven) / 4);
	}

	//Offset x, y e z

	public IEnumerator Delay(float delay)
	{
		yield return new WaitForSeconds(delay);
	}

}
