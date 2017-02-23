using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class NewDepthThing : MonoBehaviour
{
    RenderTexture rcolor;
    RenderTexture rdepth;
    Texture2D m_Output2D;

    // Use this for initialization
    void Start()
    {
        RenderTexture rcolor = new RenderTexture(GetComponent<Camera>().pixelWidth, GetComponent<Camera>().pixelHeight, 0, RenderTextureFormat.Default);
        RenderTexture rdepth = new RenderTexture(GetComponent<Camera>().pixelWidth, GetComponent<Camera>().pixelHeight, 0, RenderTextureFormat.Depth);
        GetComponent<Camera>().depthTextureMode = DepthTextureMode.Depth;
        GetComponent<Camera>().SetTargetBuffers(rcolor.colorBuffer, rdepth.colorBuffer);
        m_Output2D = new Texture2D(GetComponent<Camera>().pixelWidth, GetComponent<Camera>().pixelHeight);
    }

    // Update is called once per frame
    void OnPostRender()
    {
        RenderTexture oldActive = RenderTexture.active;
        RenderTexture.active = rdepth;
        
        m_Output2D.ReadPixels(new Rect(0, 0, GetComponent<Camera>().pixelWidth, GetComponent<Camera>().pixelHeight), 0, 0);
        m_Output2D.Apply();

        Color pix = m_Output2D.GetPixel(0, 0);
        RenderTexture.active = oldActive;
        print(pix.ToString());
    }

    void OnDestroy()
    {
        RenderTexture oldActive = RenderTexture.active;
        RenderTexture.active = rdepth;
        m_Output2D.ReadPixels(new Rect(0, 0, GetComponent<Camera>().pixelWidth, GetComponent<Camera>().pixelHeight), 0, 0);
        m_Output2D.Apply();
        byte[] pngBytes = m_Output2D.EncodeToPNG();
        File.WriteAllBytes(Application.dataPath + "replace_shader.png", pngBytes);
        RenderTexture.active = oldActive;
        print("destroyed");
    }
}