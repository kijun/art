using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public abstract class BasePatternController : MonoBehaviour {
    public abstract void Initialize(BasePattern bp);
    public abstract IEnumerator Run();
}


