using System;
using UnityEngine.EventSystems;
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
    public int detectionArea = 20000;
    public bool showCam = false;

    /// <summary>
    /// Set the height of WebCamTexture.
    /// </summary>
    [SerializeField, TooltipAttribute ("Set the height of WebCamTexture.")]
    public int requestedHeight = 360;

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

    /// <summary>
    /// The kalman filter.
    /// </summary>
    KalmanFilter KF;

    /// <summary>
    /// The cursor pos.
    /// </summary>
    Point cursorPos;

    /// <summary>
    /// The measurement.
    /// </summary>
    Mat measurement;

    /// <summary>
    /// The predicted trajectory points.
    /// </summary>
    List<Point> predictedTrajectoryPoints = new List<Point>();

    /// <summary>
    /// The cursor trajectory points.
    /// </summary>
    List<Point> cursorTrajectoryPoints = new List<Point>();

    /// <summary>
    /// The estimated trajectory points.
    /// </summary>
    List<Point> estimatedTrajectoryPoints = new List<Point>();


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
        webCamTexture.requestedFPS = 60;
        webCamTexture.requestedWidth = 1280;
        webCamTexture.requestedHeight = 720;

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
                KF = new KalmanFilter (4, 2, 0, CvType.CV_32FC1);

                // intialization of KF...
                Mat transitionMat = new Mat (4, 4, CvType.CV_32F);
                transitionMat.put (0, 0, new float[] {1,0,1,0,   0,1,0,1,  0,0,1,0,  0,0,0,1});
                KF.set_transitionMatrix (transitionMat);

                measurement = new Mat (2, 1, CvType.CV_32FC1); measurement.setTo (Scalar.all(0));

                cursorPos = new Point ();
                GetCursorPos(cursorPos);

                // Set initial state estimate.
                Mat statePreMat = KF.get_statePre ();
                statePreMat.put (0, 0, new float[] {(float)cursorPos.x,(float)cursorPos.y,0,0});
                Mat statePostMat = KF.get_statePost ();
                statePostMat.put (0, 0, new float[] {(float)cursorPos.x,(float)cursorPos.y,0,0});

                Mat measurementMat = new Mat (2, 4, CvType.CV_32FC1);
                Core.setIdentity (measurementMat);
                KF.set_measurementMatrix (measurementMat);

                Mat processNoiseCovMat = new Mat (4, 4, CvType.CV_32FC1);
                Core.setIdentity (processNoiseCovMat, Scalar.all(1e-4));
                KF.set_processNoiseCov (processNoiseCovMat);

                Mat measurementNoiseCovMat = new Mat (2, 2, CvType.CV_32FC1);
                Core.setIdentity (measurementNoiseCovMat, Scalar.all(10));
                KF.set_measurementNoiseCov (measurementNoiseCovMat);

                Mat errorCovPostMat = new Mat (4, 4, CvType.CV_32FC1);
                Core.setIdentity (errorCovPostMat, Scalar.all(.1));
                KF.set_errorCovPost (errorCovPostMat);

                break;
            } else {
                yield return null;
            }
        }
        //StartCoroutine(Trace());
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
            //texture = new Texture2D (webCamTexture.width, webCamTexture.height, TextureFormat.RGBA32, false);
            texture = new Texture2D (webCamTexture.width, webCamTexture.height, TextureFormat.RGBA32, false);

        rgbaMat = new Mat (webCamTexture.height, webCamTexture.width, CvType.CV_8UC4);
        /*
        rgbaMat2 = new Mat (webCamTexture.height, webCamTexture.width, CvType.CV_8UC4);
        rgbaMat3 = new Mat (webCamTexture.height, webCamTexture.width, CvType.CV_8UC4);
        rgbaMatFinal = new Mat (webCamTexture.height, webCamTexture.width, CvType.CV_8UC4);
        */
        rgbaMat2 = new Mat (requestedHeight, requestedWidth, CvType.CV_8UC4);
        rgbaMat3 = new Mat (requestedHeight, requestedWidth, CvType.CV_8UC4);
        rgbaMatFinal = new Mat (requestedHeight, requestedWidth, CvType.CV_8UC4);

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
        if (KF != null) TraceUpdate();
        return;
        framecnt++;
        if (hasInitDone && webCamTexture.isPlaying && webCamTexture.didUpdateThisFrame) {
            if (framecnt % 1 == 0) {
                Utils.webCamTextureToMat (webCamTexture, rgbaMat, colors);
                Imgproc.resize(rgbaMat, rgbaMatFinal, new Size(requestedWidth, requestedHeight));
                //Imgproc.blur(rgbaMat, rgbaMat2, new Size(5, 5));

                //Imgproc.putText (rgbaMat, "W:" + rgbaMat.width () + " H:" + rgbaMat.height () + " SO:" + Screen.orientation, new Point (5, rgbaMat.rows () - 10), Core.FONT_HERSHEY_SIMPLEX, 1.0, new Scalar (255, 255, 255, 255), 2, Imgproc.LINE_AA, false);
                Mat imgHSV;
                Imgproc.cvtColor(rgbaMatFinal, rgbaMat2, Imgproc.COLOR_RGB2HSV);

                Core.inRange(rgbaMat2, new Scalar(iLowH, iLowS, iLowV), new Scalar(iHighH, iHighS, iHighV), rgbaMat3);
                Imgproc.dilate(rgbaMat3, rgbaMat3, Imgproc.getStructuringElement(Imgproc.MORPH_ELLIPSE, new Size(4,4)));
                Imgproc.erode(rgbaMat3, rgbaMat3, Imgproc.getStructuringElement(Imgproc.MORPH_ELLIPSE, new Size(4,4)));
                Imgproc.erode(rgbaMat3, rgbaMat3, Imgproc.getStructuringElement(Imgproc.MORPH_ELLIPSE, new Size(4,4)));
                Imgproc.erode(rgbaMat3, rgbaMat3, Imgproc.getStructuringElement(Imgproc.MORPH_ELLIPSE, new Size(4,4)));
                Imgproc.erode(rgbaMat3, rgbaMat3, Imgproc.getStructuringElement(Imgproc.MORPH_ELLIPSE, new Size(4,4)));
                Imgproc.erode(rgbaMat3, rgbaMat3, Imgproc.getStructuringElement(Imgproc.MORPH_ELLIPSE, new Size(4,4)));

                var moments = Imgproc.moments(rgbaMat3);
                var dM01 = moments.m01;
                var dM10 = moments.m10;
                var dArea = moments.m00;
                // if the area <= 10000, I consider that the there are no object in the image and it's because of the noise, the area is not zero
                if (dArea > detectionArea) {
                    //calculate the position of the ball
                    var posX = dM10 / dArea;
                    var posY = dM01 / dArea;
                    Debug.Log("X = " + posX + " Y = " + posY + " Area = " + dArea);
                    //if (showCam) {
                    //    Utils.matToTexture2D (rgbaMat, texture, colors);
                    //} else {
                        Utils.matToTexture2D (rgbaMat3, texture, colors);
                    //}
                } else {
                    Debug.Log("not found " + dArea);
                    if (showCam) {
                        Utils.matToTexture2D (rgbaMat, texture, colors);
                    }
                }

            } else {
                //Utils.webCamTextureToMat (webCamTexture, rgbaMat, colors);
                //Utils.matToTexture2D (rgbaMat, texture, colors);
            }
        }
    }

    void TraceUpdate() {
        Imgproc.rectangle (
                rgbaMatFinal,
                new Point (0, 0),
                new Point (rgbaMatFinal.width (), rgbaMatFinal.height ()),
                new Scalar (0, 0, 0, 255), -1);
        Point predictedPt;
        Point estimatedPt;

        // First predict, to update the internal statePre variable.
        using (Mat prediction = KF.predict ()) {
            predictedPt = new Point (prediction.get (0, 0) [0], prediction.get (1, 0) [0]);
        }

        // Get cursor point.
        var pos = GetTrackerPosition();
        measurement.put (0, 0, new float[] {(float)pos.x,(float)pos.y});

        Point measurementPt = new Point(measurement.get (0, 0)[0], measurement.get (1, 0)[0]);

        // The update phase.
        using (Mat estimated = KF.correct (measurement)) {
            estimatedPt = new Point (estimated.get (0, 0) [0], estimated.get (1, 0) [0]);
        }

        predictedTrajectoryPoints.Add (predictedPt);
        cursorTrajectoryPoints.Add (measurementPt);
        estimatedTrajectoryPoints.Add (estimatedPt);

        DrawCross(rgbaMatFinal, measurementPt, new Scalar(0,255,0,255), 300 );
        DrawCross(rgbaMatFinal, estimatedPt, new Scalar(255,0,0,255), 300 );

        /*
        for (int i = 0; i < predictedTrajectoryPoints.Count-1; i++) {
            Imgproc.line(rgbaMat, predictedTrajectoryPoints[i], predictedTrajectoryPoints[i+1], new Scalar(0,255,255,i), 1);
        }
        */

        for (int i = 0; i < estimatedTrajectoryPoints.Count-1; i++) {
            Imgproc.line(rgbaMatFinal, estimatedTrajectoryPoints[i], estimatedTrajectoryPoints[i+1], new Scalar(255,255,255,255), 10);
        }

        if (predictedTrajectoryPoints.Count > 255) predictedTrajectoryPoints.RemoveAt (0);
        if (cursorTrajectoryPoints.Count > 255) cursorTrajectoryPoints.RemoveAt (0);
        if (estimatedTrajectoryPoints.Count > 255) estimatedTrajectoryPoints.RemoveAt (0);

        Utils.matToTexture2D (rgbaMatFinal, texture, colors);
    }

    Vector2 _GetTrackerPosition() {
        if (hasInitDone && webCamTexture.isPlaying && webCamTexture.didUpdateThisFrame) {
            Utils.webCamTextureToMat (webCamTexture, rgbaMat, colors);
            Imgproc.cvtColor(rgbaMat, rgbaMat2, Imgproc.COLOR_RGB2HSV);

            Core.inRange(rgbaMat2, new Scalar(iLowH, iLowS, iLowV), new Scalar(iHighH, iHighS, iHighV), rgbaMat3);
            Imgproc.dilate(rgbaMat3, rgbaMat3, Imgproc.getStructuringElement(Imgproc.MORPH_ELLIPSE, new Size(4,4)));
            Imgproc.erode(rgbaMat3, rgbaMat3, Imgproc.getStructuringElement(Imgproc.MORPH_ELLIPSE, new Size(4,4)));
            Imgproc.erode(rgbaMat3, rgbaMat3, Imgproc.getStructuringElement(Imgproc.MORPH_ELLIPSE, new Size(4,4)));
            Imgproc.erode(rgbaMat3, rgbaMat3, Imgproc.getStructuringElement(Imgproc.MORPH_ELLIPSE, new Size(4,4)));

            var moments = Imgproc.moments(rgbaMat3);
            var dM01 = moments.m01;
            var dM10 = moments.m10;
            var dArea = moments.m00;
            // if the area <= 10000, I consider that the there are no object in the image and it's because of the noise, the area is not zero
            if (dArea > 50000) {
                //calculate the position of the ball
                var posX = dM10 / dArea;
                var posY = dM01 / dArea;
                Debug.Log("X = " + posX + " Y = " + posY + " Area = " + dArea);
                return new Vector2(webCamTexture.width-(float)posX, (float)posY);
            }
        }
        Debug.Log("not found");
        return Vector2.zero;
    }

    int nfCnt = 0;
    int frameCnt = 0;
    Vector2 prevPos;
    // should really use kalman
    Vector2 GetTrackerPosition() {
        var pos = prevPos;
        if (frameCnt % 3 == 0) {
            pos = _GetTrackerPosition();
        }
        if (pos == Vector2.zero) {
            nfCnt++;
            //if (nfCnt < 4) {
                pos = prevPos;
            //}
        } else {
            nfCnt = 0;
            prevPos = pos;
        }
        frameCnt++;
        return pos;
    }

    /*
    IEnumerator Trace() {
        while (true) {
            // if green visible on screen, start drawing
            //
            float sec = 0.3f;
            var pos = GetTrackerPosition();
            if (pos != Vector2.zero) {
                var sp = new SplineParams();
                var p1 = Camera.main.ScreenToWorldPoint(pos);
                yield return new WaitForSeconds(sec);
                pos = GetTrackerPosition();
                var p2 = Camera.main.ScreenToWorldPoint(pos);
                yield return new WaitForSeconds(sec);
                pos = GetTrackerPosition();
                var p3 = Camera.main.ScreenToWorldPoint(pos);
                yield return new WaitForSeconds(sec);
                pos = GetTrackerPosition();
                var p4 = Camera.main.ScreenToWorldPoint(pos);
                yield return new WaitForSeconds(sec);
                sp.spline = new BezierSpline2D(p1, p2, p3, p4);
                pos = GetTrackerPosition();
                while (pos != Vector2.zero) {
                    yield return new WaitForSeconds(sec);
                    p1 = Camera.main.ScreenToWorldPoint(pos);
                    yield return new WaitForSeconds(sec);
                    pos = GetTrackerPosition();
                    p2 = Camera.main.ScreenToWorldPoint(pos);
                    yield return new WaitForSeconds(sec);
                    pos = GetTrackerPosition();
                    p3 = Camera.main.ScreenToWorldPoint(pos);
                    sp.spline.AddCurve(p1, p2, p3);
                }
                sp.color = Color.white.WithAlpha(0);
                //sp.width = Random.Range(0.2f, 2f);
                sp.width = 0.6f;
                Animatable2[] anims = NoteFactory.CreateLine(sp);
                int idx = 0;
                foreach (var l in anims) {
                    l.AddAnimationCurve(AnimationKeyPath.Opacity, AnimationCurve.Linear(0, 0, 1, 1));
                    idx++;
                    if (idx % 15 == 0) {
                        //yield return null;// new WaitForSeconds(0.01f);
                        yield return new WaitForSeconds(0.01f);
                    }
                }

                yield return new WaitForSeconds(2f);
                foreach (var l in anims) {
                    //l.velocity = RandomHelper.RandomVector2(-0.5f, 0.5f, -0.5f, 0.5f);
                    //l.angularVelocity = Random.Range(-90f, 90f);
                    l.AddAnimationCurve(AnimationKeyPath.Opacity, AnimationCurve.EaseInOut(0, 1, 3, 0));
                    l.DestroyIn(2f);
                }
            }
            yield return new WaitForSeconds(sec);
        }
    }
    */

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

    /// <summary>
    /// Gets cursor pos.
    /// </summary>
    /// <returns>The cursor point.</returns>
    private void GetCursorPos (Point pos)
    {
        #if ((UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR)
        //Touch
        int touchCount = Input.touchCount;
        if (touchCount >= 1)
        {
            Touch t = Input.GetTouch(0);
            ConvertScreenPointToTexturePoint (new Point (t.position.x, t.position.y), pos, gameObject, rgbaMat.cols(), rgbaMat.rows());
        }
        #else
        //Mouse
        ConvertScreenPointToTexturePoint (new Point (Input.mousePosition.x, Input.mousePosition.y), pos, gameObject, rgbaMat.cols(), rgbaMat.rows());
        #endif
    }

    /// <summary>
    /// Converts the screen point to texture point.
    /// </summary>
    /// <param name="screenPoint">Screen point.</param>
    /// <param name="dstPoint">Dst point.</param>
    /// <param name="texturQuad">Texture quad.</param>
    /// <param name="textureWidth">Texture width.</param>
    /// <param name="textureHeight">Texture height.</param>
    /// <param name="camera">Camera.</param>
    private void ConvertScreenPointToTexturePoint (Point screenPoint, Point dstPoint, GameObject textureQuad, int textureWidth = -1, int textureHeight = -1, Camera camera = null)
    {
        if (textureWidth < 0 || textureHeight < 0) {
            Renderer r = textureQuad.GetComponent<Renderer> ();
            if (r != null && r.material != null && r.material.mainTexture != null) {
                textureWidth = r.material.mainTexture.width;
                textureHeight = r.material.mainTexture.height;
            } else {
                textureWidth = (int)textureQuad.transform.localScale.x;
                textureHeight = (int)textureQuad.transform.localScale.y;
            }
        }

        if (camera == null)
            camera = Camera.main;

        Vector3 quadPosition = textureQuad.transform.localPosition;
        Vector3 quadScale = textureQuad.transform.localScale;

        Vector2 tl = camera.WorldToScreenPoint (new Vector3 (quadPosition.x - quadScale.x / 2, quadPosition.y + quadScale.y / 2, quadPosition.z));
        Vector2 tr = camera.WorldToScreenPoint (new Vector3 (quadPosition.x + quadScale.x / 2, quadPosition.y + quadScale.y / 2, quadPosition.z));
        Vector2 br = camera.WorldToScreenPoint (new Vector3 (quadPosition.x + quadScale.x / 2, quadPosition.y - quadScale.y / 2, quadPosition.z));
        Vector2 bl = camera.WorldToScreenPoint (new Vector3 (quadPosition.x - quadScale.x / 2, quadPosition.y - quadScale.y / 2, quadPosition.z));

        using(Mat srcRectMat = new Mat (4, 1, CvType.CV_32FC2))
        using(Mat dstRectMat = new Mat (4, 1, CvType.CV_32FC2)) {
            srcRectMat.put (0, 0, tl.x, tl.y, tr.x, tr.y, br.x, br.y, bl.x, bl.y);
            dstRectMat.put (0, 0, 0, 0, quadScale.x, 0, quadScale.x, quadScale.y, 0, quadScale.y);

            using(Mat perspectiveTransform = Imgproc.getPerspectiveTransform (srcRectMat, dstRectMat))
            using(MatOfPoint2f srcPointMat = new MatOfPoint2f (screenPoint))
            using(MatOfPoint2f dstPointMat = new MatOfPoint2f ()) {
                Core.perspectiveTransform (srcPointMat, dstPointMat, perspectiveTransform);

                dstPoint.x = dstPointMat.get(0,0)[0] * textureWidth / quadScale.x;
                dstPoint.y = dstPointMat.get(0,0)[1] * textureHeight / quadScale.y;
            }
        }
    }

    /// <summary>
    /// Draws Cross.
    /// </summary>
    /// <param name="img">Img.</param>
    /// <param name="center">Center.</param>
    /// <param name="color">Color.</param>
    /// <param name="radius">Radius.</param>
    private void DrawCross (Mat img, Point center, Scalar color, int radius)
    {
        float d = Mathf.Sqrt (radius);
        Imgproc.line(img, new Point( center.x - d, center.y - d ), new Point( center.x + d, center.y + d ), color, 2);
        Imgproc.line(img, new Point( center.x + d, center.y - d ), new Point( center.x - d, center.y + d ), color, 2);
    }
}

