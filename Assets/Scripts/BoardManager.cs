using UnityEngine;
using System.Collections.Generic;


public class BoardManager : MonoBehaviour {

    public GameObject[] planetPrefabs;
    public GameObject[] artifactPrefabs;

    public Count planetsPerSector = new Count(2, 5);
    public Count artifactsPerSector = new Count(1,2);
    public int sectorSize = 30;

    private PlanetarySystem sys;
    private Transform player;
    private HashSet<Sector>renderedSectors;
    private Sector prevSector;
    private GameManager gm;

    // PUBLIC METHODS
    public void SetupScene() {
        // init members
        sys = new PlanetarySystem(sectorSize);
        player = GameObject.FindGameObjectWithTag("Player").transform;
        //player.position = new Vector2(0f, 0f);
        renderedSectors = new HashSet<Sector>();
        prevSector = CurrentSector();

        // create planets and artifacts
        RenderAdjacentSectors(prevSector);
    }

    // MONOBEHAVIOUR
    void FixedUpdate() {
        if (!GameManager.instance.journeying) return;

        var currSector = CurrentSector();
        if (currSector != prevSector) {
            RenderAdjacentSectors(currSector);
            prevSector = currSector;
        }
    }

    // SECTOR HELPERS
    Sector CurrentSector() {
        var ssize = sectorSize;
        Vector2 pos = player.position;
        int x = Mathf.FloorToInt((pos.x - ssize/2.0f)/ssize);
        int y = Mathf.FloorToInt((pos.y - ssize/2.0f)/ssize);
        //Debug.Log("For pos: " + pos + "Current Sector (" + x + ", " + y + ")");
        return sys.GetSector(x, y);
    }

    HashSet<Sector> AdjacentSectors(Sector s) {
        var sectors = new HashSet<Sector>();
        //var cs = CurrentSector();
        for (int i = -1; i<=1; i++) {
            for (int j = -1; j<=1; j++) {
                sectors.Add(sys.GetSector(s.col+i, s.row+j));
            }
        }
        return sectors;
    }

    void RenderAdjacentSectors(Sector center) {
        foreach (var s in AdjacentSectors(center)) {
            RenderSector(s);
        }
    }

    void RenderSector(Sector sector) {
        Debug.Log("Rendering Sector " + sector);
        if (!renderedSectors.Contains(sector)) {
            // TODO object pool
            // create planets
            var numPlanets = Random.Range(planetsPerSector.minimum, planetsPerSector.maximum+1);
            var numArtifacts = Random.Range(artifactsPerSector.minimum, artifactsPerSector.maximum+1);
            for (int i = 0; i < numPlanets; i++) {
                InstantiateWithoutOverlap(planetPrefabs[Random.Range(0, planetPrefabs.Length)], sector);
            }
            for (int i = 0; i < numArtifacts; i++) {
                InstantiateWithoutOverlap(artifactPrefabs[Random.Range(0, artifactPrefabs.Length)], sector);
            }

            renderedSectors.Add(sector);
        }

    }

    void InstantiateWithoutOverlap(GameObject prefab, Sector sector) {
        Vector2 size = prefab.GetComponent<Collider2D>().bounds.size;
        Vector2 origin, botLeft, topRight;
        //= sector.RandomPoint();
        //Vector2 botLeft = new Vector2(origin.x - size.x/2f, origin.y - size.y/2f);
        //Vector2 topRight = new Vector2(origin.x + size.x/2f, origin.y + size.y/2f);

        do {
            origin = sector.RandomPoint();
            botLeft = new Vector2(origin.x - size.x/2f, origin.y - size.y/2f);
            topRight = new Vector2(origin.x + size.x/2f, origin.y + size.y/2f);
        } while (Physics2D.OverlapArea(botLeft, topRight));

        var go = Instantiate(prefab);
        go.transform.position = origin;
    }
}

