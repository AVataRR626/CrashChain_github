/*
Randomly chooses test in a list to display...
Has functionality to avoid close span repitition
**WARNING: USES STATIC INDEX LIST; Do not use with multiple instances
**make sure closeRepeatAvoidance is false if using multiple instances

-Matt Cabanag 3 Oct 2016 
 
*/

using UnityEngine;
using System.Collections;
using UnityEngine.UI;   

public class RandomText : MonoBehaviour
{
    public static int[] randomIndexList;
    public static int randomIndex = -1;

    private Text txt;

    public string text; 
    public string [] textRepo;

    public bool closeRepeatAvoidance = false;

	// Use this for initialization
	void Start ()
    {
        txt = GetComponent<Text>();

        RandomiseText();

    }

    //this avoids seeing the repeat selections in a short span..
    public void InitRandomIndexList()
    {
        if(randomIndex < 0 || randomIndex >= randomIndexList.Length)
        {
            randomIndex = 0;

            randomIndexList = new int[textRepo.Length];

            for(int i = 0; i < randomIndexList.Length; i++)
            {
                randomIndexList[i] = i;
            }

            Shuffle <int> (ref randomIndexList);

            string content = "";

            for(int i = 0; i < randomIndexList.Length; i++)
            {
                content += randomIndexList[i] + ",";
            }

            Debug.Log(content);
        }

        
    }
	
	public void RandomiseText()
    {

        if (closeRepeatAvoidance)
        {
            InitRandomIndexList();
            text = textRepo[randomIndexList[randomIndex]];
            randomIndex++;
        }
        else
            text = textRepo[Random.Range(0, textRepo.Length - 1)];

        if (txt != null)
        {
            txt.text = text;
        }
        
    }

    //modified from:
    //http://stackoverflow.com/questions/108819/best-way-to-randomize-an-array-with-net
    public static void Shuffle<T>(ref T[] array)
    {
        int n = array.Length;
        while (n > 1)
        {
            n--;
            int k = Random.Range(0,n);
            T temp = array[n];
            array[n] = array[k];
            array[k] = temp;
        }
    }
}
