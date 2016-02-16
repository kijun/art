using UnityEngine;
using System.Collections;

[RequireComponent (typeof (BoxCollider2D))]

public class DungeonController : MonoBehaviour {

    public GameObject wallPrefab;
    public Transform cameraLockPosition;
    public float wallWidth = 0.3f;
    public float wallMovementDuration = 5f;
    public float exitWidth = 0.5f;

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
        Debug.Log("Locking");
        if (!locked) {
            Camera cam = Camera.main;
            cameraCtrl = cam.GetComponent<CameraController>();
            // TODO add post locking delegate and move walls there
            cameraCtrl.LockCamera(cameraLockPosition.position,
                                  delegate() { CreateEnclosure(); });
            locked = true;
        }
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

    void MoveWall(GameObject wall, Vector3 initPosLocal, Vector3 endPosLocal) {
        var center = cameraLockPosition.position;
        StartCoroutine(wall.transform.Glide(initPosLocal+center, endPosLocal+center, wallMovementDuration));
    }

    GameObject CreateWall(float width, float height) {
        var w = Instantiate<GameObject>(wallPrefab);
        w.transform.localScale = new Vector2(width, height);
        return w;
    }

    IEnumerator MoveWall(GameObject wall, Vector2 initPos, Vector2 endPos) {
        yield return StartCoroutine(wall.transform.Glide(initPos, endPos, wallMovementDuration));
    }
}
