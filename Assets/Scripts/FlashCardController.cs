using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FlashCardController : MonoBehaviour
{
    [Header("Game Elements")]
    public ResultsController resultsController;

    public Timer timer;

    public AudioHelm.HelmController helmController;

    public Image note;

    public Image clef;

    public Camera gameCamera;

    public int questionIndex;

    public int[] possibleNotes;

    public int[] easyNotesTreble;

    public int[] easyNotesBass;

    public Transform[] noteSpawnsY;

    public Transform[] noteSpawnsYBass;

    [Header("Game Sounds")]
    public AudioClip rightSound;

    public AudioClip wrongSound;

    public AudioClip winSound;

    [Header("Game UI")]
    public TextMeshProUGUI scoreText;

    public TextMeshProUGUI timerText;

    public TextMeshProUGUI endText;

    public Button playButton;

    public Button exitButton;

    [Header("Game Settings")]
    public bool useBassClef;

    public float chanceOfBass = .6f;

    public float chanceOfDifficultNotes = .08f;

    public int answerStreak = 0;

    private int totalToComplete = 15;

    private int currentNote;

    private int previousNote;

    private int totalCorrect;

    private bool gameStarted;

    private Animator cameraAnimator;

    private AudioSource audioSource;

    private float timeTakenToAnswer;

    private bool questionPosed;

    private bool usingBass = false;

    private string[]
        noteNames =
        { "C", "C#", "D", "D#", "E", "F", "F#", "G", "G#", "A", "A#", "B" };

    private void Awake()
    {
        MIDIController.NoteOn += CheckCorrectNote;
        MIDIController.NoteOff += NoteOff;

        cameraAnimator = gameCamera.GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    void Start()
    {
        totalToComplete = possibleNotes.Count();
        timer.enabled = false;
        note.enabled = false;
        timerText.enabled = false;
        scoreText.text = totalCorrect + "/" + totalToComplete;

        for (int i = 0; i < totalToComplete; i++)
        {
            resultsController.questions.Add(new ResultsController.Question());
        }
    }

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Space))
        {
            StartTest();
        }

        if (timer.TimeUp() && !gameStarted && timer.enabled)
        {
            timer.enabled = false;
            timerText.enabled = false;
            gameStarted = true;
            SetNotePosition();
            resultsController.StartTotalTimer();
        }

        if (Input.GetKeyUp(KeyCode.R))
        {
            MIDIController.NoteOn -= CheckCorrectNote;
            MIDIController.NoteOff -= NoteOff;
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        timerText.text = timer.DisplayTime(timer.timeRemaining);

        if (questionPosed) timeTakenToAnswer += Time.deltaTime;

        if (Input.GetKey(KeyCode.Escape)) Application.Quit();
    }

    public void StartTest()
    {
        if (resultsController.UserIndexFilled())
        {
            endText.enabled = false;
            playButton.gameObject.SetActive(false);
            timer.enabled = true;
            timer.StartTimer();
            timerText.enabled = true;
        }
    }

    private void SetNotePosition()
    {
        usingBass = false;

        //disable all note ledgers
        foreach (Transform child in note.transform)
        {
            child.gameObject.SetActive(false);
        }

        note.enabled = true;

        timeTakenToAnswer = 0;
        questionPosed = true;

        List<int> notes = new List<int>(possibleNotes);

        // if (useBassClef)
        // {
        //     if (Random.value > chanceOfBass)
        //     {
        //         usingBass = true;
        //         if (Random.value > chanceOfDifficultNotes)
        //         {
        //             notes = new List<int>(easyNotesBass);
        //         }
        //         else
        //         {
        //             foreach (int note in easyNotesBass)
        //             {
        //                 notes.Remove (note);
        //             }
        //         }
        //     }
        //     else
        //     {
        //         usingBass = false;
        //         if (Random.value > chanceOfDifficultNotes)
        //         {
        //             notes = new List<int>(easyNotesTreble);
        //         }
        //         else
        //         {
        //             foreach (int note in easyNotesTreble)
        //             {
        //                 notes.Remove (note);
        //             }
        //         }
        //     }
        // }
        // while (previousNote == currentNote)
        // {
        //     int randIndex = Random.Range(0, notes.Count);
        // }
        currentNote = notes[questionIndex];
        if (questionIndex < notes.Count - 1) questionIndex++;

        // previousNote = currentNote;
        // Transform[] possibleSpawns;
        if (usingBass || currentNote < 24)
        {
            clef.transform.GetChild(0).GetComponent<Image>().enabled = true;
            clef.enabled = false;
            // possibleSpawns = noteSpawnsYBass;
        }
        else
        {
            clef.transform.GetChild(0).GetComponent<Image>().enabled = false;
            clef.enabled = true;
            // possibleSpawns = noteSpawnsY;
        }

        if (currentNote >= 24)
        {
            clef.transform.GetChild(0).GetComponent<Image>().enabled = false;
            clef.enabled = true;
        }

        note.transform.localPosition =
            new Vector2(note.transform.localPosition.x,
                noteSpawnsY[currentNote].localPosition.y);

        string clefName = usingBass ? "bass_" : "treble_";
        resultsController.questions[totalCorrect].noteName =
            clefName + noteNames[currentNote % 12];

        //this needs to allow for starting midi number pls
        switch (currentNote)
        {
            case 0:
                //middle and high above = true
                note.transform.GetChild(0).gameObject.SetActive(true);
                note.transform.GetChild(3).gameObject.SetActive(true);
                break;
            case 2:
                //high = true
                note.transform.GetChild(2).gameObject.SetActive(true);
                break;
            case 4:
            case 24:
            case 45:
                //middle  = true
                note.transform.GetChild(0).gameObject.SetActive(true);
                break;
            case 47:
                //below = true;
                note.transform.GetChild(1).gameObject.SetActive(true);
                break;
        }
    }

    private void CheckCorrectNote(int note)
    {
        if (gameStarted)
        {
            var currentQuestionIncorrects =
                resultsController.questions[totalCorrect].totalIncorrectGuesses;

            //if currentNote is below 11?
            // if (usingBass && currentQuestionIncorrects <= 0) currentNote -= 12;
            note -= MIDIController.startingMIDINumber;
            if (note == currentNote)
            {
                questionPosed = false;
                resultsController.questions[totalCorrect].timeTakenToAnswer =
                    timeTakenToAnswer;
                totalCorrect++;
                scoreText.text = totalCorrect + "/" + totalToComplete;

                audioSource.PlayOneShot (rightSound);
                audioSource.pitch += .1f;
                cameraAnimator.SetTrigger("Right");
                if (Complete())
                {
                    resultsController.StartTotalTimer();
                    resultsController.ShowStats();
                    audioSource.PlayOneShot (winSound);
                    gameStarted = false;
                    endText.text = "Test Complete \n\n Press ESC to exit";
                    exitButton.gameObject.SetActive(true);
                    endText.enabled = true;
                }
                else
                {
                    answerStreak++;
                    chanceOfDifficultNotes += .002f;
                    SetNotePosition();
                }
            }
            else
            {
                answerStreak = 0;
                chanceOfDifficultNotes = .08f;
                resultsController
                    .questions[totalCorrect]
                    .totalIncorrectGuesses++;
                audioSource.pitch = 1f;
                audioSource.PlayOneShot (wrongSound);
                cameraAnimator.SetTrigger("Wrong");
            }
        }
    }

    private void NoteOff(int note)
    {
    }

    public void UpdateTotal(TMP_InputField index)
    {
        totalToComplete = int.Parse(index.text);
    }

    public void Exit()
    {
        Application.Quit();
    }

    private bool Complete() => totalCorrect >= totalToComplete;
}
