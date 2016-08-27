using UnityEngine;
using System.Collections;


[RequireComponent(typeof(Camera), typeof(Rigidbody2D))]
public class CameraController : MonoBehaviour {


    /***** PRIVATE STATIC VARIABLES *****/
    static CameraController instance;


    /***** INITIALIZER *****/
    void Awake () {
        if (instance == null) {
            instance = this;
        } else if (instance != this) {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

    void Start () {
    }


    /***** PUBLIC METHODS *****/

    public void LockCamera(Vector2 position) {
        // TODO
    }

    public void UnlockCamera() {
        // TODO
    }


    /***** MONOBEHAVIOUR *****/
    void Update() {
        Debug.Log(GetComponent<Rigidbody2D>().velocity);
    }


    /***** PUBLIC PROPERTIES *****/

    public Vector2 Position {
        get {
            return transform.position;
        }
        set {
            transform.position.SwapX(value.x).SwapY(value.y);
        }
    }

    public Vector2 Velocity {
        get {
            return GetComponent<Rigidbody2D>().velocity;
        }
        set {
            GetComponent<Rigidbody2D>().velocity = value;
        }
    }

    /*
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
    */


    /***** MONOBEHAVIOUR *****/


    /***** PRIVATE METHODS *****/
    /*
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
    */
}
