using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Lzf; 

public class CrashChainImportExportManager : MonoBehaviour
{
    [Header("Set Parameters")]
    public string setString;
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

    public void SetExportText()
    {
        //currentCustomSet = PlayerPrefs.GetString(PuzzleLoader.currentCustomSetNameKey);

        exportText.text = CrashChainSetManager.GetSetString(currentCustomSet);
    }

    public void GenerateQR()
    {
        currentCustomSet = PlayerPrefs.GetString(PuzzleLoader.currentCustomSetNameKey);

        setString = CrashChainSetManager.GetSetString(currentCustomSet);

        if (qrEncodeController != null)
        {

            qrEncodeController.Encode(setString);
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
}
