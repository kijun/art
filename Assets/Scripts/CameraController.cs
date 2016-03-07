using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {
    public delegate void PostLockDelegate();

    public static CameraController instance;
    public float trackingDuration = 1.3f;

    private PlayerController player;
    private bool shaking;
    private bool locked;

    // INIT
    void Awake () {
        Screen.fullScreen = false;
        Screen.SetResolution(375,667,false);

        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        // Assign static instance
        if (instance == null) {
            instance = this;
        } else if (instance != this) {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

	void Start () {
        shaking = false;
        locked = false;
	}

    // API
    public void ResetPosition() {
        transform.position = DefaultPosition();
    }

    public void ScreenShake() {
        shaking = true;
        InvokeRepeating("CameraShake", 0, .01f);
        Invoke("StopShaking", 0.3f);
    }

    public void LockCamera(Vector2 position, PostLockDelegate postLock = null ) {
        locked = true;
        StartCoroutine(TrackToPosition(position, false, postLock));
    }

    public void UnlockCamera(PostLockDelegate postLock = null) {
        StartCoroutine(TrackToPosition(DefaultPosition(), true, postLock));
    }

    IEnumerator TrackToPosition(Vector2 position, bool unlock = false, PostLockDelegate postLock = null) {
        float elapsedTime = 0.0f;
        Vector3 startPos = transform.position;
        Vector3 endPos = new Vector3(position.x, position.y, startPos.z);
        while (elapsedTime < trackingDuration) {
            yield return null;
            elapsedTime += Time.deltaTime;
            transform.position = Vector3.Lerp(startPos, endPos, elapsedTime/trackingDuration);
        }
        if (unlock) locked = false;
        if (postLock != null) {
            postLock();
        }
    }


    Vector3 DefaultPosition() {
        return transform.position.SwapY(player.transform.position.y + 3.5f);
    }

    // PRIVATE
	void Update () {
        if (!shaking) {
            TrackPlayer();
        }
	}

    void TrackPlayer() {
        if (locked) return;

		float targetX = transform.position.x;
		float targetY = transform.position.y + Time.deltaTime * player.zoneVelocity.y;

		transform.position = new Vector3(targetX, targetY, transform.position.z);
    }

    void CameraShake() {
        float quakeAmt = Random.value*0.01f*2 - 0.01f;
        Vector3 pp = transform.position;
        pp.y+= quakeAmt; // can also add to x and/or z
        transform.position = pp;
    }

    void StopShaking() {
        shaking = false;
        CancelInvoke("CameraShake");
    }
}
