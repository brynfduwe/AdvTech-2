using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class DebugFPS : MonoBehaviour
{
    public string csvFilePath;

    public Text fpsCount;
    public Text mpfCount;

    private int fps;
    private float mpf;

    private void Start()
    {
        StreamWriter writer = new StreamWriter(csvFilePath, append: false);
        writer.Flush();
        writer.Close();
    }

    // Update is called once per frame
    void Update()
    {
        //  fps = (int)(1f / Time.unscaledDeltaTime);
        mpf = (Time.unscaledDeltaTime * 1000);

        FileStream fs = new FileStream(csvFilePath, FileMode.Append, FileAccess.Write, FileShare.Write);
        fs.Close();
        StreamWriter writer = new StreamWriter(csvFilePath, append: true);
        writer.Write(mpf.ToString() + "\n");
        writer.Close();
    }


    void FixedUpdate()
    {
        // fpsCount.text = "FPS: " + fps.ToString();
        mpfCount.text = "MSPF: " + ((int)mpf).ToString();
    }
}
