using UnityEngine;
using UnityEngine.UI;

public class CameraEffectTester : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private SimpleCameraEffects _cameraEffects;
    [SerializeField] private Transform _testTarget;
    [SerializeField] private Text _statusText;

    [Header("Key Bindings")]
    [SerializeField] private KeyCode _pulseKey = KeyCode.P;
    [SerializeField] private KeyCode _shakeKey = KeyCode.S;
    [SerializeField] private KeyCode _resetKey = KeyCode.R;
    [SerializeField] private KeyCode _nextPresetKey = KeyCode.T;

    private void Update()
    {
        HandleInput();
        UpdateUI();
    }

    private void HandleInput()
    {
        if (Input.GetKeyDown(_pulseKey))
        {
            _cameraEffects.PlayPulseZoom();
        }

        if (Input.GetKeyDown(_shakeKey))
        {
            _cameraEffects.PlayShakeEffect();
        }

        if (Input.GetKeyDown(_resetKey))
        {
            _cameraEffects.ResetAllCameras();
        }

        if (Input.GetKeyDown(_nextPresetKey))
        {
            int nextPreset = (_cameraEffects.GetCurrentPresetIndex() + 1) % 3;
            _cameraEffects.ApplyPreset(nextPreset);
        }
    }

    private void UpdateUI()
    {
        if (_statusText == null) return;

        _statusText.text = $"CAMERA EFFECT TESTER\n" +
                          $"Current Transition: {_cameraEffects.GetCurrentPresetName()}\n\n" +
                          $"CONTROLS:\n" +
                          $"{_pulseKey} - Pulse Zoom\n" +
                          $"{_shakeKey} - Shake Effect\n" +
                          $"{_resetKey} - Reset Camera\n" +
                          $"{_nextPresetKey} - Next Transition Style";
    }
}