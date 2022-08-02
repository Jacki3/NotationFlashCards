using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;

public class HelpScreen : MonoBehaviour
{
    [SerializeField]
    private Transform noteMarker;

    [SerializeField]
    private Transform markerParent;

    [SerializeField]
    private int totalKeys;

    [SerializeField]
    private float startingXPos;

    [SerializeField]
    private float sharpYPosition;

    [SerializeField]
    private int minNote;

    [SerializeField]
    private float distX;

    private float maxNote;

    private int[] sharpNotes = { 1, 3, 6, 8, 10 };

    private List<Transform> noteMarkers = new List<Transform>();

    private int startingMIDI;

    private float[] xPositions;

    private void Awake()
    {
        MIDIController.NoteOn += SpawnNoteMarker;
        MIDIController.NoteOff += DestroyNoteMarker;

        startingMIDI = MIDIController.startingMIDINumber;
        maxNote = totalKeys - 1;

        xPositions = new float[totalKeys];

        float dist = 0;

        for (int i = 0; i < totalKeys; i++)
        {
            xPositions[i] = startingXPos + dist;
            dist += distX;
            if (i % 12 == 4 || i % 12 == 11) dist += distX * .75f;
        }
    }

    private void SpawnNoteMarker(int note)
    {
        int x = note - startingMIDI;

        if (minNote <= x && x <= maxNote)
        {
            float posY = -6;
            float posX = xPositions[note - startingMIDI];

            for (int i = 0; i < sharpNotes.Length; i++)
            if (note % 12 == sharpNotes[i]) posY = sharpYPosition;
            Vector3 newPos = new Vector3(posX, posY, 1);

            Transform newMarker = Instantiate(noteMarker, markerParent);
            newMarker.localPosition = newPos;

            noteMarkers.Add (newMarker);
        }
    }

    private void DestroyNoteMarker(int note)
    {
        int x = note - startingMIDI;
        if (minNote <= x && x <= maxNote)
        {
            float posY = -6;
            float posX = xPositions[note - startingMIDI];

            for (int i = 0; i < sharpNotes.Length; i++)
            if (note % 12 == sharpNotes[i]) posY = sharpYPosition;
            Vector3 circlePos = new Vector3(posX, posY, 1);

            for (int i = 0; i < noteMarkers.Count; i++)
            {
                Vector3 newPosition = noteMarkers[i].transform.localPosition;
                if (newPosition == circlePos)
                {
                    Destroy(noteMarkers[i].gameObject);
                    noteMarkers.RemoveAt (i);
                }
            }
        }
    }
}
