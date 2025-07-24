using System;
using UnityEngine;
using UnityEngine.UI;

public class ControlsUIManager : MonoBehaviour
{
    [SerializeField] private PlayerThirdPersonCameraController cameraController;
    [SerializeField] private Slider mouseSensitivitySlider;

    private float startSensitivity;
    private float maxSensitivity;
    private float minSensitivity;

    private void Start()
    {
        startSensitivity = cameraController.GetRotationSpeed();
        maxSensitivity = startSensitivity * 2;
        minSensitivity = 0.05f;

        mouseSensitivitySlider.maxValue = maxSensitivity;
        mouseSensitivitySlider.minValue = minSensitivity;
        mouseSensitivitySlider.value = startSensitivity;

        mouseSensitivitySlider.onValueChanged.AddListener(HandleMouseSensSliderChange);
    }

    private void HandleMouseSensSliderChange(float arg0)
    {
        cameraController.SetRotationSpeed(arg0);
    }

    private void OnDestroy()
    {
        mouseSensitivitySlider.onValueChanged.RemoveListener(HandleMouseSensSliderChange);
    }
}
