using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Unity.IO.Compression;
using System.Text;
using System.IO;

public class CrashChainImportExportManager : MonoBehaviour
{
    [Header("Set Parameters")]
    public string setString;
    public string compressedSetString;
    public string currentCustomSet;

    [Header("Export Settings")]
    public InputField exportText;
    public QRCodeEncodeController qrEncodeController;
    public RawImage qrCodeImage;

    [Header("Import Settings")]
    public QRCodeDecodeController e_qrController;

    public Text UiText;
    public GameObject resetBtn;
    public GameObject scanLineObj;


    // Use this for initialization
    void Start ()
    {
        currentCustomSet = PlayerPrefs.GetString(PuzzleLoader.currentCustomSetNameKey);

        if (qrEncodeController != null)
        {
            qrEncodeController.onQREncodeFinished += qrEncodeFinished;//Add Finished Event
        }

        if (e_qrController != null)
        {
            e_qrController.onQRScanFinished += qrScanFinished;//Add Finished Event
        }
    }
	
	// Update is called once per frame
	void Update ()
    {
	
	}

    public string CompressCustomSetString()
    {
        setString = PlayerPrefs.GetString(currentCustomSet);

        byte[] compByte = Zip(currentCustomSet);

        char[] compChar = new char[compByte.Length];

        for(int i = 0; i < compByte.Length; i++)
        {
            compChar[i] = (char)compByte[i];
        }

        string compString = new string(compChar);
        compressedSetString = compString;

        return compString;
    }

    public void GenerateQR()
    {
        //currentCustomSetString = PlayerPrefs.GetString(PuzzleLoader.currentCustomSetNameKey);
        CompressCustomSetString();
        setString = CrashChainSetManager.GetSetString(currentCustomSet);
        Debug.Log(compressedSetString.Length + ";" + setString.Length);

        if (qrEncodeController != null)
        {
            //qrEncodeController.Encode(setString);
            qrEncodeController.Encode(compressedSetString);
        }
    }

    void qrEncodeFinished(Texture2D tex)
    {
        if (tex != null && tex != null)
        {
            qrCodeImage.texture = tex;
        }
        else
        {

        }
    }

    void qrScanFinished(string dataText)
    {
        UiText.text = dataText;
        if (resetBtn != null)
        {
            resetBtn.SetActive(true);
        }

        if (scanLineObj != null)
        {
            scanLineObj.SetActive(false);
        }
    }

    //used from: http://stackoverflow.com/questions/7343465/compression-decompression-string-with-c-sharp
    public static void CopyTo(Stream src, Stream dest)
    {
        byte[] bytes = new byte[4096];

        int cnt;

        while ((cnt = src.Read(bytes, 0, bytes.Length)) != 0)
        {
            dest.Write(bytes, 0, cnt);
        }
    }

    //used from: http://stackoverflow.com/questions/7343465/compression-decompression-string-with-c-sharp
    public static byte[] Zip(string str)
    {
        var bytes = Encoding.UTF8.GetBytes(str);

        using (var msi = new MemoryStream(bytes))
        using (var mso = new MemoryStream())
        {
            using (var gs = new GZipStream(mso, CompressionMode.Compress))
            {
                //msi.CopyTo(gs);
                CopyTo(msi, gs);
            }

            return mso.ToArray();
        }
    }

    //used from: http://stackoverflow.com/questions/7343465/compression-decompression-string-with-c-sharp
    public static string Unzip(byte[] bytes)
    {
        using (var msi = new MemoryStream(bytes))
        using (var mso = new MemoryStream())
        {
            using (var gs = new GZipStream(msi, CompressionMode.Decompress))
            {
                //gs.CopyTo(mso);
                CopyTo(gs, mso);
            }

            return Encoding.UTF8.GetString(mso.ToArray());
        }
    }
}
