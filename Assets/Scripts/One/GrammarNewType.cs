using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using PureShape;

namespace One {

public class GrammarNewType : MonoBehaviour {

    public void Generate() {
        var data = new Dictionary<string, string> {
            { "test", "val" },
            { "test2", "val2" }
        };

        data.Add("adsf", "ASdf");
    }
}

}


