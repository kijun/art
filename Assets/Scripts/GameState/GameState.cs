using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public delegate void GameStateChangeRequestDelegate(GameState state);

public abstract class GameState : MonoBehaviour {
    public static GameState instance;

    protected GameStateChangeRequestDelegate onChangeDelegate;

    protected virtual void Awake() {
        if (instance == null) {
            instance = this;
        } else if (instance != this) {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

    public virtual void Run(GameStateChangeRequestDelegate onChange) {
        onChangeDelegate = onChange;
    }

    public abstract void CleanUp();
}


public class GameStateMenu {
}

