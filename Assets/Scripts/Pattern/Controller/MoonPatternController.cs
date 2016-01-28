using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class MoonPatternController : BasePatternController {
    private MoonPattern param;

    public override void Initialize(BasePattern bp) {
        param = (MoonPattern)bp;
    }

    public override IEnumerator Run() {
        // do your thing with moon param
        yield return null;
    }
}

