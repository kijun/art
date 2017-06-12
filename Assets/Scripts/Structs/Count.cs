using System;

[Serializable]
public class Count {
    public int minimum; 			//Minimum value for our Count class.
    public int maximum; 			//Maximum value for our Count class.

    //Assignment constructor.
    public Count (int min, int max) {
        minimum = min;
        maximum = max;
    }
}

[Serializable]
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
