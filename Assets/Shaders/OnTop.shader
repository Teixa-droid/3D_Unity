Shader "Custom/On top" {
	Properties{
		_Color("Main Color", Color) = (1, 1, 1, 1)
		_MainTex("Base (RGB)", 2D) = "white"
	}
		SubShader{
			Tags { "Queue" = "Overlay+1" "RenderType" = "Transparent"}
			ZTest Always
			Pass {
				Blend SrcAlpha OneMinusSrcAlpha
				SetTexture[_MainTex] {
					constantColor[_Color]
					Combine texture * constant
				}
			}
	}
}