using UnityEngine;
using System;
using System.Collections;

[AddComponentMenu("Miscellaneous/TakeScreenshot")]
public class TakeScreenshot : MonoBehaviour {

#if UNITY_EDITOR 
	public string keyToPress = "p";
	public string keyToPress2 = "o";
	public int resWidth = 1920;
	public int resHeight = 1080;
	
	private bool takeHiResShot = false;
	private bool takeLowResShot = false;

	public static string DirName() {
		return string.Format("{0}/../Screenshots", Application.dataPath);
	}

	public static string ScreenShotName(int width, int height) {
		return DirName()+string.Format("/{0}x{1} {2}.png", width, height, System.DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss"));
	}
	
	public void TakeHiResShot() {
		takeHiResShot = true;
	}
	
	void LateUpdate() {
		takeLowResShot |= Input.GetKeyDown(keyToPress2);
		if (takeLowResShot) {
			takeLowResShot = false;
			Application.CaptureScreenshot(ScreenShotName(0,0),3);
		}

		takeHiResShot |= Input.GetKeyDown(keyToPress);
		if (takeHiResShot) {
			RenderTexture rt = new RenderTexture(resWidth, resHeight, 24);
			camera.targetTexture = rt;
			Texture2D screenShot = new Texture2D(resWidth, resHeight, TextureFormat.RGB24, false);
			camera.Render();
			RenderTexture.active = rt;
			screenShot.ReadPixels(new Rect(0, 0, resWidth, resHeight), 0, 0);
			camera.targetTexture = null;
			RenderTexture.active = null; // JC: added to avoid errors
			Destroy(rt);
			byte[] bytes = screenShot.EncodeToPNG();
			string filename = ScreenShotName(resWidth, resHeight);

			if (!System.IO.Directory.Exists(DirName())) {
				System.IO.Directory.CreateDirectory(DirName());
			}

			System.IO.File.WriteAllBytes(filename, bytes);
			Debug.Log(string.Format("Took screenshot to: {0}", filename));
			takeHiResShot = false;
		}
	}
#endif
}