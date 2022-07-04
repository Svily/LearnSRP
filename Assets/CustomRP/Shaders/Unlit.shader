shader "Custom RP/Unlit"{

    Properties {
        _BaseColor("Color", color) = (0.25, 1, 0.85, 1)
    }
    
        
    SubShader{
        
        Pass{
            
            HLSLPROGRAM
            #pragma vertex UnlitPassVertex
            #pragma fragment UnlitPassFragment
            #include "UnlitPass.hlsl"
            ENDHLSL
            
        
        }
        
        
    }
    
    
    
}