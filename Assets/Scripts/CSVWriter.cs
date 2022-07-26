using System.IO;
using UnityEngine;

public class CSVWriter : MonoBehaviour
{
    private string wholeFilePath;

    private string fileName;

    public void WriteCSV(string data)
    {
        fileName = ResultsController.userIndex + "_FlashCardResult.csv  ";
        wholeFilePath = Application.dataPath + "/" + fileName;

        StreamWriter tw = new StreamWriter(wholeFilePath, true);

        string newData = "";
        newData += data;

        tw.Write (newData);

        tw.Close();
    }

    public void UploadResults()
    {
        ftp ftpClient =
            new ftp(@"ftp://ftp.lewin-of-greenwich-naval-history-forum.co.uk",
                "lewin-of-greenwich-naval-history-forum.co.uk",
                "YdFDyYkUjKyjmseVmGkhipAB");
        ftpClient
            .upload("/Study4/FlashCardResults/" + fileName, @wholeFilePath);
    }
}
