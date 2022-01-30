using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class Screenshot : MonoBehaviour
{
	public string path;
	public Image square;

	private Camera camera;

	private void Start()
	{
		Destroy(gameObject);
	}

#if UNITY_EDITOR
	public void TakeScreenshot()
	{
		square.gameObject.SetActive(false);

		// Screenshot
		ItemData itemData = GetComponentInChildren<DropItem>().item.itemData;
		string fullPath = path + "/" + itemData.itemType + "/" + itemData.itemName.english + ".png";

		if (camera == null)
			camera = GetComponent<Camera>();

		RenderTexture rt = new RenderTexture(256, 256, 24);
		camera.targetTexture = rt;
		Texture2D screenShot = new Texture2D(256, 256, TextureFormat.RGBA32, false);
		camera.Render();
		RenderTexture.active = rt;
		screenShot.ReadPixels(new Rect(0, 0, 256, 256), 0, 0);
		camera.targetTexture = null;
		RenderTexture.active = null;

		if (Application.isEditor) DestroyImmediate(rt);
		else Destroy(rt);

		byte[] bytes = screenShot.EncodeToPNG();
		System.IO.File.WriteAllBytes(fullPath, bytes);

		// Texture type 
		AssetDatabase.ImportAsset(fullPath);
		TextureImporter importer = AssetImporter.GetAtPath(fullPath) as TextureImporter;
		importer.textureType = TextureImporterType.Sprite;
		AssetDatabase.WriteImportSettingsIfDirty(path);

		
		AssetDatabase.Refresh();

		
		// enable the square pic helper
		square.gameObject.SetActive(true);
	}

	public void UpdateItemSprite()
	{
		ItemData itemData = GetComponentInChildren<DropItem>().item.itemData;

		// Set the item icon 
		itemData.icon = Resources.Load<Sprite>("ItemSprites/" + itemData.itemType.ToString() + "/" + itemData.itemName.english);
	}
#endif
}

#if UNITY_EDITOR
[CustomEditor(typeof(Screenshot))]
public class ScreenshotInspector : Editor
{
	public override void OnInspectorGUI()
	{
		DrawDefaultInspector();

		Screenshot script = (Screenshot)target;
		if (GUILayout.Button("Take Screenshot"))
		{
			script.TakeScreenshot();
			script.TakeScreenshot();
			script.UpdateItemSprite();
		}
	}
}
#endif