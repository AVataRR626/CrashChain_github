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

    [Header("Debug")]
    public Text debugTxt;

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

    public void ImportButton()
    {
        string newSetName = "ImportedSet_" + System.DateTime.Now.ToString("yymmddHHmmss");

        if (CrashChainSetManager.ValidSetString(setString))
        {
            CrashChainSetManager.ImportSet(setString, newSetName);
        }
        else
        {
            UiText.text = "Invalid Set Data";
        }
    }

    public string CompressCustomSetString()
    {
        currentCustomSet = PlayerPrefs.GetString(PuzzleLoader.currentCustomSetNameKey);
        setString = CrashChainSetManager.GetSetString(currentCustomSet);

        Debug.Log(currentCustomSet + ";" + setString.Length);

        byte[] compByte = Zip(setString);

        char[] compChar = new char[compByte.Length];

        for(int i = 0; i < compByte.Length; i++)
        {
            compChar[i] = (char)compByte[i];
        }

        string compString = new string(compChar);
        compressedSetString = compString;

        return compString;
    }

    public string DecompressCustomSetString()
    {
        char [] compChar = compressedSetString.ToCharArray();
        byte [] compByte = new byte[compChar.Length];

        for(int i = 0; i < compByte.Length; i++)
        {
            compByte[i] = (byte) compChar[i];
        }

        return Unzip(compByte);
    }

    public void GenerateQR()
    {   
        CompressCustomSetString();
        
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
            debugTxt.text = compressedSetString.Length + ";" + setString.Length;
        }
        else
        {

        }
    }

    void qrScanFinished(string dataText)
    {
        //UiText.text = dataText;
        UiText.text = dataText.Length.ToString();

        compressedSetString = dataText;
        setString = DecompressCustomSetString();

        Debug.Log(compressedSetString.Length + ";" + setString.Length);

        if (resetBtn != null)
        {
            resetBtn.SetActive(true);
        }

        if (scanLineObj != null)
        {
            scanLineObj.SetActive(false);
        }

        if (!CrashChainSetManager.ValidSetString(setString))
        {
            UiText.text = "Invalid Set Data";
        }
        else
        {
            UiText.text = "Scan successful!! Ready to import.";
        }
    }

    public void Reset()
    {
        if (e_qrController != null)
        {
            e_qrController.Reset();
        }

        if (UiText != null)
        {
            UiText.text = "";
        }

        if (resetBtn != null)
        {
            resetBtn.SetActive(false);
        }

        if (scanLineObj != null)
        {
            scanLineObj.SetActive(true);
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
