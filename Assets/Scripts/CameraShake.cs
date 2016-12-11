using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public float _decreasePerSecond = 1;

    private float _intensity = 0;
    private bool _shaking = false;
    private Vector3 _originPosition;
    private Quaternion _originRotation;

    void Awake()
    {
        Messenger.AddListener<float, float>("CameraShake", OnCameraShake);
    }

    void OnCameraShake(float intensity, float time)
    {
        intensity = Mathf.Clamp01(intensity);
        _intensity = intensity;

        if (!_shaking)
        {
            _originPosition = transform.position;
            _originRotation = transform.rotation;
        }
        _shaking = true;
        _decreasePerSecond = intensity / time;
    }

    void Update()
    {
        if (_intensity > 0)
        {
            transform.position = _originPosition + Random.insideUnitSphere * _intensity;
            transform.rotation = new Quaternion(
                _originRotation.x + Random.Range(-_intensity, _intensity) * .2f,
                _originRotation.y + Random.Range(-_intensity, _intensity) * .2f,
                _originRotation.z + Random.Range(-_intensity, _intensity) * .2f,
                _originRotation.w + Random.Range(-_intensity, _intensity) * .2f);
            _intensity -= (_decreasePerSecond * Time.deltaTime);

            if (_intensity <= 0)
            {
                _shaking = false;
            }
        }
    }
}
