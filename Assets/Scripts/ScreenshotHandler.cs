using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

/// <summary>
/// This class is responsible for taking screenshots using the main camera.
/// </summary>
public class ScreenshotHandler : MonoBehaviour
{
    /// <summary>
    /// this turns true to take a screenshot
    /// </summary>
    private bool takeScreenshotOnFrameEnd;

    /// <summary>
    /// a static instance of the class to access it from anywhere
    /// </summary>
    private static ScreenshotHandler instance;

    /// <summary>
    /// the camera component of the GameObject
    /// </summary>
    private Camera mainCamera;

    private void Awake()
    {
        instance = this;
        mainCamera = GetComponent<Camera>();
    }

    /// <summary>
    /// setting the takeScreenshotOnFrameEnd to true to take a screenshot with specified width and height
    /// </summary>
    /// <param name="width"> the height of the screenshot </param>
    /// <param name="height"> the height of the screenshot </param>
    public void TakeScreenshot(int width, int height)
    {
        mainCamera.targetTexture = RenderTexture.GetTemporary(width, height, 16);
        takeScreenshotOnFrameEnd = true;
    }

    /// <summary>
    /// a static method that takes a screenshot of the entire game window
    /// </summary>
    public static void ScreenShot()
    {
        instance.TakeScreenshot(Screen.width, Screen.height);
    }

    /// <summary>
    /// getting the render texture of the camera, to convert it into a PNG image.
    /// Then the image is sagved to the Assets folder
    /// </summary>
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
