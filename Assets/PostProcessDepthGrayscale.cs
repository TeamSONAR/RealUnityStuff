using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;
//so that we can see changes we make without having to run the game

[ExecuteInEditMode]
public class PostProcessDepthGrayscale : MonoBehaviour
{
    [DllImport("RenderingPlugin")]
    private static extern void SetTimeFromUnity(float t);

    [DllImport("RenderingPlugin")]
    private static extern System.IntPtr GetRenderEventFunc();

    [DllImport("RenderingPlugin")]
    private static extern int GetError();

    [DllImport("RenderingPlugin")]
    private static extern int GetPBVal(int num);

    [DllImport("RenderingPlugin")]
    private static extern int SetupReadPixels();

    [DllImport("RenderingPlugin")]
    private static extern void UnmapFile();

    [DllImport("RenderingPlugin")]
    private static extern void SetDepthF(int GDF);


    public Material mat;

    int GL_DEPTH_COMPONENT16 = 0x81A5;
    int GL_DEPTH_COMPONENT = 0x1902;
    int GL_RED = 0x1903;
    int GL_DEPTH_COMPONENT24 = 0x81A6;
    int GL_DEPTH_COMPONENTS = 0x8284;
    int GL_DEPTTH24_STENCIL8 = 0x88F0;
    int OldFoo = 0;
    int pwidth;

    void Start()
    {
        GetComponent<Camera>().depthTextureMode = DepthTextureMode.Depth;
        Vector3 extent = GetComponent< Camera>().ViewportToScreenPoint(new Vector3(0, 0, 0));
        print(extent.x.ToString());
        print(extent.y.ToString());
    }

    void OnEnable()
    {
        int cwidth = GetComponent<Camera>().pixelWidth;
        pwidth = cwidth;
        int cheight = GetComponent<Camera>().pixelHeight;

        print((cwidth * cheight).ToString());
        int outval = SetupReadPixels();
        print(outval.ToString());
        print("Mapping");
    }

    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        //print("onrender");
        
        Graphics.Blit(source, destination, mat);
    }

    void OnPostRender()
    {
        //print("onpostrender");
        //GL.Viewport(new Rect(0, 0, 640, 480));
        SetDepthF(0x80E1);
        SetTimeFromUnity(Time.timeSinceLevelLoad);

        GL.IssuePluginEvent(GetRenderEventFunc(), 1);
    }

    void OnDisable()
    {
        print("unmapping");
        UnmapFile();
    }
}

