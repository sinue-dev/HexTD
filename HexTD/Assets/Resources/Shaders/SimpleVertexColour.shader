Shader "SimpleVertexColour" {
	Properties {
	    _Color ("Main Color", Color) = (1,0.5,0.5,1)
	    _MainTex ("Texture", 2D) = ""
	}
	 
	SubShader {
		Cull Off
	    Lighting On
	    Material {Emission[_Color]}
	    ColorMaterial AmbientAndDiffuse
	   
	    Pass {
	        SetTexture[_MainTex] {Combine texture * primary}
	    }
	}
}