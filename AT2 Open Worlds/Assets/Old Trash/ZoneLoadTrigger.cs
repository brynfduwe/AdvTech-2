using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class ZoneLoadTrigger : MonoBehaviour
{
    public int[] loadZones;

    public int[] unLoadZones;

    void OnTriggerEnter(Collider other)
    {
        for (int i = 0; i < loadZones.Length; i++)
        {
            GameObject.Find("SceneManager").GetComponent<ZoneManager>().LoadZone(loadZones[i], false);
        }

        for (int i = 0; i < unLoadZones.Length; i++)
        {
            GameObject.Find("SceneManager").GetComponent<ZoneManager>().LoadZone(unLoadZones[i], true);
        }
    }
}
