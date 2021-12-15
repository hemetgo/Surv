using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerGUIManager : MonoBehaviour
{
    public Image healthBar;
	public Image staminaBar;
	public GameObject gameOver;

	private FirstPersonController fpController;

	private void Start()
	{
		FindObjectOfType<BattleController>().TakenDamage += OnTakenDamage;
		fpController = FindObjectOfType<FirstPersonController>();
		gameOver.SetActive(false);
	}

	private void Update()
	{
		staminaBar.fillAmount = fpController.currentStamina / fpController.maxStamina;

		if (fpController.currentStamina >= fpController.maxStamina) staminaBar.transform.parent.gameObject.SetActive(false);
		else staminaBar.transform.parent.gameObject.SetActive(true);
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
