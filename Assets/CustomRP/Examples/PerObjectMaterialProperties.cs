using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class PerObjectMaterialProperties  : MonoBehaviour
{ 
    
    static int baseColorId = Shader.PropertyToID("_BaseColor");
    
    static MaterialPropertyBlock block;
   
   [SerializeField]
   Color baseColor = Color.white;

   private void OnValidate()
   {
       block ??= new MaterialPropertyBlock();
       block.SetColor(baseColorId, baseColor);
       GetComponent<Renderer>().SetPropertyBlock(block);
   }
}
