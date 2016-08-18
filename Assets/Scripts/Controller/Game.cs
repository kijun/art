using UnityEngine;


/*
 * Singleton class that manages the chain of events of a game.
 *
 * Classes like Player and Segment calls out to this class,
 * which propagates the event down the proper chain.
 */
public class Game : MonoBehaviour {


    /***** PRIVATE STATIC VARIABLES *****/
    static Game instance;


    /***** PUBLIC VARIABLES *****/
    public Player playerPrefab;
    public CameraController camera;
    // TODO refactor
    public Level level;

    /* TEMP OBJECTS */
    public Level currentLevel;


    /***** PRIVATE VARIABLES *****/
    //Level currentLevel;
    Player player;


    /***** STATIC PROPERTIES *****/
    public static Game Instance {
        get {
            return instance;
        }
    }

    /***** INITIALIZER *****/
    void Awake () {
        // Assign static instance
        if (instance == null) {
            instance = this;
        } else if (instance != this) {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);


        // Set screen to set resolution
        Screen.fullScreen = false;
        Screen.SetResolution(375,667,false);

        /* Max resolution
        var resolutions = Screen.resolutions;
        var maxResolution = resolutions[resolutions.Length-1];
        Screen.SetResolution(maxResolution.width, maxResolution.height, true);
        */

        player = Instantiate(playerPrefab);
    }

    void Start() {
        level.PlayLevel(player, camera);
    }


    /***** PUBLIC METHODS *****/
    public void SegmentCompleted(Segment segment) {
        // current level, play next segment
        //currentLevel.PlayNextSegment();
    }

    public void PlayerDied() {
        level.ReplaySegment(player, camera);
    }
}


