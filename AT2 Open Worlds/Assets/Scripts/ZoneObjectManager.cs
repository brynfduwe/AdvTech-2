using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Xml;
using System.Xml.Serialization;
using System.IO;
using UnityEngine.SceneManagement;

public class ZoneObjectManager : MonoBehaviour
{

    public ObjectDatabase ObjDB;
    public int zoneNum;
    public List<GameObject> LODObjects = new List<GameObject>();

    private bool HighLOD;

    // Use this for initialization
    void Awake ()
    {
        //if loading goes wrong, comment all code and uncomment this funtion, run and database is reset.
       //ResetDatabase();

        XmlSerializer load = new XmlSerializer(typeof(ObjectDatabase));
        FileStream stream = new FileStream(Application.dataPath + "/StreamingXML/ObjectListZone" + zoneNum.ToString() + ".xml", FileMode.Open);
        ObjDB = load.Deserialize(stream) as ObjectDatabase;
        stream.Close();


        HighLOD = true;
        LoadLowLOD();
    }


    void ResetDatabase()
    {
        ObjEntry obj = new ObjEntry();
        obj.index = 0;
        obj.highLODPath = "HexZone " + zoneNum.ToString();
        obj.lowLODPath = "LowLODZ" + zoneNum.ToString();
        obj.position = transform.localPosition;
        obj.rotation = transform.localRotation.eulerAngles;

        ObjDB.ObjList.Add(obj);

        XmlSerializer save = new XmlSerializer(typeof(ObjectDatabase));
        FileStream stream =
            new FileStream(Application.dataPath + "/StreamingXML/ObjectListZone" + zoneNum.ToString() + ".xml",
                FileMode.Create);
        save.Serialize(stream, ObjDB);
        stream.Close();
    }


    public void LoadLowLOD()
    {
        ieLoadLowLOD();
    }


    void ieLoadLowLOD()
    {
        if (HighLOD)
        {
            Transform[] allChildren = GetComponentsInChildren<Transform>();

            for (int i = 1; i < allChildren.Length; i++)
            {
                Destroy(allChildren[i].gameObject);
            }

            foreach (var obj in ObjDB.ObjList)
            {
                if (Resources.Load<GameObject>(ObjDB.ObjList[0].lowLODPath) != null)
                {
                    GameObject isnt = Resources.Load(ObjDB.ObjList[0].lowLODPath) as GameObject;

                    GameObject gobj = Instantiate(isnt, transform);
                    gobj.transform.localScale = Vector3.one;
                }
            }

            HighLOD = false;
        }
    }


    public void LoadHighLOD()
    {
        ieLoadHighLOD();
    }

    void ieLoadHighLOD()
    {
        if (!HighLOD)
        {
            Transform[] allChildren = GetComponentsInChildren<Transform>();

            for (int i = 1; i < allChildren.Length; i++)
            {
                Destroy(allChildren[i].gameObject);
            }

            if (Resources.Load<GameObject>(ObjDB.ObjList[0].highLODPath) != null)
            {
                GameObject isnt = Resources.Load(ObjDB.ObjList[0].highLODPath) as GameObject;

                GameObject gobj = Instantiate(isnt, transform);
                gobj.transform.localScale = Vector3.one;
            }
        

        HighLOD = true;

        }
    }



    // Update is called once per frame
    void Update () {
		
	}

    [System.Serializable]
    public class ObjEntry
    {
        public int index;
        public string highLODPath;
        public string lowLODPath;
        public Vector3 position;
        public Vector3 rotation;
    }

    [System.Serializable]
    public class ObjectDatabase
    {
        public List<ObjEntry> ObjList = new List<ObjEntry>();
    }
}
