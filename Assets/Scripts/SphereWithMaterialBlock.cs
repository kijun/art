using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MidiJack;

public class SphereWithMaterialBlock : MonoBehaviour {
    public Color colorOff, colorOnMin, colorOnMax;
    public float onSpeed, offSpeed, baseFlickerSpeed, baseVibrationSpeed;

    Color _curColor;
    SphereState _state = SphereState.Off;
    Color _lerpStartColor;
    float _lerpStartTime;
    float _offset;
    float _vibrationSpeed;
    Color _curColorOnMin;

    Renderer _renderer;
    MaterialPropertyBlock _propBlock;

    void Awake() {
        _propBlock = new MaterialPropertyBlock();
        _renderer = GetComponent<Renderer>();
        _propBlock.SetColor("_Color", colorOff);
        _renderer.SetPropertyBlock(_propBlock);
        _curColor = colorOff;

        _offset = Random.value;
        _vibrationSpeed = baseVibrationSpeed * Random.Range(0.5f, 2.5f);// ?
    }

    void Update() {
        var oldState = _state;

        // TODO optimize this shit
        _curColorOnMin = Color.Lerp(colorOnMin, colorOnMax, 1f-MidiMaster.GetKnob(77, 0));

        if (MidiMaster.GetKeyDown(36)) {
            _state = SphereState.On;
        } else if (MidiMaster.GetKeyDown(37)) {
            // on
            _state = SphereState.Off;
        } else if (Input.GetKey(KeyCode.Alpha3)) {
            // max
            _state = SphereState.Max;
        } else if (Input.GetKey(KeyCode.Alpha4)) {
            _state = SphereState.Flicker;
        } else if (Input.GetKey(KeyCode.Alpha5)) {
            //state = SphereState.Blue;
        } else if (Input.GetKey(KeyCode.Alpha6)) {
        } else if (Input.GetKey(KeyCode.Alpha7)) {
        } else if (Input.GetKey(KeyCode.Alpha8)) {
        } else if (Input.GetKey(KeyCode.Alpha9)) {
        }
        if (oldState != _state) {
            _lerpStartColor = _curColor;
            _lerpStartTime = Time.time;
        }
        HandleState();
    }
    void HandleState() {
        _renderer.GetPropertyBlock(_propBlock);
        Color c = _curColor;
        switch (_state) {
            case SphereState.Off:
                //c = Color.Lerp(_lerpStartColor, colorOff, _lerpDuration() * offSpeed);
                if (_lerpDuration() < 1f/offSpeed) {
                    c = Color.Lerp(_lerpStartColor, colorOff, _lerpDuration() * offSpeed + Random.Range(-0.1f, 0.1f));
                } else {
                    c = colorOff;//Color.Lerp(_lerpStartColor, _curColorOnMin, _lerpDuration() * onSpeed + Random.Range(-0.1f, 0.1f));
                    // flicker
                    //c = Color.Lerp(_curColorOnMin, colorOnMax, (Mathf.Sin(_lerpDuration() * _vibrationSpeed + _offset) + 1f) / 2f);
                }
                break;
            case SphereState.On:
                if (_lerpDuration() < 1f/onSpeed) {
                    c = Color.Lerp(_lerpStartColor, colorOnMax, _lerpDuration() * onSpeed + Random.Range(-0.005f, 0.005f));
                } else {
                    c = Color.Lerp(_curColorOnMin, colorOnMax, (Mathf.Sin(_lerpDuration() * _vibrationSpeed + _offset) + 2f) / 2f);
                    // flicker
                    //c = Color.Lerp(_curColorOnMin, colorOnMax, (Mathf.Sin(_lerpDuration() * _vibrationSpeed + _offset) + 1f) / 2f);
                }
                //c = Color.Lerp(_curColorOnMin, colorOnMax, (Mathf.Sin(_lerpDuration() * _vibrationSpeed + _offset) + 2f) / 2f);
                //var randSize = Random.Range(0.75f, 0.8f);
                //gameObject.transform.localScale = new Vector3(randSize, randSize, randSize);
                break;
            case SphereState.Max:
                if (_lerpDuration() < 1f/onSpeed) {
                    c = Color.Lerp(_lerpStartColor, _curColorOnMin, _lerpDuration() * onSpeed + Random.Range(-0.1f, 0.1f));
                } else {
                    // flicker
                    c = Color.Lerp(_curColorOnMin, colorOnMax, (Mathf.Sin(_lerpDuration() * _vibrationSpeed + _offset) + 1f) / 2f);
                }
                break;
            case SphereState.Flicker:
                c = Color.Lerp(colorOff, colorOnMax, (Mathf.Sin(_lerpDuration() * baseFlickerSpeed) + 1f) / 2f);
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
    Off, On, Flicker, Max //, Lerp, Random
}
