using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class CrashLinkEditor : MonoBehaviour
{
    public bool testMode = false;
    public string levelName = "LevelDefault";
    public int levelNumber = 0;
    public string setName = "DefaultSet";
    int selectIndex;
    public InputField iptSetName;
    public Text lblLevelNumber;    
    public InputField iptLevelName;
    public CrashLink myFocus;
    public Transform highlighter;
    public Transform spawnMarker;
    public CrashLink squareLinkPrefab;
    public CrashLink triLinkPrefab;
    public CrashLink hexLinkPrefab;
    public Vector2 toolbarBorders = new Vector2(0.25f, 0.2f);

    public string serialisedLevel;

    private bool spawnLinkSwitch = false;
    Vector3 origLoadPanelPos;

    public string setListString;

    int lvlMin = 0;
    int lvlMax = 11;

    private string[] customSets;
    public string setListKey = "SetList";

    public int blockFrameClickCount = 0;

    // Use this for initialization
    void Start ()
    {
        //setName = PlayerPrefs.GetString(PuzzleLoader.currentCustomSetKey);
        levelNumber = PlayerPrefs.GetInt(PuzzleLoader.currentCustomPuzzleNumberKey) - 1;        
        string rawCustomSetString = PlayerPrefs.GetString(setListKey, "");
        customSets = rawCustomSetString.Split(PuzzleLoader.setDelimiter);

        Init();
    }

    public void Init()
    {
        if (iptSetName == null)
        {
            GameObject sn = GameObject.FindGameObjectWithTag("EditorSetName");

            if (sn != null)
            {
                iptSetName = sn.GetComponent<InputField>();
                iptSetName.text = setName;
            }
        }

        if (lblLevelNumber == null)
        {
            GameObject ln = GameObject.FindGameObjectWithTag("EditorLevelNumber");

            if (ln != null)
            {
                lblLevelNumber = ln.GetComponent<Text>();
                lblLevelNumber.text = (levelNumber + 1).ToString();
            }
        }

        if (highlighter == null)
        {
            GameObject hl = GameObject.FindGameObjectWithTag("EditorHighlighter");

            if (hl != null)
                highlighter = hl.transform;
        }

        if (spawnMarker == null)
        {
            GameObject sm = GameObject.FindGameObjectWithTag("EditorSpawnMarker");

            if (sm != null)
                spawnMarker = sm.transform;
        }


        if (iptLevelName == null)
        {
            GameObject ln = GameObject.FindGameObjectWithTag("EditorLevelName");

            if (ln != null)
            {
                iptLevelName = ln.GetComponent<InputField>();
                levelName = iptLevelName.text;
            }


        }


        Invoke("LoadLevel", 0.1f);
    }

    public string GetLevelName(string setName)
    {
        return setName + ":" + levelNumber.ToString();
    }

    public string GetLevelName()
    {

        return GetLevelName(setName);
    }
	
    public string GetLevelKey()
    {
        return "lvl:" + GetLevelName();
    }

    public void ToggleTestMode()
    {
        testMode = !testMode;
    }

    /*
    BIG NOTE:
    The assumption is that each set will have a maximum of 12 levels

    */
    public void AddLevelNumber(int n)
    {
        //SaveLevel();

        levelNumber += n;

        if (levelNumber < lvlMin)
            levelNumber = lvlMax;

        if (levelNumber > lvlMax)
            levelNumber = lvlMin;


        lblLevelNumber.text = (levelNumber + 1).ToString();

        levelName = GetLevelName();

        if (PlayerPrefs.HasKey(GetLevelKey()))
            LoadLevel(levelName);

        Debug.Log("SavedLevel:" + levelName);

    }

	// Update is called once per frame
	void Update ()
    {

        highlighter.GetComponent<SpriteRenderer>().enabled = !testMode;

        

        if (Input.GetMouseButtonDown(0))
        {
            //Debug.Log(Input.mousePosition + ";" + Screen.height);
            
            //don't move behind the tool bars...
            if(Input.mousePosition.y < (Screen.height - (Screen.height * toolbarBorders.y)) &&
                Input.mousePosition.x > (Screen.width * toolbarBorders.x)
               )
            { 
                Vector3 mouseDownPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                mouseDownPos.z = highlighter.transform.position.z;

                //if they haven't clicked on any blocks, lose focus.
                if (blockFrameClickCount == 0)
                    myFocus = null;

                if(myFocus == null)
                {
                    highlighter.transform.position = mouseDownPos;
                    highlighter.GetComponent<SmoothSnap>().SetAnchorGridCoordinatesOnPos();
                    highlighter.GetComponent<SmoothSnap>().noSnapOverride = false;
                }
            }
        }


        if (myFocus != null && highlighter != null)
        {
            highlighter.transform.position = myFocus.transform.position;
            highlighter.GetComponent<SmoothSnap>().noSnapOverride = true;

            if (spawnLinkSwitch)
            {
                if (Input.GetMouseButtonUp(0))
                {
                    myFocus.GetComponent<SmoothSnap>().ResetConflictSwitch();
                }
                spawnLinkSwitch = false;

                highlighter.GetComponent<SmoothSnap>().snapSwitch = true;
            }
        }

        //start a new count every frame...
        blockFrameClickCount = 0;
    }

    public void SetSetName(string newName)
    {
        setName = newName;
    }

    //change shell type of focused crash link
    public void ChangeShellType(int newType)
    {
        if(myFocus != null)
        {
            myFocus.shellType = newType;
        }
    }

    //change core type of focused crash link
    public void ChangeCoreType(int newType)
    {
        if (myFocus != null)
        {
            myFocus.coreType = newType;
        }
    }

    //only affects square links; change the direction blocks...
    public void ToggleBlockModes(int blockMode)
    {
        if (myFocus != null)
        {
            switch(blockMode)
            {
                case 0:
                    myFocus.north = !myFocus.north;
                    break;

                case 1:
                    myFocus.east = !myFocus.east;
                    break;

                case 2:
                    myFocus.west = !myFocus.west;
                    break;

                case 3:
                    myFocus.south = !myFocus.south;
                    break;
                   
            }
        }
    }

    public void ToggleMovability(bool mode)
    {
        if(myFocus != null)
        {
            myFocus.movable = mode;
            myFocus.ColourOutlines();
        }
    }

    public void ToggleMovability()
    {
        if (myFocus != null)
        {
            myFocus.movable = !myFocus.movable;
            myFocus.ColourOutlines();
        }
    }


    //handle the spawning of the crash links. 0: square, 1: triangle, 2: hex
    public void SpawnCrashLink(int linkType)
    {
        if(spawnMarker != null)
        {
            //Vector3 plantPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            //plantPos.z = 0;
            //spawnMarker.transform.position = plantPos;

            switch (linkType)
            {
                case 0:
                    if (squareLinkPrefab != null)                    
                       myFocus = Instantiate(squareLinkPrefab, spawnMarker.transform.position, Quaternion.identity) as CrashLink;
                    break;

                case 1:
                    if (triLinkPrefab != null)
                        myFocus = Instantiate(triLinkPrefab, spawnMarker.transform.position, Quaternion.identity) as CrashLink;
                    break;

                case 2:
                    if (hexLinkPrefab != null)
                        myFocus = Instantiate(hexLinkPrefab, spawnMarker.transform.position, Quaternion.identity) as CrashLink;
                    break;
            }

            /*
            myFocus.GetComponent<SmoothSnap>().StartDrag();
            myFocus.GetComponent<MouseDrag2D>().StartDrag();
            myFocus.GetComponent<MouseDrag2D>().TrackMouse();
            myFocus.GetComponent<CrashLink>().StartDrag();
            */

            spawnLinkSwitch = true;

        }

    }

    //delete the focused crash link
    public void DeleteLink()
    {
        if(myFocus !=  null)
        {
            Destroy(myFocus.gameObject);
            myFocus = null;
        }

        if (highlighter != null)
            highlighter.position = new Vector3(-100, -100, -100);
    }

    public void LoadLevel()
    {
        levelName = GetLevelName();

        LoadLevel(levelName);
    }

    //save the level to player prefs, using the levelName
    public void SaveLevel()
    {
        //retreive the level list        

        //if there are already levels in the list
        if (PlayerPrefs.HasKey(CrashChainSetManager.SetListKey))
        {

            string setList = PlayerPrefs.GetString(CrashChainSetManager.SetListKey);
            //if the level name hasn't been listed yet, add it in there... 
            if (!setList.Contains(setName))
            {
                setList = setList + ";" + setName;
                PlayerPrefs.SetString(CrashChainSetManager.SetListKey, setList);
                
            }
             
            setListString = setList;

        }
        else
        {
            //start a new level list. (no delimiter at the start or end)
            PlayerPrefs.SetString("SetList", setName);
        }

        //and now save the actual level.
        string serialisation = SerialiseLevel();

        levelName = GetLevelName();

        PlayerPrefs.SetString(GetLevelKey(), serialisation);

        //keep track of current level in editor
        PlayerPrefs.SetString("CurrentLevelEditorLevel", GetLevelKey());

        Debug.Log("Level Saved:" + levelName);

        //PopulateLoadDropdown();
    }

    //serialise the level in the crash chain format...
    public string SerialiseLevel()
    {
        /*
        See CrashChain Util.SeraliseLevel() for exact formatting rules.      
        */

        //prep the serialisation string..
        serialisedLevel = CrashChainUtil.SerialiseLevel();

        return serialisedLevel;
    }

    public void LoadLevel(string levelName)
    {
        string levelString = GetLevelKey();

        if (!PlayerPrefs.HasKey(levelString))
        {
            Debug.LogError("Load Level: NO SUCH LEVEL!! " + levelString);
            return;
        }

        if (iptLevelName != null)
            iptLevelName.text = levelName;

        serialisedLevel = PlayerPrefs.GetString(GetLevelKey());
        DeserialiseLevel(serialisedLevel);

        PlayerPrefs.SetString("CurrentLevelEditorLevel", levelName);
        Debug.Log("Level Loaded:" + levelName);
    }

    //Loads a level based on the serialisedLevel string variable..
    public void DeserialiseLevel(string serialisedLevel)
    {
        //first clear the level...
        ClearLevel();
        CrashChainUtil.DeserialiseLevel(serialisedLevel, spawnMarker, squareLinkPrefab, triLinkPrefab, hexLinkPrefab);
    }

    public void ClearLevel()
    {
        highlighter.position = new Vector3(-100, -100, -100);
        myFocus = null;

        CrashChainUtil.ClearLevel();
        
    }

    public void RelativeSwap(int sourceDelta)
    {
        int swapLvlNo = (levelNumber + sourceDelta) % 12;

        if (swapLvlNo < 0)
            swapLvlNo = lvlMax;

        Debug.Log("Relative Swap: src:" + levelNumber + " delta: " + sourceDelta + " swp:" + swapLvlNo);

        SwapLevels(setName, levelNumber, swapLvlNo);

        //levelNumber = swapLvlNo;
    }

    public void SwapLevels(string setSource, int sourceLvlNo, int destLvlNo)
    {

        //get the level keyse
        string sourceKeyName = "lvl:" + setSource + ":" + sourceLvlNo;
        string destKeyName = "lvl:" + setSource + ":" + destLvlNo;

        Debug.Log("SwapLevels: " + sourceKeyName + "," + destKeyName);

        //get the level data
        string sourceLevelString = PlayerPrefs.GetString(sourceKeyName);
        string destLevelString = PlayerPrefs.GetString(destKeyName);


        Debug.Log("SourceLevelString:" + sourceLevelString);
        Debug.Log("DestLevelString:" + destLevelString);

        //rewrite them swapped.
        PlayerPrefs.SetString(destKeyName, sourceLevelString);
        PlayerPrefs.SetString(sourceKeyName, destLevelString);

        sourceLevelString = PlayerPrefs.GetString(sourceKeyName);
        destLevelString = PlayerPrefs.GetString(destKeyName);

        Debug.Log("SourceLevelString:" + sourceLevelString);
        Debug.Log("DestLevelString:" + destLevelString);
    }
}
