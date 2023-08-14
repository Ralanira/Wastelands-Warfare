using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Music
{
    public class MusicPlayer : MonoBehaviour
    {
        private AudioSource _audiosource;
        [SerializeField] AudioClip[] songs;

        void Awake() 
        {
        /* int numMusicPlayers = FindObjectsOfType<MusicPlayer>().Length;
            if (numMusicPlayers > 1) 
            {
                Destroy(gameObject);  
            }
            else 
            {
                DontDestroyOnLoad(gameObject);
            }*/
        }

        void Start() 
        {
            _audiosource = GetComponent<AudioSource>();  
            if(!_audiosource.isPlaying)
                ChangeSong(Random.Range(0, songs.Length));  
        }

        void Update() 
        {
            if(!_audiosource.isPlaying)
                ChangeSong(Random.Range(0, songs.Length));
        }

        public void ChangeSong(int songPicked)
        {
            _audiosource.clip = songs[songPicked];
            _audiosource.Play();
        }
    }
}

