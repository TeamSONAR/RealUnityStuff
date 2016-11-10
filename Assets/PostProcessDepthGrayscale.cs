using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;
//using OpenGL32.dll;


//so that we can see changes we make without having to run the game

[ExecuteInEditMode]
public class PostProcessDepthGrayscale : MonoBehaviour
{

    public Material mat;

    void Start()
    {
        GetComponent<Camera>().depthTextureMode = DepthTextureMode.Depth;
        //GL.
        
    }

    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        Graphics.Blit(source, destination, mat);
        //mat is the material which contains the shader
        //we are passing the destination RenderTexture to

        //Texture2D MyTex = new Texture2D(source.width, source.height);
        System.IntPtr Zoop;
        Zoop = source.GetNativeDepthBufferPtr();
        //source.depth = 16;
        //print(Zoop.ToInt32().ToString());
        print(source.depth.ToString());

        //RenderTexture.active = source;
        //MyTex.ReadPixels(new Rect(0, 0, source.width, source.height), 0, 0);
        //MyTex.Apply();
        //byte[] bytes;
        //bytes = MyTex.EncodeToPNG();
    }
}