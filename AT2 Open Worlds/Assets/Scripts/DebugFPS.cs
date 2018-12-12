using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugFPS : MonoBehaviour
{

    public Text fpsCount;
    public Text mpfCount;

    private int fps;
    private float mpf;
	
	// Update is called once per frame
	void Update ()
	{
	  //  fps = (int)(1f / Time.unscaledDeltaTime);
        mpf = (Time.unscaledDeltaTime * 1000);
    }


    void FixedUpdate()
    {
       // fpsCount.text = "FPS: " + fps.ToString();
        mpfCount.text = "MSPF: " + ((int)mpf).ToString();
    }
}
