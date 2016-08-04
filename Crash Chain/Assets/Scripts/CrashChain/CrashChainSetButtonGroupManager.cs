using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CrashChainSetButtonGroupManager : MonoBehaviour
{
    public CrashChainSetManager myManager;
    public GameObject setLoadButton;
    public GameObject duplicateButton;
    public GameObject deleteButton;

    public string setName = "exmple";

	// Use this for initialization
	void Start ()
    {
        Button u = setLoadButton.GetComponent<Button>();
        LevelButton lb = setLoadButton.GetComponent<LevelButton>();
        u.onClick.AddListener( () => lb.LoadLevel(setName));
	}
	
	// Update is called once per frame
	void Update ()
    {
	
	}
}
