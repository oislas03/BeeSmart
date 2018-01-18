using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;


public class PNGUploader : MonoBehaviour {

    // Take a shot immediately
    IEnumerator Start()
    {
        yield return null;
    }





   public  IEnumerator UploadPNG( )
    {
       

        // We should only read the screen buffer after rendering is complete
        yield return new WaitForEndOfFrame();
        Debug.Log(1);

        // Create a texture the size of the screen, RGB24 format
        int width = Screen.width;
        int height = Screen.height;
        Texture2D tex = new Texture2D(width, height, TextureFormat.RGB24, false);

        // Read screen contents into the texture
        tex.ReadPixels(new Rect(0, 0, width, height), 0, 0);
        tex.Apply();
        Debug.Log(2);

        // Encode texture into PNG
        byte[] bytes = tex.EncodeToPNG();
        Object.Destroy(tex);
        Debug.Log(Application.dataPath + "/StreamingAssets/Screenshots/"+EstadoJuego.estadoJuego.path);
        // For testing purposes, also write to a file in the project folder
        Debug.Log(3);

        File.WriteAllBytes(Application.dataPath + "/StreamingAssets/Screenshots/" + EstadoJuego.estadoJuego.path, bytes);

        // Upload to a cgi script
        yield return null;

    }
}
