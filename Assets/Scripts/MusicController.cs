using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicController : MonoBehaviour
{
    public AudioSource angryMusic;
    public AudioSource surpriseMusic;
    public AudioSource joyMusic;
    public AudioSource sadMusic;
    public AudioSource defaultMusic;

    private AudioSource musicPlay;
    private AudioSource musicToPlay;

    /** 
     *  Initialization
     */
    void Start()
    {
        musicPlay = defaultMusic;
        musicToPlay = defaultMusic;
    }

    /** 
     *  Reset the music to default one
     */
    public void ResetMusic()
    {
        StopMusic(musicPlay);
    }

    /** 
	 *  Change the music accordind to the emotion detected
	 */
    public void SwitchMusic(string emotionName)
    {
        switch (emotionName)
        {
            case "currentAnger":
                musicToPlay = angryMusic;
                break;
            case "currentSurprise":
                musicToPlay = surpriseMusic;
                break;
            case "currentJoy":
                musicToPlay = joyMusic;
                break;
            case "currentSadness":
                musicToPlay = sadMusic;
                break;
            default:
                break;
        }

        if (musicToPlay != musicPlay)
        {
            StopMusic(musicPlay);
            PlayMusic(musicToPlay);
        }
    }

    /** 
	 *  Play the music accordind to the emotion detected
	 */
    private void PlayMusic(AudioSource music)
    {
        musicPlay = music;
        musicPlay.Play();
    }

    /** 
	 *  Stop the music when no/another emotion is detected
	 */
    private void StopMusic(AudioSource music)
    {
        music.Stop();
        musicPlay = defaultMusic;
    }

    /*
		WATCH OUT : FOLLOWING METHODS GENERATE AN INFINITE LOOP WITH THE PAUSE MENU AS BOTH USE TIME.TIMESCALE FUNCTIONS
		TO USE THEM, CHANGE THE PAUSE MENU METHOD OR SEE UNITY DOCUMENTATION ON InvokeRepeating/CancelInvoke METHODS TO WRITE
		NEW FUNCTIONS FOR FADE IN/FADE OUT EFFECTS.
	*/

    /** 
	 *  Fade in the music sound
	 */
    private void FadeIn(AudioSource music)
    {
        music.volume = 0;
        music.Play();
        while (music.volume < 1.0f)
        {
            music.volume += 0.1f * Time.deltaTime;
        }
    }

    /** 
	 *  Fade out the music sound
	 */
    private void FadeOut(AudioSource music)
    {
        while (music.volume > 0.1f)
        {
            music.volume -= 0.1f * Time.deltaTime;
        }
        music.Stop();
        musicPlay = defaultMusic;
    }
}
