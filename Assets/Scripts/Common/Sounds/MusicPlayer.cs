using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pikachu.Scripts.Common.Sounds
{
    public class MusicPlayer : MonoBehaviour
    {
        [SerializeField] private AudioSource musicPlayer;
        [SerializeField] private AudioClip[] backgroundMusics;

        private YieldInstruction _startDelay = new WaitForSeconds(0.5f);
        private YieldInstruction _delayNextMusic = new WaitForSeconds(1.5f);

        private void Awake()
        {
            StartCoroutine(PlayListMusics());
        }

        private IEnumerator PlayListMusics()
        {
            int rand, previous = 0;
            yield return _startDelay;

            while (true)
            {
                yield return _delayNextMusic;
                rand = Random.Range(0, backgroundMusics.Length);

                if (rand == previous)
                    rand = (rand + 1) % backgroundMusics.Length;

                previous = rand;
                musicPlayer.clip = backgroundMusics[rand];
                musicPlayer.Play();
                yield return new WaitForSeconds(backgroundMusics[rand].length);
            }
        }
    }
}
