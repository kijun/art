using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereWithMaterialBlock : MonoBehaviour {
    public Color black, white, blue;
    public float Speed = 1, Offset;
    public SphereState state = SphereState.Black;

    public Color curColor;

    private Renderer _renderer;
    private MaterialPropertyBlock _propBlock;

    void Awake() {
        _propBlock = new MaterialPropertyBlock();
        _renderer = GetComponent<Renderer>();
        //Speed = Random.value * 5f;
        //Offset = Random.value;
        curColor = white;
    }

    void Update() {
        if (Input.GetKey(KeyCode.Alpha1)) {
            state = SphereState.Black;
        } else if (Input.GetKey(KeyCode.Alpha2)) {
            state = SphereState.White;
        } else if (Input.GetKey(KeyCode.Alpha3)) {
            state = SphereState.Lerp;
            Speed = 1000;
            Offset = Random.value * 0.4f;
        } else if (Input.GetKey(KeyCode.Alpha4)) {
            state = SphereState.Random;
            Speed = Random.value * 100f + 100f;
            Offset = Random.value;
        } else if (Input.GetKey(KeyCode.Alpha5)) {
            state = SphereState.Blue;
        } else if (Input.GetKey(KeyCode.Alpha6)) {
        } else if (Input.GetKey(KeyCode.Alpha7)) {
        } else if (Input.GetKey(KeyCode.Alpha8)) {
        } else if (Input.GetKey(KeyCode.Alpha9)) {
        }
        HandleState();
    }
    void HandleState() {
        _renderer.GetPropertyBlock(_propBlock);
        Color c = curColor;
        switch (state) {
            case SphereState.Black:
                c = black;
                break;
            case SphereState.White:
                c = white;
                break;
            case SphereState.Blue:
                c = blue;
                break;
            case SphereState.Lerp:
            case SphereState.Random:
                c = Color.Lerp(black, white, (Mathf.Sin(Time.time * Speed + Offset) + 2f) / 2f);
                break;

        }
        if (!c.RGBEquals(curColor)) {
            _propBlock.SetColor("_Color", c);
            curColor = c;
            _renderer.SetPropertyBlock(_propBlock);
        }
    }
}

public enum SphereState {
    Black, White, Blue, Lerp, Random
}
