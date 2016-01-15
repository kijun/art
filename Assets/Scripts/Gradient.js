#pragma strict

 var startColor = Color.red;
 var endColor = Color.blue;

 function Start () {
     var mesh = GetComponent(MeshFilter).mesh;
     var colors = new Color[mesh.vertices.Length];
     colors[0] = startColor;
     colors[1] = endColor;
     colors[2] = startColor;
     colors[3] = endColor;
     mesh.colors = colors;
 }
