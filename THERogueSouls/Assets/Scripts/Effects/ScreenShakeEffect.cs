using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenShakeEffect : MonoBehaviour
{
    Transform _targetCamTransform;

    Vector3 _currentCameraVelocity = Vector3.zero;

    [SerializeField]
    float _shakeAmount, _shakeDuration, _shakeRate, _shakeSmoothRate;

    private void OnEnable()
    {
        _targetCamTransform = Camera.main.transform;
    }

    public void ShakeScreen()
    {
        InvokeRepeating("StartShake", 0, _shakeRate);
        Invoke("StopShake", _shakeDuration);
    }

    void StartShake()
    {
        if(_shakeAmount > 0)
        {
            Vector3 cameraPos = _targetCamTransform.position;
            float offsetX = Random.value * _shakeAmount * 2 - _shakeAmount;
            float offsetY = Random.value * _shakeAmount * 2 - _shakeAmount;
            cameraPos.x += offsetX;
            cameraPos.y += offsetY;

            _targetCamTransform.position = Vector3.SmoothDamp(_targetCamTransform.position, cameraPos, ref _currentCameraVelocity, _shakeSmoothRate);
        }
    }

    void StopShake()
    {
        CancelInvoke("StartShake");
    }

}
