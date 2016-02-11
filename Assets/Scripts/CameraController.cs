using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

    public static CameraController instance;
    public float trackingDuration = 1.3f;

    private PlayerController player;
    private bool shaking;
    private bool locked;

    // INIT
	// Use this for initialization
	void Start () {
        shaking = false;
        locked = false;
	}

    void Awake () {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        // Assign static instance
        if (instance == null) {
            instance = this;
        } else if (instance != this) {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
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

    public void LockCamera(Vector2 position) {
        locked = true;
        StartCoroutine(TrackToPosition(position));
    }

    public void UnlockCamera() {
        StartCoroutine(TrackToPosition(DefaultPosition(), true));
    }

    IEnumerator TrackToPosition(Vector2 position, bool unlock = false) {
        float elapsedTime = 0.0f;
        Vector3 startPos = transform.position;
        Vector3 endPos = new Vector3(position.x, position.y, startPos.z);
        while (elapsedTime < trackingDuration) {
            yield return null;
            elapsedTime += Time.deltaTime;
            transform.position = Vector3.Lerp(startPos, endPos, elapsedTime/trackingDuration);
        }
        if (unlock) locked = false;
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
		float targetY = transform.position.y + Time.deltaTime * player.yBaseSpeed;

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
