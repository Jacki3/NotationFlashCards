using System.Collections;
using System.Collections.Generic;
using MidiJack;
using UnityEngine;
using UnityEngine.InputSystem;

public class MIDIController : MonoBehaviour
{
    public AudioHelm.HelmController helmController;

    public static int startingMIDINumber = 36;

    public delegate void NoteOnEventHandler(int note);

    public static event NoteOnEventHandler NoteOn;

    public delegate void NoteOffEventHandler(int note);

    public static event NoteOffEventHandler NoteOff;

    void Start()
    {
        InputSystem.onDeviceChange += (device, change) =>
        {
            if (change != InputDeviceChange.Added) return;

            var midiDevice = device as Minis.MidiDevice;
            if (midiDevice == null) return;

            midiDevice.onWillNoteOn += (note, velocity) =>
            {
                print(note.noteNumber);
                helmController.NoteOn(note.noteNumber, velocity);
                NoteOn(note.noteNumber);
            };

            midiDevice.onWillNoteOff += (note) =>
            {
                NoteOff(note.noteNumber);
            };
        };
    }
}
