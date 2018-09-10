using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_5_3 || UNITY_5_3_OR_NEWER
using UnityEngine.SceneManagement;
#endif
using OpenCVForUnity;
using OpenCVForUnityExample;

/// <summary>
/// WebCamTextureToMat Example
/// An example of converting a WebCamTexture image to OpenCV's Mat format.
/// </summary>
public class Tracer : MonoBehaviour {
    /// <summary>
    /// Set the name of the device to use.
    /// </summary>
    [SerializeField, TooltipAttribute ("Set the name of the device to use.")]
    public string requestedDeviceName = null;

    /// <summary>
    /// Set the width of WebCamTexture.
    /// </summary>
    [SerializeField, TooltipAttribute ("Set the width of WebCamTexture.")]
    public int requestedWidth = 640;

    public int iLowH = 155;
    public int iHighH = 200;

    public int iLowS = 130;
    public int iHighS = 255;

    public int iLowV = 50;
    public int iHighV = 255;

    /// <summary>
    /// Set the height of WebCamTexture.
    /// </summary>
    [SerializeField, TooltipAttribute ("Set the height of WebCamTexture.")]
    public int requestedHeight = 480;

    /// <summary>
    /// Set FPS of WebCamTexture.
    /// </summary>
    [SerializeField, TooltipAttribute ("Set FPS of WebCamTexture.")]
    public int requestedFPS = 60;

    /// <summary>
    /// Set whether to use the front facing camera.
    /// </summary>
    [SerializeField, TooltipAttribute ("Set whether to use the front facing camera.")]
    public bool requestedIsFrontFacing = false;

    /// <summary>
    /// The webcam texture.
    /// </summary>
    WebCamTexture webCamTexture;

    /// <summary>
    /// The webcam device.
    /// </summary>
    WebCamDevice webCamDevice;

    /// <summary>
    /// The rgba mat.
    /// </summary>
    Mat rgbaMat;
    Mat rgbaMat2;
    Mat rgbaMat3;
    Mat rgbaMatFinal;

    /// <summary>
    /// The colors.
    /// </summary>
    Color32[] colors;

    /// <summary>
    /// The texture.
    /// </summary>
    Texture2D texture;

    /// <summary>
    /// Indicates whether this instance is waiting for initialization to complete.
    /// </summary>
    bool isInitWaiting = false;

    /// <summary>
    /// Indicates whether this instance has been initialized.
    /// </summary>
    bool hasInitDone = false;

    /// <summary>
    /// The FPS monitor.
    /// </summary>
    FpsMonitor fpsMonitor;

    // Use this for initialization
    void Start ()
    {
        fpsMonitor = GetComponent<FpsMonitor> ();

        Initialize ();
    }

    /// <summary>
    /// Initializes webcam texture.
    /// </summary>
    private void Initialize ()
    {
        if (isInitWaiting)
            return;

        StartCoroutine (_Initialize ());
    }

    /// <summary>
    /// Initializes webcam texture by coroutine.
    /// </summary>
    private IEnumerator _Initialize ()
    {
        if (hasInitDone)
            Dispose ();

        isInitWaiting = true;

        // Creates the camera
        if (!String.IsNullOrEmpty (requestedDeviceName)) {
            int requestedDeviceIndex = -1;
            if (Int32.TryParse (requestedDeviceName, out requestedDeviceIndex)) {
                if (requestedDeviceIndex >= 0 && requestedDeviceIndex < WebCamTexture.devices.Length) {
                    webCamDevice = WebCamTexture.devices [requestedDeviceIndex];
                    webCamTexture = new WebCamTexture (webCamDevice.name, requestedWidth, requestedHeight, requestedFPS);
                }
            } else {
                for (int cameraIndex = 0; cameraIndex < WebCamTexture.devices.Length; cameraIndex++) {
                    if (WebCamTexture.devices [cameraIndex].name == requestedDeviceName) {
                        webCamDevice = WebCamTexture.devices [cameraIndex];
                        webCamTexture = new WebCamTexture (webCamDevice.name, requestedWidth, requestedHeight, requestedFPS);
                        break;
                    }
                }
            }
            if (webCamTexture == null)
                Debug.Log ("Cannot find camera device " + requestedDeviceName + ".");
        }

        if (webCamTexture == null) {
            // Checks how many and which cameras are available on the device
            for (int cameraIndex = 0; cameraIndex < WebCamTexture.devices.Length; cameraIndex++) {
                if (WebCamTexture.devices [cameraIndex].isFrontFacing == requestedIsFrontFacing) {
                    webCamDevice = WebCamTexture.devices [cameraIndex];
                    webCamTexture = new WebCamTexture (webCamDevice.name, requestedWidth, requestedHeight, requestedFPS);
                    break;
                }
            }
        }

        if (webCamTexture == null) {
            if (WebCamTexture.devices.Length > 0) {
                webCamDevice = WebCamTexture.devices [0];
                webCamTexture = new WebCamTexture (webCamDevice.name, requestedWidth, requestedHeight, requestedFPS);
            } else {
                Debug.LogError ("Camera device does not exist.");
                isInitWaiting = false;
                yield break;
            }
        }

        // Starts the camera.
        webCamTexture.Play ();

        while (true) {
            // If you want to use webcamTexture.width and webcamTexture.height on iOS, you have to wait until webcamTexture.didUpdateThisFrame == 1, otherwise these two values will be equal to 16. (http://forum.unity3d.com/threads/webcamtexture-and-error-0x0502.123922/).
            #if UNITY_IOS && !UNITY_EDITOR && (UNITY_4_6_3 || UNITY_4_6_4 || UNITY_5_0_0 || UNITY_5_0_1)
            if (webCamTexture.width > 16 && webCamTexture.height > 16) {
            #else
            if (webCamTexture.didUpdateThisFrame) {
                #if UNITY_IOS && !UNITY_EDITOR && UNITY_5_2
                while (webCamTexture.width <= 16) {
                    webCamTexture.GetPixels32 ();
                    yield return new WaitForEndOfFrame ();
                }
                #endif
                #endif

                Debug.Log ("name:" + webCamTexture.deviceName + " width:" + webCamTexture.width + " height:" + webCamTexture.height + " fps:" + webCamTexture.requestedFPS);
                Debug.Log ("videoRotationAngle:" + webCamTexture.videoRotationAngle + " videoVerticallyMirrored:" + webCamTexture.videoVerticallyMirrored + " isFrongFacing:" + webCamDevice.isFrontFacing);

                isInitWaiting = false;
                hasInitDone = true;

                OnInited ();

                break;
            } else {
                yield return null;
            }
        }
    }

    /// <summary>
    /// Releases all resource.
    /// </summary>
    private void Dispose ()
    {
        isInitWaiting = false;
        hasInitDone = false;

        if (webCamTexture != null) {
            webCamTexture.Stop ();
            WebCamTexture.Destroy (webCamTexture);
            webCamTexture = null;
        }
        if (rgbaMat != null) {
            rgbaMat.Dispose ();
            rgbaMat = null;
        }
        if (texture != null) {
            Texture2D.Destroy(texture);
            texture = null;
        }
    }

    /// <summary>
    /// Raises the webcam texture initialized event.
    /// </summary>
    private void OnInited ()
    {
        if (colors == null || colors.Length != webCamTexture.width * webCamTexture.height)
            colors = new Color32[webCamTexture.width * webCamTexture.height];
        if (texture == null || texture.width != webCamTexture.width || texture.height != webCamTexture.height)
            texture = new Texture2D (webCamTexture.width, webCamTexture.height, TextureFormat.RGBA32, false);

        rgbaMat = new Mat (webCamTexture.height, webCamTexture.width, CvType.CV_8UC4);
        rgbaMat2 = new Mat (webCamTexture.height, webCamTexture.width, CvType.CV_8UC4);
        rgbaMat3 = new Mat (webCamTexture.height, webCamTexture.width, CvType.CV_8UC4);
        rgbaMatFinal = new Mat (webCamTexture.height, webCamTexture.width, CvType.CV_8UC4);

        gameObject.GetComponent<Renderer> ().material.mainTexture = texture;

        gameObject.transform.localScale = new Vector3 (webCamTexture.width, webCamTexture.height, 1);
        Debug.Log ("Screen.width " + Screen.width + " Screen.height " + Screen.height + " Screen.orientation " + Screen.orientation);

        if (fpsMonitor != null){
            fpsMonitor.Add ("width", rgbaMat.width ().ToString());
            fpsMonitor.Add ("height", rgbaMat.height ().ToString());
            fpsMonitor.Add ("orientation", Screen.orientation.ToString());
        }


        float width = rgbaMat.width ();
        float height = rgbaMat.height ();

        float widthScale = (float)Screen.width / width;
        float heightScale = (float)Screen.height / height;
        if (widthScale < heightScale) {
            Camera.main.orthographicSize = (width * (float)Screen.height / (float)Screen.width) / 2;
        } else {
            Camera.main.orthographicSize = height / 2;
        }
    }

    int framecnt = 0;
    // Update is called once per frame
    void Update ()
    {
        framecnt++;
        if (hasInitDone && webCamTexture.isPlaying && webCamTexture.didUpdateThisFrame) {
            if (framecnt % 1 == 0) {
                Utils.webCamTextureToMat (webCamTexture, rgbaMat, colors);
                //Imgproc.blur(rgbaMat, rgbaMat2, new Size(5, 5));

                //Imgproc.putText (rgbaMat, "W:" + rgbaMat.width () + " H:" + rgbaMat.height () + " SO:" + Screen.orientation, new Point (5, rgbaMat.rows () - 10), Core.FONT_HERSHEY_SIMPLEX, 1.0, new Scalar (255, 255, 255, 255), 2, Imgproc.LINE_AA, false);
                Mat imgHSV;
                Imgproc.cvtColor(rgbaMat, rgbaMat2, Imgproc.COLOR_RGB2HSV);

                //Imgproc.dilate(rgbaMat, rgbaMat, Imgproc.getStructuringElement(Imgproc.MORPH_ELLIPSE, new Size(3,3)));
                //Imgproc.dilate(rgbaMat, rgbaMat, Imgproc.getStructuringElement(Imgproc.MORPH_ELLIPSE, new Size(8,8)));
                //Imgproc.erode(rgbaMat, rgbaMat, Imgproc.getStructuringElement(Imgproc.MORPH_ELLIPSE, new Size(3,3)));
                Core.inRange(rgbaMat2, new Scalar(iLowH, iLowS, iLowV), new Scalar(iHighH, iHighS, iHighV), rgbaMat3);
                Imgproc.dilate(rgbaMat3, rgbaMat3, Imgproc.getStructuringElement(Imgproc.MORPH_ELLIPSE, new Size(4,4)));
                Imgproc.erode(rgbaMat3, rgbaMat3, Imgproc.getStructuringElement(Imgproc.MORPH_ELLIPSE, new Size(3,3)));
                Imgproc.erode(rgbaMat3, rgbaMat3, Imgproc.getStructuringElement(Imgproc.MORPH_ELLIPSE, new Size(3,3)));
                Imgproc.erode(rgbaMat3, rgbaMat3, Imgproc.getStructuringElement(Imgproc.MORPH_ELLIPSE, new Size(3,3)));
                Imgproc.erode(rgbaMat3, rgbaMat3, Imgproc.getStructuringElement(Imgproc.MORPH_ELLIPSE, new Size(3,3)));
                /*
                Imgproc.dilate(rgbaMat3, rgbaMat3, Imgproc.getStructuringElement(Imgproc.MORPH_ELLIPSE, new Size(8,8)));
                Imgproc.dilate(rgbaMat3, rgbaMat3, Imgproc.getStructuringElement(Imgproc.MORPH_ELLIPSE, new Size(8,8)));
                Imgproc.erode(rgbaMat3, rgbaMat3, Imgproc.getStructuringElement(Imgproc.MORPH_ELLIPSE, new Size(3,3)));
                Imgproc.erode(rgbaMat3, rgbaMat3, Imgproc.getStructuringElement(Imgproc.MORPH_ELLIPSE, new Size(3,3)));
                */

                var moments = Imgproc.moments(rgbaMat3);
                var dM01 = moments.m01;
                var dM10 = moments.m10;
                var dArea = moments.m00;
                // if the area <= 10000, I consider that the there are no object in the image and it's because of the noise, the area is not zero
                if (dArea > 20*20) {
                    //calculate the position of the ball
                    var posX = dM10 / dArea;
                    var posY = dM01 / dArea;
                    Debug.Log("X = " + posX + " Y = " + posY + " Area = " + dArea);
                } else {
                    Debug.Log("not found");
                }

                Utils.matToTexture2D (rgbaMat3, texture, colors);
            } else {
                //Utils.webCamTextureToMat (webCamTexture, rgbaMat, colors);
                //Utils.matToTexture2D (rgbaMat, texture, colors);
            }
        }
    }

    /// <summary>
    /// Raises the destroy event.
    /// </summary>
    void OnDestroy ()
    {
        Dispose ();
    }

    /// <summary>
    /// raises the play button click event.
    /// </summary>
    /*
    public void onplaybuttonclick ()
    {
        if (hasinitdone)
            webcamtexture.play ();
    }

    /// <summary>
    /// raises the pause button click event.
    /// </summary>
    public void onpausebuttonclick ()
    {
        if (hasinitdone)
            webcamtexture.pause ();
    }
    */

    /// <summary>
    /// Raises the stop button click event.
    /// </summary>
    public void OnStopButtonClick ()
    {
        if (hasInitDone)
            webCamTexture.Stop ();
    }

    /// <summary>
    /// Raises the change camera button click event.
    /// </summary>
    public void OnChangeCameraButtonClick ()
    {
        if (hasInitDone) {
            requestedDeviceName = null;
            requestedIsFrontFacing = !requestedIsFrontFacing;
            Initialize ();
        }
    }
}
