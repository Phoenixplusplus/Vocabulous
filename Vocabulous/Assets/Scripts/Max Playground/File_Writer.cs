//////////////////////////////////////////
// Kingston University: Module CI6530   //
// Games Creation Processes             //
// Coursework 2: PC/MAC Game            //
// Team Chumbawumba                     //
// Vocabulous                           //
//////////////////////////////////////////

// Legacy script, was used a LOT for pharsing and editing of dictionaries
using System.IO;
using UnityEngine;

public class File_Writer : MonoBehaviour
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
