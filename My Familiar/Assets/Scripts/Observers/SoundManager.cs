using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : Observer
{
    // Audiosource for each sound
    public AudioSource BalloonPopSource;
    public AudioSource BounceSource;
    public AudioSource CharacterHurt;
    public AudioSource EatingSource;
    public AudioSource EvolvedSource;
    public AudioSource FootballSource;
    public AudioSource LevelUpSource;
    public AudioSource QuestCompleteSource;
    public AudioSource TapSource; 

    // Receives info on events from subjects
    public override void OnNotify(GameObject GO, Events _event)
    {
        // Play a sound based on event
        switch (_event)
        {
            case Events.CharacterHurt:
                CharacterHurt.Play();
                break;

            case Events.Evolve: // Evolved
                EvolvedSource.Play();
                break;

            case Events.ItemUsed:
                if (GO.name.Contains("Balloon")) // Balloon pop
                {
                    BalloonPopSource.Play();
                }
                else if (GO.name.Contains("Trampoline")) // Bounce
                {
                    BounceSource.Play();
                }
                else if (GO.name.Contains("Apple")) // Eating
                {
                    EatingSource.Play();
                }
                else if (GO.name.Contains("Football")) // Football
                {
                    FootballSource.Play();
                }
                break;

            case Events.LevelUp: // Level Up
                LevelUpSource.Play();
                break;

            case Events.Tap: // Tap
                TapSource.Play();
                break;

            default:
                break;
        }
    }
}
