using System.IO;
using UnityEngine;

public class CSVWriter : MonoBehaviour
{
    private string fileName;

    void Start()
    {
#if UNITY_EDITOR
        fileName = Application.dataPath + "studyThreeFlashCardResults.csv";
#endif



#if PLATFORM_STANDALONE_WIN
        fileName = "D:/StudyThree/studyThreeFlashCardResults.csv";
#endif
    }

    public void WriteCSV(string data)
    {
        TextWriter tw = new StreamWriter(fileName, true);

        string newData = "";
        newData += data;

        tw.Write (newData);

        tw.Close();
    }
}
