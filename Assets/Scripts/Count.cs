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
