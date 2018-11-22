using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class thenew : MonoBehaviour {

    WebCamTexture webcamTexture;
    Color32[] data;

    void Start()
    {
        // Start web cam feed
        webcamTexture = new WebCamTexture();
        webcamTexture.Play();
        data = new Color32[webcamTexture.width * webcamTexture.height];
    }

    void Update()
    {
        if (webcamTexture.didUpdateThisFrame)
        {
            webcamTexture.GetPixels32(data);
            // Do processing of data here.
        }
    }
}
