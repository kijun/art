using UnityEngine;
using SVGImporter;
using System.Collections;
using System.Collections.Generic;

public class BackgroundController : MonoBehaviour {
    public int maxBackgroundObjects = 3;
    public SVGRenderer globePrefab; // assume size 1x1x1

    public Color[] smallPlanetColorOptions;
    public Color[] largePlanetColorOptions;
    public float[] smallPlanetDiameterOptions;
    public float[] largePlanetDiameterOptions;
    public Range smallPlanetSpeedRange = new Range(2f, 5f);
    public Range largePlanetSpeedRange = new Range(0.5f, 1f);

    public BoxCollider2D visibleArea;
    public PlayerController playerCtrl;

    private List<GameObject> backgroundObjects = new List<GameObject>();

	// Use this for initialization
	void Start () {
        playerCtrl = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
	}

	// Update is called once per frame
	void Update () {
        if (backgroundObjects.Count < maxBackgroundObjects) {
            SpawnObject();
        }
	}

    void SpawnObject() {
        // create object and set basic property
        var newObject = Instantiate<SVGRenderer>(globePrefab);
        float diameter;
        Color color;
        float speed;

        if (numberOfLargeObjects() > 0) {
            // spawn small object
            diameter = smallPlanetDiameterOptions[Random.Range(0, smallPlanetDiameterOptions.Length)];
            color = smallPlanetColorOptions[Random.Range(0, smallPlanetColorOptions.Length)];
            speed = smallPlanetSpeedRange.RandomValue();
        } else {
            // spawn large object
            diameter = largePlanetDiameterOptions[Random.Range(0, largePlanetDiameterOptions.Length)];
            color = largePlanetColorOptions[Random.Range(0, largePlanetColorOptions.Length)];
            speed = largePlanetSpeedRange.RandomValue();
        }
        newObject.transform.localScale = Vector3.one * diameter;
        //newObject.color = color;
        Debug.Log(color +"/" + newObject.color);
        Debug.Log(color.GetType() + "/" + newObject.color.GetType());
        newObject.color = color;

        // choose spawn point
        Bounds visibleBounds = visibleArea.bounds;
        Vector3 visibleSize = visibleArea.bounds.size;
        float spawnRange = visibleSize.x*2+visibleSize.y*2+diameter;
        float spawnSeed = Random.value * spawnRange;
        float radius = diameter/2f;

        Vector2 velocity;
        float minx = visibleBounds.min.x-radius;
        float maxx = visibleBounds.max.x+radius;
        float miny = visibleBounds.min.y-radius;
        float maxy = visibleBounds.max.y+radius;

        Vector2 spawnPoint;
        if (spawnSeed < visibleSize.x+diameter) {
            // TOP
            spawnPoint = new Vector2(Random.Range(minx,maxx), maxy);
        } else if (spawnSeed < visibleSize.x+visibleSize.y+diameter*2) {
            // RIGHT
            spawnPoint = new Vector2(maxx, Random.Range(miny,maxy));
        } else if (spawnSeed < 2*visibleSize.x+visibleSize.y+diameter*3) {
            spawnPoint = new Vector2(Random.Range(minx,maxx), miny);
            // LEFT
        } else {
            // BOT
            spawnPoint = new Vector2(minx, Random.Range(miny,maxy));
        }
        // slight chance that object won't be visible;
        var exitPoint = new Vector2(Random.Range(minx, maxx), Random.Range(miny, maxy));
        newObject.transform.position = spawnPoint;

        var rgbd = newObject.GetComponent<Rigidbody2D>();
        var baseVelocity = (exitPoint-spawnPoint).normalized * speed;
        //rgbd.velocity = baseVelocity + playerCtrl.yBaseSpeed * Vector2.up;

        // add it to list
        newObject.GetComponent<BackgroundObject>().onBecameInvisibleDelegate = delegate (GameObject go) {
            // TODO object pool
            backgroundObjects.Remove(go);
            Destroy(go);
        };
        backgroundObjects.Add(newObject.gameObject);
        Debug.Log("new object created");
    }

    int numberOfLargeObjects() {
        // determine size of the next object to spawn
        int numLargeObjects = 0;
        float minLargeSize = Mathf.Min(largePlanetDiameterOptions);

        foreach (var go in backgroundObjects) {
            if (go.transform.localScale.x > minLargeSize) {
                numLargeObjects++;
            }
        }
        return numLargeObjects;
    }
}
