using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Debugger : MonoBehaviour {

    public Text text;

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {
        text.text = string.Format("{0:0.0}",Time.time);
	}
}
