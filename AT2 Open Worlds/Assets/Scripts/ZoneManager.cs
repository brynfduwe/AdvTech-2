using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ZoneManager : MonoBehaviour
{
    public bool[] activeZones = new bool[29];
    public int[] sceneLoadCount = new int[29];

    public void LoadZone(int zone, bool unload)
    {
        LoadScene("HexZone/Zone " + zone.ToString(), unload, zone);
    }

    void LoadScene(string zone, bool unload, int index)
    {
        if (unload)
        {
            if (activeZones[index - 1])
            {
                activeZones[index - 1] = false;
           //     sceneLoadCount[index - 1]--;
                StartCoroutine(unloadSceneCor(zone, index));
            }
        }
        else
        {
            if (!activeZones[index - 1])
            {
                activeZones[index - 1] = true;
            //    sceneLoadCount[index - 1]++;
                StartCoroutine(LoadSceneCor(zone, index));
            }
        }
    }


    IEnumerator LoadSceneCor(string zone, int num)
    {
        if (!SceneManager.GetSceneByName("Zone " + num.ToString()).isLoaded)
        {
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(zone, LoadSceneMode.Additive);

            while (!asyncLoad.isDone)
            {
                yield return null;
            }
        }

    }


    IEnumerator unloadSceneCor(string zone, int num)
    {
        AsyncOperation asyncunLoad = SceneManager.UnloadSceneAsync(zone);

        // while (!asyncunLoad.isDone)
        // {
        yield return null;
       // }
    }


    void Update()
    {
      //  StartCoroutine(LoadedChecker());
    }


    IEnumerator LoadedChecker()
    {
        for (int i = 1; i < activeZones.Length; i++)
        {
            if (activeZones[i] == false && SceneManager.GetSceneByName("HexZone/Zone " + i.ToString()).isLoaded)
            {
                // LoadZone(i, true);
                int zoneCount = sceneLoadCount[i];
                for (int j = 0; j < sceneLoadCount[i]; j++)
                {
                    LoadZone(i, true);
                }

            }

            yield return null;
        }
    }
}
