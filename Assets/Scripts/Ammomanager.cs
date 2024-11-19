using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


namespace Ammomanager111
{
    public class Ammomanager : MonoBehaviour
    {
        public static Ammomanager Instance { get; set; }

        public TextMeshProUGUI ammodisplay1;
        public TextMeshProUGUI ammodisplay2;
        public TextMeshProUGUI ammodisplay3;
        public TextMeshProUGUI ammodisplay4;


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
