using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;


public class GameManager : SingletonPersistent<GameManager>
{
    [SerializeField] private CharacterStats m_CharacterStats;
    public int Currency { get; private set; } = 0;


    public event Action CurrencyUpdated;
    public event Action LevelLost;

    void Start()
    {

    }

    public void UpdateCurrency(int changedAmount)
    {
        Currency += changedAmount;
    }
}