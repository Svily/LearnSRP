using UnityEngine;

[DisallowMultipleComponent]
public class PerObjectMaterialProperties  : MonoBehaviour
{ 
    
    static int baseColorId = Shader.PropertyToID("_BaseColor");
    static int cutoffId = Shader.PropertyToID("_Cutoff");
    static int metallicId = Shader.PropertyToID("_Metallic"),
               smoothnessId = Shader.PropertyToID("_Smoothness");

    
    static MaterialPropertyBlock block;
   
   [SerializeField]
   Color baseColor = Color.white;
   
   
   [SerializeField, Range(0f, 1f)]
   float cutoff = 0.5f, metallic = 0f, smoonthness = 0.5f;

   private void Awake()
   {
       this.OnValidate();
   }

   private void OnValidate()
   {
       block ??= new MaterialPropertyBlock();
       block.SetColor(baseColorId, baseColor);
       block.SetFloat(cutoffId, cutoff);
       block.SetFloat(metallicId, metallic);
       block.SetFloat(smoothnessId, smoonthness);
       GetComponent<Renderer>().SetPropertyBlock(block);
   }
}
