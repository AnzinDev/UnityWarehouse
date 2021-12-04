using UnityEngine;
using UnityEngine.UI;
using ZXing;
using Assets;
using System.Collections;
using System.Threading.Tasks;
using System;

public class QRdetector : MonoBehaviour
{
    public Camera ReaderCamera;
    public bool EnableDebugInLog;

    [Range(0, 10)]
    public int RecognitionTimes;

    [Range(0.03f, 2)]
    public float Frequency;

    private ZXingDecoder zXingDecoder;
    private RestClient client;

    private string currRecognize = string.Empty;
    private string prevRecognize = string.Empty;
    private int recoTimes = 0;

    public readonly int imageWidth = 800;
    public readonly int imageHeight = 600;

    void Awake()
    {
        zXingDecoder = new ZXingDecoder(BarcodeFormat.QR_CODE);
        client = new RestClient("http://localhost:8080/");
        client.AddObserver(GetComponent<InfoPanel>());
        StartCoroutine(CoroutineCatch(Frequency, false, 3));
    }


    private IEnumerator CoroutineCatch(float frequency, bool enableDebugging, int _recoTimes)
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);

            DecodeFromFrame();
            currRecognize = zXingDecoder.resultText;

            if (enableDebugging)
            {
                Debugging.DebugInLog(currRecognize);
            }

            if (currRecognize == prevRecognize)
            {
                recoTimes++;
            }

            if (recoTimes > _recoTimes && currRecognize != null)
            {
                StartCoroutine(client.POST(currRecognize));
                recoTimes = 0;
            }
            prevRecognize = currRecognize;
        }
    }

    private void DecodeFromFrame()
    {
        var rt = new RenderTexture(imageWidth, imageHeight, 24);
        RenderTexture.active = rt;
        var image = new Texture2D(rt.width, rt.height, TextureFormat.RGB24, false);

        ReaderCamera.targetTexture = rt;
        ReaderCamera.Render();
        image.ReadPixels(new Rect(0, 0, imageWidth, imageHeight), 0, 0);
        image.Apply();

        zXingDecoder.DecodeImage(image.GetRawTextureData(), imageWidth, imageHeight);

        RenderTexture.active = null;
        ReaderCamera.targetTexture = null;

        Destroy(rt);
        Destroy(image);
    }
}
