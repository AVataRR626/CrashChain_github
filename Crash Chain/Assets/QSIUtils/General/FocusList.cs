//QSI General Utils
//30 Sept 2016
//Matt Cabanag

using UnityEngine;
using System.Collections;

public class FocusList : MonoBehaviour
{
    public int focusIndex = 0;

    public GameObject[] focusList;


    public void Focus(int target)
    {
        focusIndex = target;

        for(int i = 0; i < focusList.Length; i++)
        {
            focusList[i].SetActive(i == focusIndex);
        }
    }
}
