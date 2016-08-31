using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityStandardAssets.Utility;

public class PuzzleMenuGenerator : MonoBehaviour
{
    //set settings
    [Header("Set Settings")]
    public int setNumber = 1;
    public int maxSet = 12;
    public bool checkLocked = true;

    [Header ("Custom Set Management")]
    public bool customMode = false;
    public string customLevelLoader = "CustomLevelLoader";
    public string setName;
    public string setListKey = "SetList";
    public char setDelimiter = ';';
    private bool emptyMode = false;

    //display stuff
    [Header("Display Settings")]
    public Text textDisplay;
    public string prefix = "Set: ";

    //layout stuff
    [Header("Grid Layout")]
    public GameObject template;
    public Transform startPoint;
    public float xDistance = 3;
    public float yDistance = 3;
    public int colCount = 3;
    public int rowCount = 4;

    //movement stuff
    [Header("Movement Settings")]
    public AutoMoveAndRotate autoMover;
    public float moveAcceleration = 2;
    public float xSpeed;    
    public float xMax = 1000;
    public float xMin = -1000;
    public float buttonGenDelay = 0.05f;
    public bool loopSets = true;
    public GameObject leftArrow;
    public GameObject rightArrow;

    private bool moveMode;
    private int direction = 1;
    Vector3 originalPos;
    private string [] customSets;    

    private string currentSetKey;

    // Use this for initialization
    void Start ()
    {
        currentSetKey = PuzzleLoader.currentSetKey;
        originalPos = transform.position;
        autoMover = GetComponent<AutoMoveAndRotate>();

        if (customMode)
        {
            currentSetKey = PuzzleLoader.currentCustomSetKey;

            string rawCustomSetString = PlayerPrefs.GetString(setListKey,"");

            if(rawCustomSetString != "")
                customSets = rawCustomSetString.Split(setDelimiter);
            else
            {
                setNumber = 1;
                emptyMode = true;

            }

            PlayerPrefs.SetInt(currentSetKey, setNumber);
            if (customSets != null)
                maxSet = customSets.Length;
            else
                maxSet = 1;

        }

        DisplayUpdate();

        if (!emptyMode)
        {
            StartCoroutine("GenerateButtonsTimed", buttonGenDelay);

            if(customMode)
                PlayerPrefs.SetString(currentSetKey, customSets[setNumber - 1]);
        }

    }


	// Update is called once per frame
	void Update ()
    {
	    if(moveMode)
        {
            xSpeed += moveAcceleration * Time.deltaTime * -direction;

            if(transform.position.x < originalPos.x + xMax ^ transform.position.x > originalPos.x + xMin)
            {
                xSpeed = 0;
                moveMode = false;
                transform.position = originalPos;
                DeleteButtons();
                GenerateButtons();
                
            }

            autoMover.moveUnitsPerSecond.value.x = xSpeed;
            DisplayUpdate();
        }
	}

    public void DisplayUpdate()
    {
        setNumber = PlayerPrefs.GetInt(currentSetKey);

        if (setNumber <= 0)
        {
            setNumber = 1;
            PlayerPrefs.SetInt(currentSetKey, 1);
        }

        if (!customMode)
            textDisplay.text = prefix + setNumber.ToString();
        else if (!emptyMode)
            textDisplay.text = setNumber.ToString() + " : " + customSets[setNumber - 1];
        else
            textDisplay.text = "NO CUSTOM PUZZLES FOUND";

        if(!loopSets)
        {   
            if (leftArrow != null)
                leftArrow.SetActive(true);

            if (rightArrow != null)
                rightArrow.SetActive(true);

            if (setNumber == 1)
            {
                if (leftArrow != null)
                    leftArrow.SetActive(false);
            }

            if (setNumber == maxSet)
            {
                if (rightArrow != null)
                    rightArrow.SetActive(false);
            }
        }
    }


    public void SetMod(int dir)
    {
        if(dir > 0)
        {
            SetChange(1);
        }

        if(dir < 0)
        {
            SetChange(-1);
        }
    }

    void SetChange(int i)
    {
        
        setNumber += i;
        moveMode = true;
        direction = i;
        

        //internal m enu management..
        if (setNumber < 1)
        {
            if (loopSets)
                setNumber = maxSet;
            else
            {
                setNumber = 1;
                moveMode = false;                
            }
        }

        

        if (setNumber > maxSet)
        {
            if (loopSets)
                setNumber = 1;
            else
            {
                setNumber = maxSet;
                moveMode = false;

                
            }
        }

        Debug.Log("SET CHANGE:" + i + "," + setNumber + "," + currentSetKey);

        //scene management
        PlayerPrefs.SetInt(currentSetKey, setNumber);
    }

    public void GenerateButtons()
    {
        if(!emptyMode)
            StartCoroutine("GenerateButtonsTimed", buttonGenDelay);
    }

    IEnumerator GenerateButtonsTimed(float waitTime)
    {
        Vector3 spawnPos = startPoint.position;

        int puzzleNumber = 1;

        for (int i = 0; i < rowCount; i++)
        {
            for (int j = 0; j < colCount; j++)
            {
                //place the button
                GameObject b = Instantiate(template, spawnPos, startPoint.rotation) as GameObject;
                b.transform.SetParent(transform);

                //set the button settings
                PuzzleLoader pl = b.GetComponent<PuzzleLoader>();

                if (pl != null)
                {
                    pl.setNumber = setNumber;
                    pl.puzzleNumber = puzzleNumber;

                    //if loading custom puzzles...
                    if (customMode)
                    {
                        //don't follow the standard numbering system..
                        pl.customMode = true;
                        pl.customSetName = customSets[setNumber - 1];

                        //and always unlock the first one
                        if (puzzleNumber == 1)
                        {
                            pl.locked = false;
                            pl.checkLocked = false;
                        }

                        //set the custom loader
                        pl.customLevelLoader = customLevelLoader;
                    }

                    if(!checkLocked)
                    {
                        pl.locked = false;
                        pl.checkLocked = false;
                    }

                    Transform label = pl.transform.FindChild("Text");

                    if (label != null)
                    {
                        label.GetComponent<Text>().text = puzzleNumber.ToString();
                    }
                }

                spawnPos.x += xDistance;
                puzzleNumber++;

                //Debug.Log("WOO");

                yield return new WaitForSeconds(waitTime);
            }
            spawnPos.y += yDistance;
            spawnPos.x = startPoint.position.x;
        }
    }

    void DeleteButtons()
    {
        foreach(Transform child in transform)
        {
            Destroy(child.gameObject);
        }
    }
}
