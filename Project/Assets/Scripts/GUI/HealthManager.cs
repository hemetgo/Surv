using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthManager : MonoBehaviour
{
    public Image healthBar;
	public GameObject gameOver;

	private void Start()
	{
		FindObjectOfType<BattleController>().TakenDamage += OnTakenDamage;
		gameOver.SetActive(false);
	}

	private void OnTakenDamage(int hp)
	{
		healthBar.fillAmount = hp * 0.1f;

		if (hp <= 0)
		{
			gameOver.SetActive(true);
			Cursor.lockState = CursorLockMode.None;
		}
	}
}
