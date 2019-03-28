//////////////////////////////////////////
// Kingston University: Module CI6530   //
// Games Creation Processes             //
// Coursework 2: PC/MAC Game            //
// Team Chumbawumba                     //
// Vocabulous                           //
//////////////////////////////////////////


// -- This script is lagacy and not part of the build, however, to avoid Git conflicts, leave this alone -- //


using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class File_Writer1 : MonoBehaviour
{
    private StreamWriter writer = null;

    public void open(string path)
    {
        writer = new StreamWriter(Application.dataPath + path, true);
        if (writer == null) Debug.Log("File_Writer.open() - Cannot Open file");
    }

    public void writeLine(string txt)
    {
        writer.WriteLine(txt);
    }

    public void close()
    {
        if (writer != null) writer.Close();
    }

    void OnDestroy()
    {
        if (writer != null) writer.Close();
    }
}
