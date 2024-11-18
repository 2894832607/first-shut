using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SoundManager111
{
    public class SoundManager : MonoBehaviour
    {
        public static SoundManager Instance { get; set; }

        public AudioSource soundGun;
        public AudioSource reloadGun;
        public AudioSource emptyGun;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
            }
            else
            {
                Instance = this;
            }
        }
    }
}
