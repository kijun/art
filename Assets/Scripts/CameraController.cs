using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

    public static CameraController instance;

    private PlayerController player;
    private bool shaking;

	// Use this for initialization
	void Start () {
        shaking = false;
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

    /*
	bool CheckXMargin() {
		// Returns true if the distance between the camera and the player in the x axis is greater than the x margin.
		//return Mathf.Abs(transform.position.x - player.position.x) > xMargin;
	}


	bool CheckYMargin() {
		// Returns true if the distance between the camera and the player in the y axis is greater than the y margin.
		//return Mathf.Abs(transform.position.y - player.position.y) > yMargin;
	}
    */

	// Update is called once per frame
	void Update () {
        if (!shaking) {
            TrackPlayer();
        }
	}

    public void ResetPosition() {
        transform.position = transform.position.SwapY(player.transform.position.y + 3.5f);
    }

    void TrackPlayer() {
		// By default the target x and y coordinates of the camera are it's current x and y coordinates.
		float targetX = transform.position.x;
		float targetY = transform.position.y + Time.deltaTime * player.yBaseSpeed;

		// If the player has moved beyond the x margin...
		//if(CheckXMargin())
			// ... the target x coordinate should be a Lerp between the camera's current x position and the player's current x position.
        //    targetX = player.position.x;
			//targetX = Mathf.Lerp(transform.position.x, player.position.x, xSmooth * Time.deltaTime);

		// If the player has moved beyond the y margin...
		//if(CheckYMargin())
			// ... the target y coordinate should be a Lerp between the camera's current y position and the player's current y position.
//            targetY = player.position.y - yMargin;
			//targetY = Mathf.Lerp(transform.position.y, player.position.y, ySmooth * Time.deltaTime);


		// Set the camera's position to the target position with the same z component.
		transform.position = new Vector3(targetX, targetY, transform.position.z);
    }

    public void ScreenShake() {
        shaking = true;
        InvokeRepeating("CameraShake", 0, .01f);
        Invoke("StopShaking", 0.3f);
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
