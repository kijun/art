using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhraseB : MonoBehaviour {

    public CellB cellB;
    public float period = 0.5f;

    void Start() {
        StartCoroutine(Run());
    }

    IEnumerator Run() {
        for (int i = 0; i < 200; i++) {
            var baseCnt = 1+(i/10f);
            var cnt = 1+(i/10f);
            //var cnt = Random.Range(baseCnt, baseCnt*2);
            cellB.count = (int)cnt;
            cellB.Run();
            cellB.entryAngle += 90;
            cellB.Run();
            cellB.entryAngle += 90;
            cellB.Run();
            cellB.entryAngle += 90;
            cellB.Run();
            cellB.entryAngle += 90;
            //cellB.entryAngle = Random.Range(0, 4) * 90;
            //cellB.scale = Vector2.one * (5f / cnt);
            yield return new WaitForSeconds(period);
        }

        for (int i = 0; i < 200; i++) {
            cellB.Run();
            var baseCnt = 5 - (i/10f);
            var cnt = Random.Range(baseCnt, baseCnt*2);
            cellB.count = (int)cnt;
            cellB.entryAngle += 90;
            //cellB.scale = Vector2.one * (5f / cnt);
            yield return new WaitForSeconds(period);
        }
        /* increasingly.
        for (int i = 0; i < 200; i++) {
            var baseCnt = 1+(i/10f);
            var cnt = 1+(i/10f);
            //var cnt = Random.Range(baseCnt, baseCnt*2);
            cellB.count = (int)cnt;
            cellB.Run();
            cellB.entryAngle += 90;
            //cellB.entryAngle = Random.Range(0, 4) * 90;
            //cellB.scale = Vector2.one * (5f / cnt);
            yield return new WaitForSeconds(period);
        }
        */
    }
}



