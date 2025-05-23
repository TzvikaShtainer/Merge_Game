using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using Zenject.ReflectionBaking.Mono.Cecil;

public class FModEvents : MonoBehaviour
{
    public static FModEvents Instance { get; private set; }

    [field: Header("Gameplay Music")]
    [field: SerializeField] public FMODUnity.EventReference GameplayMusic { get; private set; }


    [field: Header("Click")]
    [field: SerializeField] public FMODUnity.EventReference Click { get; private set; }

    [field: Header("UpgradeSpecificFruit")]
    [field: SerializeField] public FMODUnity.EventReference UpgradeSpecificFruit { get; private set; }

    [field: Header("Merge")]
    [field: SerializeField] public FMODUnity.EventReference Merge { get; private set; }

    [field: Header("DestroyAllLowestLevelFruit")]
    [field: SerializeField] public FMODUnity.EventReference DestroyAllLowestLevelFruit { get; private set; }

    [field: Header("RemoveSpecificFruit")]
    [field: SerializeField] public FMODUnity.EventReference RemovePowerup { get; private set; }

    [field: Header("Start Game")]
    [field: SerializeField] public FMODUnity.EventReference StartGame { get; private set; }

    [field: Header("ShakeBox")]
    [field: SerializeField] public FMODUnity.EventReference ShakeBox { get; private set; }


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}