//////////////////////////////////////////
// Kingston University: Module CI6530   //
// Games Creation Processes             //
// Coursework 2: PC/MAC Game            //
// Team Chumbawumba                     //
// Vocabulous                           //
//////////////////////////////////////////

using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class File_Reader : MonoBehaviour
{
    private bool _fileOpen = false;
    private string _path = "";
    private FileInfo _myFile = null;
    private StreamReader reader = null;
    private string _txt = "";
    private string _currLine = "";
    private int _currIndex = 0;
    private string _currWord = "";

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
            Debug.Log("File_Reader:open() - filepath("+ filepath + ") not valid");
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
        _currLine = ret;
        if (ret == null) close();
        return ret;
    }

    // BUGGED TO FUCK AND BACK - do not use
    public string nextWord()
    {
        string next = "";

        if (_currIndex == _currLine.Length -1 || _currLine == "")
        {
            nextLine();
            _currIndex = 0;
        }
        if (!_fileOpen) return null;
        bool found = false;
        while (found == false)
        {
            if ((int)_currLine[_currIndex] >= 79 && (int)_currLine[_currIndex] <= 122)
            {
                next = next + _currLine[_currIndex];
            }
            else
            {
                if (next.Length < 2)
                {
                    next = "";
                }
                else
                {
                    found = true;
                }
            }
            _currIndex++;
            if (_currIndex == _currLine.Length - 1)
            {
                nextLine();
                _currIndex = 0;
            }
        }
        return next;
    }

    void OnDestroy()
    {
        if (reader != null) reader.Close();
    }


}
