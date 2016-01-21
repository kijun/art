Shader "Sprite" {
    Properties {
        _MainTex ("Base (RGB) Trans (A)", 2D) = "white" {}
    }

    SubShader {
        Tags {"Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent"}

        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha
        Lighting Off

        Pass {
            SetTexture [_MainTex] { combine texture }
        }
    }
}
