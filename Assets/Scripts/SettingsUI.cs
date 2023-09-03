using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SettingsUI : MonoBehaviour
{
    [SerializeField] Slider edgeTolerance;
    [SerializeField] TextMeshProUGUI edgeToleranceTxt;
    [SerializeField] Slider musicVolume;
    [SerializeField] Slider sfxVolume;
    [SerializeField] Slider lerp;
    [SerializeField] TextMeshProUGUI lerpText;
    [SerializeField] Slider ballSpeed;
    [SerializeField] TextMeshProUGUI ballSpeedText;

    void Start()
    {
        edgeTolerance.onValueChanged.AddListener((value) => { Settings.Instance.SetMovementEdgeTolerance(value / 10); });
        edgeTolerance.onValueChanged.AddListener((value) => { UpdateEdgeToleranceText(value / 10); });
        musicVolume.onValueChanged.AddListener((value) => { Settings.Instance.SetMusicVolume(value); });
        sfxVolume.onValueChanged.AddListener((value) => { Settings.Instance.SetSFXVolume(value); });
        lerp.onValueChanged.AddListener((value) => { Settings.Instance.SetLerp(value / 100f); });
        lerp.onValueChanged.AddListener((value) => { UpdateLerpText(value / 100f); });
        ballSpeed.onValueChanged.AddListener((value) => { Settings.Instance.SetBallSpeed(value / 10f); });
        ballSpeed.onValueChanged.AddListener((value) => { UpdateBallSpeedText(value / 10f); });
    }

    private void OnEnable()
    {
        edgeTolerance.value = Settings.Instance.MovementEdgeTolerance * 10;
        UpdateEdgeToleranceText(Settings.Instance.MovementEdgeTolerance);
        musicVolume.value = Settings.Instance.MasterVolume;
        sfxVolume.value = Settings.Instance.SFXVolume;
        lerp.value = Settings.Instance.Lerp * 100f;
        UpdateLerpText(Settings.Instance.Lerp);
        ballSpeed.value = Settings.Instance.BallSpeed * 10;
        UpdateBallSpeedText(Settings.Instance.BallSpeed);
    }

    private void UpdateEdgeToleranceText(float newValue) => edgeToleranceTxt.text = newValue.ToString("0.#");

    private void UpdateLerpText(float newValue) => lerpText.text = newValue.ToString("0.##");
    private void UpdateBallSpeedText(float newValue) => ballSpeedText.text = newValue.ToString("0.#");
}
