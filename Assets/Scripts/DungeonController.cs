using UnityEngine;
using System.Collections;

[RequireComponent (typeof (BoxCollider2D))]

public class DungeonController : MonoBehaviour {

    public GameObject wallPrefab;
    public Frame framePrefab;
    public Transform cameraLockPosition;
    public float wallWidth = 0.3f;
    public float wallMovementDuration = 5f;
    public float exitWidth = 0.5f;
    public Collider2D frameSpawnLocation;

    private CameraController cameraCtrl;
    private PlayerController player;
    private bool locked;

	// Use this for initialization
	void Start () {
	}

    void OnTriggerEnter2D(Collider2D other) {
        Debug.Log(other);
        if (other.gameObject.tag == "Player") {
            player = other.gameObject.GetComponent<PlayerController>();
            player.LockCurrentRegion();
            Lock();
        }
    }

    void Lock() {
        if (!locked) {
            Camera cam = Camera.main;
            cameraCtrl = cam.GetComponent<CameraController>();
            // TODO add post locking delegate and move walls there
            cameraCtrl.LockCamera(
                    cameraLockPosition.position,
                    delegate {
                        CreateEnclosure();
                        StartCoroutine(CoroutineWithWait(SpawnFrame(), wallMovementDuration));
                    });
            locked = true;
        }
    }

    IEnumerator CoroutineWithWait(IEnumerator coroutine, float waitTime) {
        yield return new WaitForSeconds(waitTime);
        yield return StartCoroutine(coroutine);
    }

    void CreateEnclosure() {
        // left
        var wall = CreateWall(wallWidth, CameraHelper.Height);
        var initPos = new Vector3(-CameraHelper.HalfWidth-wallWidth/2, 0, 0);
        var endPos = initPos.IncrX(wallWidth);
        MoveWall(wall, initPos, endPos);

        // right
        wall = CreateWall(wallWidth, CameraHelper.Height);
        initPos = new Vector3(CameraHelper.HalfWidth+wallWidth/2, 0, 0);
        endPos = initPos.IncrX(-wallWidth);
        MoveWall(wall, initPos, endPos);

        // bot
        wall = CreateWall(CameraHelper.Width, wallWidth);
        initPos = new Vector3(0, -CameraHelper.HalfHeight-wallWidth/2, 0);
        endPos = initPos.IncrY(wallWidth);
        MoveWall(wall, initPos, endPos);

        // top1
        wall = CreateWall(CameraHelper.HalfWidth-exitWidth/2f, wallWidth);
        initPos = new Vector3(-1*CameraHelper.HalfWidth/2, CameraHelper.HalfHeight+wallWidth/2, 0);
        endPos = initPos.IncrY(-wallWidth);
        MoveWall(wall, initPos, endPos);

        // top2
        wall = CreateWall(CameraHelper.HalfWidth-exitWidth/2f, wallWidth);
        initPos = new Vector3(CameraHelper.HalfWidth/2f, CameraHelper.HalfHeight+wallWidth/2, 0);
        endPos = initPos.IncrY(-wallWidth);
        MoveWall(wall, initPos, endPos);
    }

    GameObject CreateWall(float width, float height) {
        var w = Instantiate<GameObject>(wallPrefab);
        w.transform.localScale = new Vector2(width, height);
        return w;
    }

    void MoveWall(GameObject wall, Vector3 initPosLocal, Vector3 endPosLocal) {
        var center = cameraLockPosition.position;
        StartCoroutine(wall.transform.Glide(initPosLocal+center, endPosLocal+center, wallMovementDuration));
    }

    IEnumerator SpawnFrame() {
        while (true) {
            var frame = Instantiate<Frame>(framePrefab);
            frame.transform.position = frameSpawnLocation.bounds.RandomPoint();
            var rg2d = frame.GetComponent<Rigidbody2D>();
            rg2d.angularVelocity = Random.Range(-1.60f, 1.60f);
            rg2d.velocity = new Vector2(Random.Range(-0.5f, 0.5f), Random.Range(-0.5f, -0.2f) );
            yield return new WaitForSeconds(Random.Range(5f, 7f));
        }
    }
}
