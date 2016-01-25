using UnityEngine;
using System.Collections;

public class Spawner : MonoBehaviour {

    public GameObject artifact;
    public GameObject planet;
    public GameObject astroid;
    public SnowFlake snowFlake;
    public float rateOfComet = 5f;

	// Use this for initialization
    private float halfScreenWidth;
    private bool madeOne;
    private BoxCollider2D spawnBox;


    void Awake() {
        spawnBox = GetComponent<BoxCollider2D>();
        halfScreenWidth = Camera.main.orthographicSize / Screen.height * Screen.width;
    }

    void Start() {
        StartCoroutine(SpawnComet());
        StartCoroutine(SpawnSnowFlake());
    }

	// Update is called once per frame
	void FixedUpdate () {
        if (!GameManager.instance.journeying) return;


        /*
        if (!madeOne) {
            var a = Instantiate(artifact);
            a.transform.position = new Vector2(
                    Random.Range(-1f*halfScreenWidth, halfScreenWidth), CameraHelper.Height);
            madeOne = true;
        }
        */
	}

    IEnumerator SpawnComet() {
        while (GameManager.instance.journeying) {
            if (Random.value < (1f/rateOfComet)*0.1f) {
                var a = Instantiate(astroid);
                a.transform.position = spawnBox.bounds.RandomPoint();
                    //new Vector2(
                        //Random.Range(-1*halfScreenWidth, halfScreenWidth), CameraHelper.Height);
                var rg = a.GetComponent<Rigidbody2D>();
                var direction = new Vector2(Random.Range(-2f, 2f), Random.Range(-6f, -8f));
                rg.velocity = direction;
            }

            yield return new WaitForSeconds(0.1f);
        }
    }

    IEnumerator SpawnSnowFlake() {
        while (GameManager.instance.journeying) {
            if (Random.value < (1f/rateOfComet)*0.1f) {
                var a = Instantiate<SnowFlake>(snowFlake);
                a.transform.position = RandomStartPoint();
                a.FallDown(Random.Range(-160, 160),
                        new Vector2(Random.Range(-2, 2), Random.Range(-2, -3)));
            }

            yield return new WaitForSeconds(0.1f);
        }

    }

    Vector2 RandomStartPoint() {
        return spawnBox.bounds.RandomPoint();
    }
}
