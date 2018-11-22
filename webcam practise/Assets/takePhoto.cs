using UnityEngine;
using System.Collections;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;

public class takePhoto : MonoBehaviour
{
    public string deviceName;
    //接收返回的图片数据
    WebCamTexture tex;
    public Texture2D _tex;
    public Texture2D image2;

    private int textureWidth;
    private int textureHeight;
    private Color[][] current, previous;
    private float[][] dist;
    private float threshold;

    void Start()
    {
        textureWidth = 320;
        textureHeight = 180;
        threshold = 0.2F;

        _tex = new Texture2D(textureWidth, textureHeight);

        image2 = new Texture2D(textureWidth, textureHeight);
    }

    void OnGUI()
    {
        if (GUI.Button(new Rect(10, 20, 100, 40), "开启摄像头"))
        {
            // 调用摄像头
            StartCoroutine(start());
        }

        if (tex != null)
        {
            int d = tex.width / textureWidth;
            for (int x = 0; x < textureWidth;x++){
                for (int y = 0; y < textureHeight; y++)
                {
                    float distance = Vector3.Distance(new Vector3(tex.GetPixel(x*d,y*d).r,tex.GetPixel(x*d, y*d).g,tex.GetPixel(x*d, y*d).b) ,new Vector3(_tex.GetPixel(x,y).r,_tex.GetPixel(x, y).g,_tex.GetPixel(x, y).b));
                    //float distance = Vector3.Distance(new Vector3(tex.GetPixel(x*d,y*d).r,tex.GetPixel(x*d, y*d).g,tex.GetPixel(x*d, y*d).b) ,new Vector3(_tex.GetPixel(x*d,y*d).r,_tex.GetPixel(x*d, y*d).g,_tex.GetPixel(x*d, y*d).b));
                            if(distance> threshold){
                                image2.SetPixel(x, y, Color.black);
                            }else{
                                image2.SetPixel(x, y, Color.white);
                            }

                    _tex.SetPixel(x, y, tex.GetPixel(x*d, y*d));
                }
            }
            image2.Apply();
            _tex.Apply();

            GUI.DrawTexture(new Rect(30,30, textureWidth, textureHeight), image2,ScaleMode.ScaleToFit);
            GUI.DrawTexture(new Rect(300, 300, textureWidth, textureHeight), _tex, ScaleMode.ScaleAndCrop);
        }

        Debug.Log(tex.width);
        Debug.Log(tex.height);
        Debug.Log(textureWidth);
        Debug.Log(textureHeight);
        Debug.Log(_tex.width);
        Debug.Log(_tex.height);
    }

    public IEnumerator start()
    {
        yield return Application.RequestUserAuthorization(UserAuthorization.WebCam);
        if (Application.HasUserAuthorization(UserAuthorization.WebCam))
        {
            WebCamDevice[] devices = WebCamTexture.devices;
            deviceName = devices[0].name;
            tex = new WebCamTexture(deviceName, 280, 200);
            tex.Play();
        }
    }

}