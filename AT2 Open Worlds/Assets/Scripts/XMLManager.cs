using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Xml;
using System.Xml.Serialization;
using System.IO;

public class XMLManager : MonoBehaviour
{
    public static XMLManager instance;
    public AIdatabase AiDb;
    public List<Transform> AIObjectList = new List<Transform>();

    // SINGLETON
    void Awake ()
	{
	    instance = this;
	}

    void Start()
    {
        //// loads from xml file
        XmlSerializer load = new XmlSerializer(typeof(AIdatabase));
        FileStream stream = new FileStream(Application.dataPath + "/StreamingXML/aiList.xml", FileMode.Open);
        AiDb = load.Deserialize(stream) as AIdatabase;
        stream.Close();

        // //loads objects
        foreach (var ai in AiDb.AIList)
        {
            GameObject gobj = Resources.Load<GameObject>("AI");
            gobj.transform.position = ai.position + new Vector3(0,4,0);
            gobj.transform.eulerAngles = ai.rotation;
            GameObject inst = Instantiate(gobj);
            AIObjectList.Add(inst.transform);
        }

        //XmlSerializer save = new XmlSerializer(typeof(AIdatabase));
        //FileStream stream = new FileStream(Application.dataPath + "/StreamingXML/aiList.xml", FileMode.Create);
        //save.Serialize(stream, AiDb);
        //stream.Close();
    }


    void OnApplicationQuit()
    {
        Debug.Log("Saved!");

        for (int i = 0; i < AIObjectList.Count; i++)
        {
            AiDb.AIList[i].position = AIObjectList[i].position;
            AiDb.AIList[i].rotation = AIObjectList[i].eulerAngles;
        }

        //saves the xml to file
        XmlSerializer save = new XmlSerializer(typeof(AIdatabase));
        FileStream stream = new FileStream(Application.dataPath + "/StreamingXML/aiList.xml", FileMode.Create);
        save.Serialize(stream, AiDb);
        stream.Close();
    }


    public void Save()
    {

    }

    public void Load()
    {

    }

    [System.Serializable]
    public class AIEntry
    {
        public int ID;
        public int zone;
        public Vector3 position;
        public Vector3 rotation; 
    }

    [System.Serializable]
    public class AIdatabase
    {
        public List<AIEntry> AIList = new List<AIEntry>();
    }
}
