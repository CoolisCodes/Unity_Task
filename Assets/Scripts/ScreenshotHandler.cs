using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class ScreenshotHandler : MonoBehaviour
{
    private bool takeScreenshotOnFrameEnd;

    private static ScreenshotHandler instance;

    public Camera mainCamera;

    private void Awake()
    {
        instance = this;
        mainCamera = GetComponent<Camera>();
    }

    public void TakeScreenshot(int width, int height)
    {
        mainCamera.targetTexture = RenderTexture.GetTemporary(width, height, 16);
        takeScreenshotOnFrameEnd = true;
    }

    public static void ScreenShot()
    {
        instance.TakeScreenshot(Screen.width, Screen.height);
    }

    private void OnPostRender()
    {
        if (takeScreenshotOnFrameEnd)
        {
            takeScreenshotOnFrameEnd = false;

            RenderTexture renderTexture = mainCamera.targetTexture;

            Texture2D renderResult = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.ARGB32, false);

            Rect rect = new Rect(0, 0, renderTexture.width, renderTexture.height);

            renderResult.ReadPixels(rect, 0, 0);

            byte[] screenshotBytes = renderResult.EncodeToPNG();

            File.WriteAllBytes(Application.dataPath + "/Screenshot.png", screenshotBytes);

            Debug.Log("Screenshot saved!");

            RenderTexture.ReleaseTemporary(renderTexture);

            mainCamera.targetTexture = null;
        }
    }
}
