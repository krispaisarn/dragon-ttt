using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TTT.Audio
{
    public class AudioManager : BaseMono
    {
        public AudioSource sfxPlayer;
        public AudioSource bgmPlayer;
        [Header("AudioClip")]
        [SerializeField] private AudioClip buttonSFX;
        [SerializeField] private AudioClip resultSFX;
        [SerializeField] private AudioClip roundEndSFX;
        [SerializeField] private AudioClip placeMarkSFX;

        private bool isInitialized;
        public override void Initialize()
        {
            if (isInitialized)
                return;

            isInitialized = true;
        }

        public void PlayButtonClick()
        {
            sfxPlayer.PlayOneShot(buttonSFX);
        }

        public void PlayResult()
        {
            sfxPlayer.PlayOneShot(resultSFX);
        }

        public void PlayRoundEnd()
        {
            sfxPlayer.PlayOneShot(roundEndSFX);
        }
        public void PlayMark()
        {
            sfxPlayer.PlayOneShot(placeMarkSFX);
        }

        public void ToggleBGM(bool isPlay)
        {
            if (!isPlay)
            {
                bgmPlayer.Stop();
            }
            else
            {
                if (!bgmPlayer.isPlaying)
                    bgmPlayer.Play();
            }
        }
    }
}
