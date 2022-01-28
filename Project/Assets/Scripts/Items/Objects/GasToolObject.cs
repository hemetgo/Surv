using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GasToolObject : CraftToolObject
{
    [Header("Gas")]
    public ItemData.GasType gasType;
    public GameObject gasLight;
    public float maxGas;
    public float gasDecreaseSpeed;
    public float currentGas;

    [Header("Bar")]
    public GameObject gui;
    public Image gasBar;

    private void Update()
    {
        gasBar.fillAmount = currentGas / maxGas;

        if (currentGas > 0)
        {
            if (PlayerPrefs.GetString("GameState").Equals("Resume")) currentGas -= Time.deltaTime * gasDecreaseSpeed;
            gasLight.SetActive(true);
		}
		else
		{
            currentGas = 0;
            gasLight.SetActive(false);
		}
	}

	private void FixedUpdate()
	{
        gui.SetActive(false);
    }

    public override void Interact()
    {
        FindObjectOfType<UIManager>().StartCraft(craftTool, toolLevel, this);
    }

    public override bool CanInteract(GameObject obj)
	{
        return true;
	}

    public override ObjectType GetObjectType()
    {
        return ObjectType.CraftTool;
    }

	public override string GetInteractButton()
	{
        return "Interact";
	}

    public void AddGas(float addGas)
	{
        currentGas += addGas;

        if (currentGas > maxGas) currentGas = maxGas;
    }

    public void RemoveGas(float remGas)
	{
        currentGas -= remGas;

        if (currentGas < 0) currentGas = 0;
	}



    public bool HaveGas(float gasCost)
    {
        if (currentGas >= gasCost)
        {
            return true;
        }
        return false;
    }

}
