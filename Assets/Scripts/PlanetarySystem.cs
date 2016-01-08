using UnityEngine;
using System.Collections.Generic;

public class PlanetarySystem {

    private Dictionary<int, Dictionary<int, Sector>> sectors;
    private int sectorSize;

    public PlanetarySystem (int sectorSize) {
        sectors = new Dictionary<int, Dictionary<int, Sector>>();
        this.sectorSize = sectorSize;
    }

    public Sector GetSector(int col, int row) {
        if (!sectors.ContainsKey(col)) {
            sectors[col] = new Dictionary<int, Sector>();
        }
        if (!sectors[col].ContainsKey(row)) {
            sectors[col][row] = new Sector(col, row, sectorSize);
        }
        return sectors[col][row];
    }
}

public class Sector {

    public int col, row, size;

    public List<Planet> planets = new List<Planet>();

    public Sector(int col, int row, int sectorSize) {
        this.col = col; this.row = row; this.size = sectorSize;

        for (int i=0; i<Consts.sectorDensity; i++) {
            Planet newPlanet = null;
            while (newPlanet == null) {
                // create planet params
                float x = col*Consts.sectorSize; // midpoint
                float y = row*Consts.sectorSize; // midpoint
                x += Consts.sectorSize * (Random.value - 0.5f);
                y += Consts.sectorSize * (Random.value - 0.5f);
                var radius = RandomHelper.Between(Consts.planetMinRadius, Consts.planetMaxRadius);

                newPlanet = new Planet(x, y, radius);
                foreach (var p in planets) {
                    if (p.Overlaps(newPlanet)) {
                        newPlanet = null;
                        break;
                    }
                }
            }
            planets.Add(newPlanet);
        }
    }

    public Vector2 Origin {
        get {
            return new Vector2(col * size, row * size);
        }
    }

    public int Size {
        get {
            return size;
        }
    }

    public Vector2 RandomPoint() {
        var o = Origin;
        var radius = size/2f;
        return new Vector2(Random.Range(o.x - radius, o.x + radius),
                           Random.Range(o.y - radius, o.y + radius));
    }
}

public class Planet {

    public Vector2 origin;
    public float radius;

    public Planet (float x, float y, float radius) {
        origin = new Vector2(x, y);
        this.radius = radius;
    }

    public bool Overlaps(Planet otherPlanet) {
        if (Vector2.Distance(origin, otherPlanet.origin) < radius+otherPlanet.radius) {
            return true;
        }
        return false;
    }
}

public class Artifact : MonoBehaviour{
    public string poem;

    void OnCollisionEnter2D(Collision2D coll) {
//        if (coll.gameObject.tag == "Player")
    }
}

public class Inventory {
    public List<Artifact> artifacts = new List<Artifact>();
}
