using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ResultsController : MonoBehaviour
{
    [Header("Results")]
    public CSVWriter csvWriter;

    public static int userIndex;

    public static int totalTestsRun;

    public float totalTime;

    public bool preTest = true;

    public List<Question> questions = new List<Question>();

    [Header("UI")]
    public TextMeshProUGUI totalTimeText;

    public TextMeshProUGUI questionResults;

    private bool totalTimerStarted;

    [System.Serializable]
    public class Question
    {
        public int totalIncorrectGuesses;

        public float timeTakenToAnswer;

        public string noteName;
    }

    void Start()
    {
    }

    void Update()
    {
        if (totalTimerStarted) totalTime += Time.deltaTime;
    }

    public void StartTotalTimer() =>
        totalTimerStarted = totalTimerStarted ? false : true;

    public void ShowStats()
    {
        float sum = 0;
        float totalErrors = 0;

        string formattedTotalTime = totalTime.ToString("0.00");
        totalTimeText.text = "Total Time to Complete: " + formattedTotalTime;

        string testState = preTest ? "preTest" : "postTest";

        csvWriter
            .WriteCSV("User_" +
            userIndex +
            "_" +
            testState +
            "_" +
            totalTestsRun +
            ",");

        foreach (Question question in questions)
        {
            int index = (questions.IndexOf(question) + 1);
            string formattedTime = question.timeTakenToAnswer.ToString("0.00");

            questionResults.text +=
                "Question " +
                index +
                "\n" +
                "Incorrect Guesses: " +
                question.totalIncorrectGuesses +
                "\n" +
                "Time Taken To Answer: " +
                formattedTime +
                "\n";

            csvWriter
                .WriteCSV("q" +
                index +
                "_noteName" +
                "," +
                "q" +
                index +
                "_time, " +
                "q" +
                index +
                "_errors,");
        }
        csvWriter.WriteCSV("Total Time,");
        csvWriter.WriteCSV("Average Answer Time,");
        csvWriter.WriteCSV("Total Errors,");
        csvWriter.WriteCSV("Error Average");

        csvWriter.WriteCSV(System.Environment.NewLine + ",");

        foreach (Question question1 in questions)
        {
            string formattedTime = question1.timeTakenToAnswer.ToString("0.00");

            csvWriter
                .WriteCSV(question1.noteName +
                "," +
                formattedTime +
                "," +
                question1.totalIncorrectGuesses +
                ",");

            sum += question1.timeTakenToAnswer;
            totalErrors += question1.totalIncorrectGuesses;
        }
        sum /= questions.Count;
        float errorAvg = totalErrors / questions.Count;
        string formattedAvg = sum.ToString("0.00");
        string formattedErrorAvg = errorAvg.ToString("0.00");
        csvWriter.WriteCSV(formattedTotalTime + ",");
        csvWriter.WriteCSV(formattedAvg + ",");
        csvWriter.WriteCSV(totalErrors + ",");
        csvWriter.WriteCSV (formattedErrorAvg);
        csvWriter.WriteCSV(System.Environment.NewLine);
    }

    public void ChangeUserIndex(TMP_InputField index)
    {
        userIndex = int.Parse(index.text);
    }

    public void ChangeTestType()
    {
        preTest = preTest ? false : true;
    }

    public void ChangeRuns(TMP_InputField index)
    {
        totalTestsRun = int.Parse(index.text);
    }
}
