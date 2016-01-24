using UnityEngine;
using System.Collections;

class CameraHelper {
    public static float Width {
        get {
            var halfScreenWidth = Camera.main.orthographicSize / Screen.height * Screen.width;
            return 2*halfScreenWidth;
        }
    }

    public static float Height {
        get {
            return 2 * Camera.main.orthographicSize;
        }
    }
}
