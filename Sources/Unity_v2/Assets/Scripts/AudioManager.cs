// Audio.AudioManager
using Audio;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;

    //public Sound[] footsteps;

    public AudioLowPassFilter filter;

    public bool muted;

    public static AudioManager Instance
    {
        get;
        set;
    }

    private void Awake()
    {
        Instance = this;
        Sound[] array = sounds;
        foreach (Sound sound in array)
        {
            sound.source = base.gameObject.AddComponent<AudioSource>();
            sound.source.clip = sound.clip;
            sound.source.loop = sound.loop;
            sound.source.volume = sound.volume;
            sound.source.pitch = sound.pitch;
            sound.source.bypassListenerEffects = sound.bypass;
        }
        //array = footsteps;
        foreach (Sound sound2 in array)
        {
            sound2.source = base.gameObject.AddComponent<AudioSource>();
            sound2.source.clip = sound2.clip;
            sound2.source.loop = sound2.loop;
            sound2.source.volume = sound2.volume;
            sound2.source.pitch = sound2.pitch;
            sound2.source.bypassListenerEffects = sound2.bypass;
        }

    }

    public void MuteSounds(bool b)
    {
        if (b)
        {
            AudioListener.volume = 0f;
        }
        else
        {
            AudioListener.volume = 1f;
        }
        muted = b;
    }

    public void PlayButton()
    {
        if (muted)
        {
            return;
        }
        Sound[] array = sounds;
        foreach (Sound sound in array)
        {
            if (sound.name == "Button")
            {
                sound.source.pitch = 0.8f + Random.Range(-0.03f, 0.03f);
                break;
            }
        }
        Play("Button");
    }

    public void PlayPitched(string n, float v)
    {
        if (muted)
        {
            return;
        }
        Sound[] array = sounds;
        foreach (Sound sound in array)
        {
            if (sound.name == n)
            {
                sound.source.pitch = 1f + Random.Range(0f - v, v);
                break;
            }
        }
        Play(n);
    }

    public void MuteMusic()
    {
        Sound[] array = sounds;
        int num = 0;
        Sound sound;
        while (true)
        {
            if (num < array.Length)
            {
                sound = array[num];
                if (sound.name == "Song")
                {
                    break;
                }
                num++;
                continue;
            }
            return;
        }
        sound.source.volume = 0f;
    }

    public void SetVolume(float v)
    {
        Sound[] array = sounds;
        int num = 0;
        Sound sound;
        while (true)
        {
            if (num < array.Length)
            {
                sound = array[num];
                if (sound.name == "Song")
                {
                    break;
                }
                num++;
                continue;
            }
            return;
        }
        sound.source.volume = v;
    }
    public void UnmuteMusic()
    {
        Sound[] array = sounds;
        int num = 0;
        Sound sound;
        while (true)
        {
            if (num < array.Length)
            {
                sound = array[num];
                if (sound.name == "Song")
                {
                    break;
                }
                num++;
                continue;
            }
            return;
        }
        sound.source.volume = 1.15f;
    }

    public void Play(string n)
    {
        if (muted && n != "Song")
        {
            return;
        }
        Sound[] array = sounds;
        int num = 0;
        Sound sound;
        while (true)
        {
            if (num < array.Length)
            {
                sound = array[num];
                if (sound.name == n)
                {
                    break;
                }
                num++;
                continue;
            }
            return;
        }
        sound.source.Play();
    }

    /*
    public void PlayFootStep()
    {
        if (!muted)
        {
            int num = Random.Range(0, footsteps.Length - 1);
            footsteps[num].source.Play();
        }
    }
    */

    public void Stop(string n)
    {
        Sound[] array = sounds;
        int num = 0;
        Sound sound;
        while (true)
        {
            if (num < array.Length)
            {
                sound = array[num];
                if (sound.name == n)
                {
                    break;
                }
                num++;
                continue;
            }
            return;
        }
        sound.source.Stop();
    }

    public bool isPlaying()
    {
        return GetComponent<AudioSource>().isPlaying;
    }
}