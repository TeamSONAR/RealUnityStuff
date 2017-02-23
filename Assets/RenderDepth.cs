using UnityEngine;
using UnityEngine.Windows;

[ExecuteInEditMode]
public class RenderDepth : MonoBehaviour
{
    [Range(0f, 3f)]
    public float depthLevel = 0.5f;

    private Shader _shader;
    private Shader shader
    {
        get { return _shader != null ? _shader : (_shader = Shader.Find("Custom/RenderDepth")); }
    }

    private Material _material;
    private Material material
    {
        get
        {
            if (_material == null)
            {
                _material = new Material(shader);
                _material.hideFlags = HideFlags.HideAndDontSave;
            }
            return _material;
        }
    }

    void RenderToTexture()
    {
        var w = 512;
        var h = w;
        var tempRt = new RenderTexture(w, h, 0);
        GetComponent<Camera>().targetTexture = tempRt;
        GetComponent<Camera>().Render();
        RenderTexture.active = tempRt;
        var tex2d = new Texture2D(w, h, TextureFormat.ARGB32, false);
        tex2d.ReadPixels(new Rect(0, 0, w, h), 0, 0);
        tex2d.Apply();
        var imageData = tex2d.EncodeToPNG();
        //Windows.File.WriteAllBytes("depth_img.png", imageData);
        RenderTexture.active = null;
        GetComponent<Camera>().targetTexture = null;
        Destroy(tempRt);
    }

    private void Start()
    {
        if (!SystemInfo.supportsImageEffects)
        {
            print("System doesn't support image effects");
            enabled = false;
            return;
        }
        if (shader == null || !shader.isSupported)
        {
            enabled = false;
            print("Shader " + shader.name + " is not supported");
            return;
        }

        // turn on depth rendering for the camera so that the shader can access it via _CameraDepthTexture
        GetComponent<Camera>().depthTextureMode = DepthTextureMode.Depth;
    }

    private void OnDisable()
    {
        if (_material != null)
            DestroyImmediate(_material);
    }

    private void OnRenderImage(RenderTexture src, RenderTexture dest)
    {
        if (shader != null)
        {
            material.SetFloat("_DepthLevel", depthLevel);
            Graphics.Blit(src, dest, material);
        }
        else
        {
            Graphics.Blit(src, dest);
        }
    }
}

