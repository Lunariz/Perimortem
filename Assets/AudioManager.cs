using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum Audio {
    Beep,
    Footstep1,
    Footstep2,
    Footstep3,
    Jump,
    Heartbeat,
    Hum,
    Bump,
    Flash
}

public enum Music
{
    BaseMusic,
    Tension1,
    Tension2,
}

public class AudioManager : MonoBehaviour {
    public AudioSource baseMusic;
    public AudioSource Tension1;
    public AudioSource Tension2;

    private List<AudioSource> musicSources;

    public List<AudioClip> audios;
    public List<AudioClip> musics;
    private List<float> volumes; //intended volume
    public static AudioManager instance;
    private List<AudioSource> sources = new List<AudioSource>();
    private List<AudioSource> playingSources = new List<AudioSource>();



	// Use this for initialization
	void Start () {
        instance = this;
        volumes = new List<float>() { 1, 0, 0 };
        
        PlayMusic(Music.BaseMusic);
        PlayMusic(Music.Tension1, false);
        PlayMusic(Music.Tension2, false);

        musicSources = new List<AudioSource>() { baseMusic, Tension1, Tension2 };
	}

    // Update is called once per frame
    void Update()
    {
        

        if (Camera.main != null)
            transform.position = Camera.main.transform.position;
        List<AudioSource> moveSources = new List<AudioSource>();
        foreach (AudioSource source in playingSources)
        {
            if (!source.isPlaying)
            {
                moveSources.Add(source);
                sources.Add(source);
            }
        }
        foreach (AudioSource source in moveSources)
        {
            playingSources.Remove(source);
        }
        for (int i = 0; i < volumes.Count; i++)
        {
            if (musicSources[i].volume != volumes[i])
            {
                float diff = volumes[i] -  musicSources[i].volume;
                if (Mathf.Abs(diff) <= Time.deltaTime)
                    musicSources[i].volume = volumes[i];
                else
                {
                    musicSources[i].volume += ((diff / Mathf.Abs(diff)) * Time.deltaTime) / 2f;
                }
            }
        }
    }

    public void PlayMusic(int musicIndex, bool on = true, float volume = 1)
    {
        AudioSource source = null;
        switch (musicIndex)
        {
            case (int)Music.BaseMusic:
                if (baseMusic == null)
                    baseMusic = getSource();
                source = baseMusic;
                break;
            case (int)Music.Tension1:
                if (Tension1 == null)
                    Tension1 = getSource();
                source = Tension1;
                break;
            case (int)Music.Tension2:
                if (Tension2 == null)
                    Tension2 = getSource();
                source = Tension2;
                break;
        }

        source.clip = musics[musicIndex];
        if (!source.isPlaying)
        {
            source.Play();
            source.loop = true;
        }
        if (on)
        {
            volumes[musicIndex] = volume;
        }
        else
        {
            source.volume = 0;
            volumes[musicIndex] = 0;
        }

    }

    public void PlayMusic(Music music, bool on = true, float volume = 1)
    {
        PlayMusic((int)music, on, volume);
    }

    public void PlaySound(Audio audio)
    {
        getSource().PlayOneShot(audios[(int)audio]);
    }
	


    private AudioSource getSource()
    {
        if (sources.Count > 0)
        {
            AudioSource a = sources[0];
            playingSources.Add(a);
            sources.Remove(a);
            return a;
        }
        else
        {
            AudioSource a = this.gameObject.AddComponent<AudioSource>();
            playingSources.Add(a);
            return a;
        }
    }
}
