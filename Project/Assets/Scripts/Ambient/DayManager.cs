using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class DayManager : MonoBehaviour
{
    public Light directionalLight;
    public LightingPreset preset;
    [Range(0, 24)] public float timeOfDay;
	public float timeSpeed;

	private void Update()
	{
		if (preset == null) return;

		if (Application.isPlaying)
		{
			timeOfDay += Time.deltaTime / 60;
			timeOfDay %= 24;
			UpdateLighting(timeOfDay / 24);
		}
		else
		{
			UpdateLighting(timeOfDay / 24);
		}
	}

	private void UpdateLighting(float timePercent)
	{
		RenderSettings.ambientLight = preset.ambientColor.Evaluate(timePercent);
		//RenderSettings.fogColor = preset.fogColor.Evaluate(timePercent);

		if (directionalLight != null)
		{
			directionalLight.color = preset.directionalColor.Evaluate(timePercent);
			directionalLight.transform.localRotation = Quaternion.Euler(new Vector3((timePercent * 360) - 90, 170, 0));
		}
	}

	private void OnValidate()
	{
		if (directionalLight != null)
			return;

		if (RenderSettings.sun != null)
		{
			directionalLight = RenderSettings.sun;
		}
		else
		{
			Light[] lights = FindObjectsOfType<Light>();
			foreach(Light light in lights)
			{
				if (light.type == LightType.Directional)
				{
					directionalLight = light;
					return;
				}
			}
		}
	}
}
