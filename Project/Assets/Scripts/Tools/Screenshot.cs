using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Screenshot : MonoBehaviour
{
	public string path;
	public string fileName;

	private Camera camera;

    public void TakeScreenshot()
	{
		string fullPath = path + "/" + fileName + ".png";

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

#if UNITY_EDITOR
		AssetDatabase.Refresh();
#endif
	}

}


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
		}
	}
}