using System;
using System.Collections;
using UnityEngine;

public class ScreenshotTool : MonoBehaviour {

	private static ScreenshotTool control;

	private void Awake() {
		control = this;
	}

	#if UNITY_EDITOR
	[UnityEditor.MenuItem("NOÉ'S FUCK SHIT/Take Screenshot")]
	#endif
	public static void Capture() {
		GameObject temp = new GameObject("*SNAP*", typeof(ScreenshotTool));
		temp.GetComponent<ScreenshotTool>().StartCoroutine(control.DoCapture());
	}

	private IEnumerator DoCapture() {

		yield return new WaitForEndOfFrame();

		Texture2D screenshot = NoTransparency(ScreenCapture.CaptureScreenshotAsTexture(), Camera.main.backgroundColor);

		byte[] bytes = screenshot.EncodeToPNG();
		UnityEngine.Object.Destroy(screenshot);

		string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
		string filename = $"{path}/{Application.productName} @ {System.DateTime.Now}.png";
		System.IO.File.WriteAllBytes(filename, bytes);
		Debug.Log("screenshot taken! ;D");
		Destroy(gameObject);
	}

	public static Texture2D NoTransparency(Texture2D texture, Color transparencyColour) {
		Color col;
		for (int x = 0; x < texture.width; x++) {
			for (int y = 0; y < texture.height; y++) {
				col = texture.GetPixel(x, y);
				if (col.a != 1) {
					col = Color.Lerp(transparencyColour, col, col.a);
					col.a = 1;
					texture.SetPixel(x, y, col);
				}
			}
		}
		texture.Apply();
		return texture;
	}
}