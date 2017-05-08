using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public enum Direction {
    Up, Right, Down, Left
}

public class CellD : BaseCell {
    /* multiple [] bricks*/
    public Vector2[] scales;
    public Color[] colors;
    public float speed = 1;
    public int objsPerRun = 3;

    public void Run() {
        StartCoroutine(_Run());
    }

    public IEnumerator _Run() {
        var randArray = new int[] {0, 1, 2, 3};
        List<int> randomized = randArray.OrderBy(u => Random.value).ToList();

        int randomIndex = Random.Range(0, 3);
        int randomDirection = Random.value > 0.5 ? -1 : 1;
        for (int i = 0; i < objsPerRun; i++) {
            var dir = (Direction)randomized[i];
            var p = CreatePlane(Quaternion.identity);
            var scale = scales[i];
            p.localScale = scale;
            p.level = level;
            var x = (inGameWidth + scale.x) / 2f;
            var y = (inGameHeight + scale.y) / 2f;
            switch (dir) {
                case Direction.Up:
                    x *= Random.Range(-0.7f, 0.7f);
                    y *= -1;
                    p.velocity = Vector2.up * speed;
                    break;
                case Direction.Right:
                    x *= -1;
                    y *= Random.Range(-0.7f, 0.7f);
                    p.velocity = Vector2.right * speed;
                    break;
                case Direction.Down:
                    x *= Random.Range(-0.7f, 0.7f);
                    p.velocity = Vector2.down * speed;
                    break;
                case Direction.Left:
                    p.velocity = Vector2.left * speed;
                    y *= Random.Range(-0.7f, 0.7f);
                    break;
            }
            p.color = colors[(randomIndex + i)%colors.Length];
            p.position = new Vector2(x, y);
        }
        yield return null;
    }

    Animatable2 CreatePlane(Quaternion entryAngle) {
        return Instantiate<Animatable2>(prefab, Vector2.one, entryAngle);
    }

    T GetRandomEnum<T>()
    {
        System.Array A = System.Enum.GetValues(typeof(T));
        T V = (T)A.GetValue(UnityEngine.Random.Range(0,A.Length));
        return V;
    }

}

