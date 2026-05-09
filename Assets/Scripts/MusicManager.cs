using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance;

    [Header("Music")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip[] songs;

    private List<AudioClip> shuffledSongs = new List<AudioClip>();
    private int currentSongIndex = 0;

    void Awake()
    {
        // Singleton
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        ShuffleSongs();
        PlayNextSong();
    }

    void Update()
    {
        if (!audioSource.isPlaying)
        {
            PlayNextSong();
        }
    }

    void ShuffleSongs()
    {
        shuffledSongs.Clear();

        foreach (AudioClip song in songs)
        {
            shuffledSongs.Add(song);
        }

        for (int i = 0; i < shuffledSongs.Count; i++)
        {
            AudioClip temp = shuffledSongs[i];

            int randomIndex = Random.Range(i, shuffledSongs.Count);

            shuffledSongs[i] = shuffledSongs[randomIndex];
            shuffledSongs[randomIndex] = temp;
        }

        currentSongIndex = 0;
    }

    void PlayNextSong()
    {
        // Reshuffle when all songs have been played
        if (currentSongIndex >= shuffledSongs.Count)
        {
            ShuffleSongs();
        }

        audioSource.clip = shuffledSongs[currentSongIndex];
        audioSource.Play();

        currentSongIndex++;
    }
}