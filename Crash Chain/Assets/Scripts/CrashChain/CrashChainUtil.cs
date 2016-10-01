using UnityEngine;
using System.Collections;
using System;

public class CrashChainUtil : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public static void ClearLevel()
    {        
        CrashLink[] allTheLinks = FindObjectsOfType<CrashLink>();
        //Debug.Log("ClearLevel:"+allTheLinks.Length);
        foreach (CrashLink l in allTheLinks)
        {
            Destroy(l.gameObject);
            //Debug.Log("Destroying:" + l.name);
        }
    }

    public static string BitSerialiseLevel()
    {
        Vector3 camPos = Camera.main.transform.position;
        float zoom = Camera.main.orthographicSize;
                
        string xCam = new string(CharByteConvert(camPos.x));
        string yCam = new string(CharByteConvert(camPos.y));
        string zCam = new string(CharByteConvert(camPos.z));
        string zm = new string(CharByteConvert(zoom));

        string serialisedLevel = xCam + yCam + zm;

        //get all the crash links and serialise them..
        CrashLink[] allTheLinks = FindObjectsOfType<CrashLink>();

        foreach (CrashLink l in allTheLinks)
        {
            serialisedLevel += l.BitSerialise();
        }
        return serialisedLevel;
    }

    //Adapted from: https://msdn.microsoft.com/en-us/library/yhwsaf3w(v=vs.110).aspx
    //Convert a float into a char array based on its bit representation...
    public static char[] CharByteConvert(float argument)
    {
        byte [] byteArr = BitConverter.GetBytes(argument);

        char[] result = new char[byteArr.Length];

        for(int i = 0; i < result.Length; i++)
        {
            result[i] = (char)byteArr[i];
        }

        return result;
    }

    public static void BitDeserialiseLevel(string serialisedLevel, Transform spawnMarker, CrashLink squareLinkPrefab, CrashLink triLinkPrefab, CrashLink hexLinkPrefab)
    {
        char[] levelBits = serialisedLevel.ToCharArray();

        Debug.Log(levelBits.Length + ";" + serialisedLevel.Length);
        //reconstruct the float values from the bytes
        float[] camResults = new float[3];
        for(int i = 0; i < 3; i++)
        {
            byte[] accumulator = new byte[4];
            for(int j = 0; j < 4; j++)
            {
                Debug.Log("camscan:" + i + ";" + j);
                accumulator[j] = (byte)levelBits[i * 4 + j];
            }
            camResults[i] = BitConverter.ToSingle(accumulator,0);
        }

        Vector3 camPos = new Vector3(0, 0, -1);

        camPos.x = camResults[0];
        camPos.y = camResults[1];
        //camPos.z = camResults[2];


        Camera.main.transform.position = camPos;
        Camera.main.orthographicSize = camResults[2];

        /*
        Vector3 camPos = new Vector3(0, 0, 0);

        camPos.x = float.Parse(camAttrs[0]);
        camPos.y = float.Parse(camAttrs[1]);
        camPos.z = float.Parse(camAttrs[2]);


        Camera.main.transform.position = camPos;
        Camera.main.orthographicSize = float.Parse(camAttrs[3]);
        */

        //okay, now onto the pieces..
        //char[] puzzlePieces = components[1].ToCharArray();

        for(int i = 12; i < levelBits.Length; i+=3)
        {
            //put the encoding in the right format for consumption..
            int[] parts = new int[3];

            for(int j = 0; j < 3; j++)
            {
                parts[j] = levelBits[i + j];
            }

            //recover the type and spawn the right one
            int type = (int)parts[0] & 3;
            CrashLink newLink = null;
            switch(type)
            {
                case 0:
                    newLink = Instantiate(squareLinkPrefab, spawnMarker.position, Quaternion.identity) as CrashLink;
                    break;

                case 1:
                    newLink = Instantiate(triLinkPrefab, spawnMarker.position, Quaternion.identity) as CrashLink;
                    break;

                case 2:
                    newLink = Instantiate(hexLinkPrefab, spawnMarker.position, Quaternion.identity) as CrashLink;
                    break;

                default:
                    newLink = Instantiate(squareLinkPrefab, spawnMarker.position, Quaternion.identity) as CrashLink;
                    break;
            }

            //get the rest of the info! (plzwork)
            newLink.BitDeserialise(parts);
        }

    }

    public static string SerialiseLevel()
    {

        /*
        Crash Chain Level Formatting

        Type :-
            Q :- square link
            T :- triangle
            H :- hex
            C :- starting camera location

        Single Entry (For Q,T,H types) :-
            ignore :- []
            data delimiter :- |
            direction blockings :-
                N :- north
                E :- east
                W :- west
                S :- south
                F :- none or free...
                * can be any order, F overrides everything...

            [Type]|[shell type (int)]|[core type (int)]|[x grid (int)]|[y grid (int)]|[direction blockings]

            example entry:
            Q|0|1|22|55|F
        
        Single Entry for C type :-
            C|[x coordinate]|[y coordinate]|[z coordinate]

            example entry:
            C|122.32|99.22|0|

        Serialisation :-
            entry delimiter :- ;

            [entry];[entry];[entry];[entry]; .... ;[entry]

            example serialisation:
            C|122.32|99.22;Q|0|1|22|55|F;Q|0|1|20|50|F;T|0|1|15|15|F;H|0|1|22|55|F        
        */

        Vector3 camPos = Camera.main.transform.position;
        float zoom = Camera.main.orthographicSize;

        string serialisedLevel = "C|"+camPos.x+"|"+camPos.y+"|"+camPos.z+"|"+zoom+";";

        //get all the crash links and serialise them..
        CrashLink[] allTheLinks = FindObjectsOfType<CrashLink>();

        foreach (CrashLink l in allTheLinks)
        {
            serialisedLevel += l.Serialise() + ";";
        }

        return serialisedLevel;
    }

    public static void DeserialiseLevel(string serialisedLevel, Transform spawnMarker, CrashLink squareLinkPrefab, CrashLink triLinkPrefab, CrashLink hexLinkPrefab)
    {
        //then split the string into individual entries..
        string[] entries;

        char[] delim = { ';' };

        entries = serialisedLevel.Split(delim);

        foreach (string e in entries)
        {
            CrashLink newLink = null;

            //check what type of link this is, then spawn it..

            if (e.Length > 0)
            {
                if (e[0] == 'Q')
                {
                    newLink = Instantiate(squareLinkPrefab, spawnMarker.position, Quaternion.identity) as CrashLink;
                }

                if (e[0] == 'T')
                {
                    newLink = Instantiate(triLinkPrefab, spawnMarker.position, Quaternion.identity) as CrashLink;
                }

                if (e[0] == 'H')
                {
                    newLink = Instantiate(hexLinkPrefab, spawnMarker.position, Quaternion.identity) as CrashLink;
                }

                if (newLink != null)
                    newLink.Deserialise(e);

                if(e[0] == 'C')
                {
                    string[] attributes;
                    char[] localDelim = { '|' };

                    attributes = e.Split(localDelim);

                    Vector3 camPos = new Vector3(0,0,0);

                    camPos.x = float.Parse(attributes[1]);
                    camPos.y = float.Parse(attributes[2]);
                    camPos.z = float.Parse(attributes[3]);


                    Camera.main.transform.position = camPos;

                    if(attributes.Length > 4)
                        Camera.main.orthographicSize = float.Parse(attributes[4]);
                }

            }
        }
    }
}
