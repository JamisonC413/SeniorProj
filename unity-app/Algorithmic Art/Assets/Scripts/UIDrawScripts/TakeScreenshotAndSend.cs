using System;
using System.Collections;
using System.Collections.Generic;
// This import is necessary for DllImport to be available.
using System.Runtime.InteropServices;
using UnityEngine;

public class TakeScreenshotAndSend : MonoBehaviour
{
    // Import the internal function that was defined in /Assets/Plugins/WebGL/React.jslib
    // Make sure that the function name and signature are the same in both of these locations.
    // See: https://react-unity-webgl.dev/docs/api/event-system#dispatching-events
    [DllImport("__Internal")]
    private static extern void SendImage(string base64Image);

    // Function to be called whenever the "take picture" button is pressed.
    public void TakePicture()
    {
        StartCoroutine(CapturePNG());
    }

    private IEnumerator CapturePNG()
    {
        // Temporarily hide the UI so it doesn't show up in the screenshot.
        GameObject.Find("UI Canvas").GetComponent<Canvas>().enabled = false;
        GameObject.Find("Brush").GetComponent<SpriteRenderer>().enabled = false;

        // Wait for end of frame before taking a screenshot.
        yield return new WaitForEndOfFrame();

        // Get screenshot as a 2D texture.
        Texture2D texture = ScreenCapture.CaptureScreenshotAsTexture();

        // Show the UI again.
        GameObject.Find("UI Canvas").GetComponent<Canvas>().enabled = true;
        GameObject.Find("Brush").GetComponent<SpriteRenderer>().enabled = true;

        Texture2D croppedTexture = new Texture2D(350, 350);
        croppedTexture.SetPixels(texture.GetPixels((int)Camera.main.WorldToScreenPoint(GameObject.Find("Display").GetComponent<SpriteRenderer>().bounds.min).x, (int)Camera.main.WorldToScreenPoint(GameObject.Find("Display").GetComponent<SpriteRenderer>().bounds.min).y, 350, 350));
        croppedTexture.Apply();

        // Convert this texture to a PNG in a byte representation.
        byte[] bytes = ImageConversion.EncodeToPNG(croppedTexture);

        // Destroy texture now that we're finished with it.
        UnityEngine.Object.Destroy(croppedTexture);

        // Convert the PNG in byte-form to a base64 string.
        string base64Image = Convert.ToBase64String(bytes);

        // Call the imported SendImage() function and pass it the base64 string we have created.
        //
        // Some notes:
        // - This code is only compiled if the game is running in a WebGL environment;
        //   denoted by the #if preprocessor directive.
        // - This must be done to prevent compiler errors since WebGL functions are
        //   unavailable when running the game in the Unity Editor.
#if UNITY_WEBGL == true && UNITY_EDITOR == false
            SendImage(base64Image);
#endif
    }
}