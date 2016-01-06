using UnityEngine;
using System.Collections.Generic;

public class PlanetarySystem {

    Dictionary<int, Dictionary<int, Sector>> sectors = new Dictionary<int, Dictionary<int, Sector>>();

    public Sector GetSector(int col, int row) {
        if (!sectors.ContainsKey(col)) {
            sectors[col] = new Dictionary<int, Sector>();
        }
        if (!sectors[col].ContainsKey(row)) {
            sectors[col][row] = new Sector(col, row);
        }
        return sectors[col][row];
    }
}

public class Sector {

    public int col, row;

    public List<Planet> planets = new List<Planet>();

    public Sector(int col, int row) {
        this.col = col; this.row = row;
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


