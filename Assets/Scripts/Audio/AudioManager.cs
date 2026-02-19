using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    public Sound[] sfxSounds;
    private AudioSource sfxSource;

    private void Awake()
    {
        if (instance == null) instance = this;
        sfxSource = gameObject.AddComponent<AudioSource>();
    }

    public void Play(string name)
    {
        Sound s = System.Array.Find(sfxSounds, x => x.name == name);
        if (s != null) sfxSource.PlayOneShot(s.clip, s.volume);
    }
}
