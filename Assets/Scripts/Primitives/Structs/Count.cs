using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Count {
    public int minimum; 			//Minimum value for our Count class.
    public int maximum; 			//Maximum value for our Count class.

    //Assignment constructor.
    public Count (int min, int max) {
        minimum = min;
        maximum = max;
    }

    public int Length {
        get {
            return maximum - minimum;
        }
    }
}

[System.Serializable]
public class Range {
    public float minimum;
    public float maximum;

    //Assignment constructor.
    public Range (float val) {
        minimum = val;
        maximum = val;
    }

    //Assignment constructor.
    public Range (float min, float max) {
        minimum = min;
        maximum = max;
    }
}

public class RangeInt {
    public int minimum; 			//Minimum value for our Count class.
    public int maximum; 			//Maximum value for our Count class.

    //Assignment constructor.
    public RangeInt (int min, int max) {
        minimum = min;
        maximum = max;
    }

    public int Length {
        get {
            return maximum - minimum;
        }
    }

    public IEnumerable<int> Choose(int count = 0) {
        var indices = new HashSet<int>();
        while (indices.Count < count) {
            indices.Add(Random.Range(minimum, maximum));
        }

        foreach (var i in indices) {
            yield return i;
        }
    }
}
