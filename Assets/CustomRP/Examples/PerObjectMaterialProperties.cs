using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class PerObjectMaterialProperties  : MonoBehaviour
{ 
    
    static int baseColorId = Shader.PropertyToID("_BaseColor");
    static int cutoffId = Shader.PropertyToID("_Cutoff");

    
    static MaterialPropertyBlock block;
   
   [SerializeField]
   Color baseColor = Color.white;
   
   
   [SerializeField, Range(0f, 1f)]
   float cutoff = 0.5f;

   private void Awake()
   {
       this.OnValidate();
   }

   private void OnValidate()
   {
       block ??= new MaterialPropertyBlock();
       block.SetColor(baseColorId, baseColor);
       block.SetFloat(cutoffId, cutoff);
       GetComponent<Renderer>().SetPropertyBlock(block);
   }
}
