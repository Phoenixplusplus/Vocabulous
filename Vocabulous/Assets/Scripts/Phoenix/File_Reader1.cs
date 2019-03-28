//////////////////////////////////////////
// Kingston University: Module CI6530   //
// Games Creation Processes             //
// Coursework 2: PC/MAC Game            //
// Team Chumbawumba                     //
// Vocabulous                           //
//////////////////////////////////////////
// LEGACY // 

using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class File_Reader1 : MonoBehaviour
{
    private bool _fileOpen = false;
    private string _path = "";
    private FileInfo _myFile = null;
    private StreamReader reader = null;
    private string _txt = "";

    public bool isOpen()
    {
        if (_fileOpen)
            return _fileOpen;
        else return false;
    }

    public void open(string filepath)
    {
        if (_fileOpen)
        {
            reader.Close();
        }
        _myFile = new FileInfo(Application.dataPath + filepath);
        if (_myFile != null && _myFile.Exists)
        {
            reader = _myFile.OpenText();
        }
        if (reader == null)
        {
            Debug.Log(filepath + " not valid");
        }
        else
        {
            _fileOpen = true;   
        }
    }

    public void close()
    {
            reader.Close();
            _fileOpen = false;
    }

    public string nextLine()
    {
        if (!_fileOpen) return null;

        string ret = reader.ReadLine();
        if (ret == null) close();
        return ret;
    }

    void OnDestroy()
    {
        if (reader != null) reader.Close();
    }


}
