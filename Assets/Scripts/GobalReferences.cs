using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace GobalReferences111
{
    public class GobalReferences : MonoBehaviour
    {
        public static GobalReferences Instance { get; set; }

        public GameObject bulletImpactEffectPrefab;

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
