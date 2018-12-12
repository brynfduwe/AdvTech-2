using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.SceneManagement;

using System.Xml;
using System.Xml.Serialization;
using System.IO;
using UnityEngine.SceneManagement;

public class ZoneNode : MonoBehaviour
{
    public float distToPlayer;
    public float range;
    public int loadZone;

    public AIdatabase AiDb;
    public List<Transform> AIObjectList = new List<Transform>();

    public bool loaded = false;
    bool aiLoaded;

    public int AICount;

    // Use this for initialization
    void Start ()
    {
        AICount = 50;
    }

    // Update is called once per frame
    void Update ()
	{
        distToPlayer = Vector3.Distance(transform.position, GameObject.Find("Player").transform.position);

        //  if (distToPlayer < range)

        Vector3 playerDir = transform.position - GameObject.Find("Player").transform.position;
        float playerAngle = Vector3.Dot(playerDir.normalized, GameObject.Find("Player").transform.forward);

        if (playerAngle > 0.5f || distToPlayer < range)
        {
            if (!loaded)
            {
                loaded = true;
                GameObject.Find("SceneManager").GetComponent<ZoneManager>().LoadZone(loadZone, false);

                if (distToPlayer < 150)
                {
                    Load();
                    aiLoaded = true;
                }
                else
                {
                    if (aiLoaded)
                    {
                        Save();
                        aiLoaded = false;
                    }
                }
            }
            StartCoroutine(checkLODDistance());
        }
        else
        {
            if (loaded)
            {
                loaded = false;
                GameObject.Find("SceneManager").GetComponent<ZoneManager>().LoadZone(loadZone, true);
            }
            if (aiLoaded)
            {
                Save();
                aiLoaded = false;
            }
        }

    }


    void Load()
    {

     //   UnityEditor.AssetDatabase.ImportAsset(Application.dataPath + "/StreamingXML/aiListZone" + loadZone.ToString() + ".xml");
    //    UnityEditor.AssetDatabase.ImportAsset(Application.dataPath + "/StreamingXML");

        AIObjectList.Clear();
        AiDb.AIList.Clear();

        // loads from xml file
        XmlSerializer load = new XmlSerializer(typeof(AIdatabase));
        FileStream stream = new FileStream(Application.dataPath + "/StreamingXML/aiListZone" + loadZone.ToString() + ".xml", FileMode.Open);
        Debug.Log(stream.Name.ToString());
        AiDb = load.Deserialize(stream) as AIdatabase;
        stream.Close();


        // //loads objects

        int i = 0;
        foreach (var ai in AiDb.AIList)
        {
            if (i < 30)
            {
                GameObject gobj = Resources.Load<GameObject>("AI");
                gobj.transform.position = new Vector3(ai.position.x, 0.8f, ai.position.z);
                gobj.transform.eulerAngles = ai.rotation;
                GameObject inst = Instantiate(gobj);
                AIObjectList.Add(inst.transform);
                inst.GetComponent<AI>().SetStateOnSpawn(ai.state);

                inst.GetComponent<AI>().nodes = transform.parent.GetComponentsInChildren<Transform>();
                inst.GetComponent<AI>().FindNearestNode();

                i++;
            }
        }

        AICount = AIObjectList.Count;
    }

    void Save()
    {
        if (AiDb.AIList.Count > 0)
        {
            Debug.Log("Saved Zone " + loadZone.ToString());

//            File.Delete(Application.dataPath + "/StreamingXML/aiListZone" + loadZone.ToString() + ".xml");
            UnityEditor.AssetDatabase.ImportAsset(Application.dataPath + "/StreamingXML/aiListZone" + loadZone.ToString() + ".xml");

            AiDb.AIList.Clear();
            AiDb = new AIdatabase();
            AIdatabase x = new AIdatabase();

            AICount = 0;

            List<Transform> tempList = new List<Transform>();

            foreach (var ai in AIObjectList)
            {
                if (ai != null)
                {
                    tempList.Add(ai);
                }
            }

            for (int i = 0; i < tempList.Count; i++)
            {

                if (tempList[i] != null)
                {
                    AIEntry ax = new AIEntry();
                    ax.position = tempList[i].position;
                    ax.rotation = tempList[i].eulerAngles;
                    ax.state = (int)tempList[i].GetComponent<AI>().CurrentState;

                    x.AIList.Add(ax);
                }

                if ((i > 0 && AIObjectList[i].position == AIObjectList[0].position) && (AIObjectList[i] != null && AIObjectList[0] != null))
                {
                    break;
                }


                AICount++;
            }

            //saves the xml to file
            XmlSerializer save = new XmlSerializer(typeof(AIdatabase));
            FileStream stream =
                new FileStream(Application.dataPath + "/StreamingXML/aiListZone" + loadZone.ToString() + ".xml",
                    FileMode.Create);
            save.Serialize(stream, x);
            stream.Close();

            //  Debug.Log(AiDb.AIList.Count.ToString());
        }

     //   UnityEditor.AssetDatabase.ImportAsset(Application.dataPath + "/StreamingXML/aiListZone" + loadZone.ToString() + ".xml");
     //   UnityEditor.AssetDatabase.ImportAsset(Application.dataPath + "/StreamingXML");

        Unload();      
    }

    void SaveAdd(Transform ai)
    {
        //Debug.Log("Additional Save " + loadZone.ToString());

        //AIEntry ax = new AIEntry();
        //ax.position = ai.position;
        //ax.rotation = ai.eulerAngles;

        //AiDb.AIList.Add(ax);

        ////saves the xml to file
        //XmlSerializer save = new XmlSerializer(typeof(AIdatabase));
        //FileStream stream = new FileStream(Application.dataPath + "/StreamingXML/aiListZone" + loadZone.ToString() + ".xml", FileMode.Create);
        //save.Serialize(stream, AiDb);
        //stream.Close();
    }


    void Unload()
    {
        foreach (var ai in AIObjectList)
        {
            if (ai != null)
            {
                Destroy(ai.gameObject);
            }
        }

        Debug.Log("Loaded Zone " + loadZone.ToString() + " = " + AIObjectList.Count.ToString());

        AIObjectList.Clear();
    }


    IEnumerator checkLODDistance()
    {
        Vector3 playerDir = transform.position - GameObject.Find("Player").transform.position;
        float playerAngle = Vector3.Dot(playerDir.normalized, GameObject.Find("Player").transform.forward);

        if (distToPlayer > 100)
        {

            if (GameObject.Find("HexZone " + loadZone.ToString() + " Manager") != null)
                GameObject.Find("HexZone " + loadZone.ToString() + " Manager").GetComponent<ZoneObjectManager>().LoadLowLOD();
        }
        else
        {
           if(GameObject.Find("HexZone " + loadZone.ToString() + " Manager") != null)
            GameObject.Find("HexZone " + loadZone.ToString() + " Manager").GetComponent<ZoneObjectManager>().LoadHighLOD();
        }

        yield return null;
    }


    void OnApplicationQuit()
    {
        if (loaded)
        {
          //  Save();
        }

        ////uncomment this and comment the rest of Start to create a new XML
        //foreach (var ai in AiDb.AIList)
        //{
        //    ai.position = transform.position + new Vector3(Random.Range(-30, 30), 0, Random.Range(-30, 30));
        //    ai.state = (int)AI.State.Idle;
        //}
        //XmlSerializer save = new XmlSerializer(typeof(AIdatabase));
        //FileStream stream = new FileStream(Application.dataPath + "/StreamingXML/aiListZone" + loadZone.ToString() + ".xml", FileMode.Create);
        //save.Serialize(stream, AiDb);
        //stream.Close();
    }


    public void AddToAIList(Transform obj)
    {
        AIObjectList.Add(obj);

      //  SaveAdd(obj);

        if (!loaded)
        {
            Save();
            Destroy(obj.gameObject);
        }
    }


    [System.Serializable]
    public class AIEntry
    {
        public int ID;
        public int zone;
        public int state;
        public Vector3 position;
        public Vector3 rotation;
    }

    [System.Serializable]
    public class AIdatabase
    {
        public List<AIEntry> AIList = new List<AIEntry>();
    }
}
