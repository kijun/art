using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using PureShape;

public class Agent : MonoBehaviour {

    public Dictionary<Location, Agent> adjacent = new Dictionary<Location, Agent>();
    public Agent next;
    public Agent prev;
    public int row;
    public int col;

    public AgentState state;

    public void ChangeColor(Color color, Note note, float virality) {
        // float delay =
        // run change,
        //
        animatable.color = color;
        Debug.Log("changing colooor");
    }

    public void ChangeSize(Vector2 size, Note note, float virality) {
    }

    public Animatable2 animatable {
        get {
            return GetComponent<Animatable2>();
        }
    }
/*
 * behaviors?
 *
 * change color
 * disappear
 * enlarge (certain direction)
 * movement (permanent, temporary)
 * additional (max bill)
 * rotate
 * graphical (font, ikko tanaka)
 * turn to circle, or a particular shape
 *
 */
    public static Agent[,] CreateBoard(int cols, int rows, float length) {
        Debug.Log("Creating board " + cols + " " + rows);
        var board = new Agent[cols, rows]; // x, y
        var diagonal = length * 1.414f;
        var xmin = -(diagonal*(cols-1))/2;
        var ymin = -(diagonal*(rows-1))/2;
        for (int i = 0; i < cols; i++) {
            for (int j = 0; j < rows; j++) {
                var rp = new RectParams {
                    x = xmin + i * diagonal,
                    y = ymin + j * diagonal,
                    width = length,
                    height = length,
                    color = new Color32(1, 104, 200, 255)
                };

                var anim = NoteFactory.CreateRect(rp);
                var agent = anim.gameObject.AddComponent<Agent>();
                board[i, j] = agent;
            }
        }

        for (int x = 0; x < cols; x++) {
            for (int y = 0; y < rows; y++) {
                var agent = (Agent)board.GetValue2(x, y);
                agent.adjacent.AddIfNotNull(Location.TopLeft,     (Agent)board.GetValue2(x-1, y-1));
                agent.adjacent.AddIfNotNull(Location.Top,         (Agent)board.GetValue2(x,   y-1));
                agent.adjacent.AddIfNotNull(Location.TopRight,    (Agent)board.GetValue2(x+1, y-1));
                agent.adjacent.AddIfNotNull(Location.Left,        (Agent)board.GetValue2(x-1, y));
                agent.adjacent.AddIfNotNull(Location.Right,       (Agent)board.GetValue2(x+1, y));
                agent.adjacent.AddIfNotNull(Location.BottomLeft,  (Agent)board.GetValue2(x-1, y+1));
                agent.adjacent.AddIfNotNull(Location.Bottom,      (Agent)board.GetValue2(x,   y+1));
                agent.adjacent.AddIfNotNull(Location.BottomRight, (Agent)board.GetValue2(x+1, y+1));
            }
        }

        return board;
    }
}

