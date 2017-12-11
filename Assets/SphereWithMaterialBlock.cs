using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereWithMaterialBlock : MonoBehaviour {
    public Color colorOff, colorOnMin, colorOnMax;
    public float onSpeed, offSpeed, baseFlickerSpeed;

    Color _curColor;
    SphereState _state = SphereState.Off;
    Color _lerpStartColor;
    float _lerpStartTime;
    float _offset;
    float _flickerSpeed;

    Renderer _renderer;
    MaterialPropertyBlock _propBlock;

    void Awake() {
        _propBlock = new MaterialPropertyBlock();
        _renderer = GetComponent<Renderer>();
        _propBlock.SetColor("_Color", colorOff);
        _renderer.SetPropertyBlock(_propBlock);
        _curColor = colorOff;


        _offset = Random.value;
        _flickerSpeed = baseFlickerSpeed * Random.Range(0.5f, 1.5f);// ?
    }

    void Update() {
        if (Input.GetKey(KeyCode.Alpha1)) {
            // off
            _state = SphereState.Off;
            _lerpStartColor = _curColor;
            _lerpStartTime = Time.time;
        } else if (Input.GetKey(KeyCode.Alpha2)) {
            // on
            _state = SphereState.On;
            _lerpStartColor = _curColor;
            _lerpStartTime = Time.time;
        } else if (Input.GetKey(KeyCode.Alpha3)) {
            // max
            _state = SphereState.Max;
            _lerpStartColor = _curColor;
            _lerpStartTime = Time.time;
        } else if (Input.GetKey(KeyCode.Alpha4)) {
            /* ?
            state = SphereState.Random;
            Speed = Random.value * 100f + 100f;
            Offset = Random.value;
            */
        } else if (Input.GetKey(KeyCode.Alpha5)) {
            //state = SphereState.Blue;
        } else if (Input.GetKey(KeyCode.Alpha6)) {
        } else if (Input.GetKey(KeyCode.Alpha7)) {
        } else if (Input.GetKey(KeyCode.Alpha8)) {
        } else if (Input.GetKey(KeyCode.Alpha9)) {
        }
        HandleState();
    }
    void HandleState() {
        _renderer.GetPropertyBlock(_propBlock);
        Color c = _curColor;
        switch (_state) {
            case SphereState.Off:
                c = Color.Lerp(_lerpStartColor, colorOff, _lerpDuration() * offSpeed);
                break;
            case SphereState.On:
            case SphereState.Max:
                if (_lerpDuration() < 1f/onSpeed) {
                    c = Color.Lerp(_lerpStartColor, colorOnMin, _lerpDuration() * onSpeed);
                } else {
                    // flicker
                    c = Color.Lerp(colorOnMin, colorOnMax, (Mathf.Sin(_lerpDuration() * baseFlickerSpeed + _offset) + 1f) / 2f);
                }
                break;
        }
        if (!c.RGBEquals(_curColor)) {
            _propBlock.SetColor("_Color", c);
            _curColor = c;
            _renderer.SetPropertyBlock(_propBlock);
        }
    }

    float _lerpDuration() {
        return Time.time - _lerpStartTime;
    }
}

public enum SphereState {
    Off, On, Max //, Lerp, Random
}
