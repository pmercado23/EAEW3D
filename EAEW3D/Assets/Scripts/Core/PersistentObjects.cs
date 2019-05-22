using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Core
{
    public class PersistentObjects : MonoBehaviour
    {
        [SerializeField] GameObject persistantObjectPreFab;

        static bool hasSpawned = false;

        private void Awake()
        {
            if (hasSpawned) { return; }

            SpawnPersistentObjects();

            hasSpawned = true;

        }

        private void SpawnPersistentObjects()
        {
            GameObject persistantObject = Instantiate(persistantObjectPreFab);
            DontDestroyOnLoad(persistantObject);
        }
    }
}
