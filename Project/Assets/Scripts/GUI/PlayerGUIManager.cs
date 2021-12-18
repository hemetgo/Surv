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
	private HealthController healthController;

	private void Start()
	{
		fpController = FindObjectOfType<FirstPersonController>();
		healthController = fpController.GetComponent<HealthController>();
		healthController.UpdatedHealth += OnUpdateHealth;

		gameOver.SetActive(false);
	}

	private void Update()
	{
		staminaBar.fillAmount = fpController.currentStamina / fpController.maxStamina;

		if (fpController.currentStamina >= fpController.maxStamina) staminaBar.transform.parent.gameObject.SetActive(false);
		else staminaBar.transform.parent.gameObject.SetActive(true);
	}

	private void OnUpdateHealth(int hp)
	{
		healthBar.fillAmount = (float)hp / (float)healthController.maxHp;

		if (hp <= 0)
		{
			gameOver.SetActive(true);
			Cursor.lockState = CursorLockMode.None;
		}
	}
}
