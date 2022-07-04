using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

public partial class CameraRender
{
    partial void DrawUnsupportedShaders();

    partial void DrawGizoms();

    partial void PrepareForSceneWindow();
    
#if UNITY_EDITOR
    
    private static Material errorMaterial;
    
    static ShaderTagId[] legacyShaderTagIds = {
        new ShaderTagId("Always"),
        new ShaderTagId("ForwardBase"),
        new ShaderTagId("PrepassBase"),
        new ShaderTagId("Vertex"),
        new ShaderTagId("VertexLMRGBM"),
        new ShaderTagId("VertexLM")
    };
    
    partial void DrawUnsupportedShaders()
    {
        if (errorMaterial == null)
        {
            errorMaterial = new Material(Shader.Find("Hidden/InternalErrorShader"));
        }
        
        
        var drawingSettings = new DrawingSettings(legacyShaderTagIds[0], new SortingSettings(this.camera))
        {
            overrideMaterial = errorMaterial
        };
        for (int i = 0; i < legacyShaderTagIds.Length; i++)
        {
            drawingSettings.SetShaderPassName(i, legacyShaderTagIds[i]);
        }
        var filteringSettings = FilteringSettings.defaultValue;
        this.context.DrawRenderers(this.cullingResults, ref drawingSettings, ref filteringSettings);
    }


    partial void DrawGizoms()
    {
        if (Handles.ShouldRenderGizmos())
        {
            this.context.DrawGizmos(this.camera, GizmoSubset.PreImageEffects);
            this.context.DrawGizmos(this.camera, GizmoSubset.PostImageEffects);
        }
    }

    partial void PrepareForSceneWindow()
    {
        if (this.camera.cameraType == CameraType.SceneView)
        {
            ScriptableRenderContext.EmitWorldGeometryForSceneView(this.camera);
        }
    }
    
#endif
}
