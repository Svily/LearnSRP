using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Rendering;

public partial class CameraRender
{
    private ScriptableRenderContext context;

    private Camera camera;

    private const string buffName = "Render Camera";

    private CommandBuffer buffer = new CommandBuffer() { name = buffName };

    private CullingResults cullingResults;

    private static ShaderTagId unlitShaderTagId = new ShaderTagId("SRPDefaultUnlit");



    public void Render(ScriptableRenderContext context, Camera camera)
    {
        this.context = context;
        this.camera = camera;
        this.PrepareBuffer();
        this.PrepareForSceneWindow();
        if (!this.Cull())
        {
            return;
        }
        this.SetUp();
        this.DrawVisibleGeometry();
        this.DrawUnsupportedShaders();
        this.DrawGizoms();
        this.Submit();
    }



    private void DrawVisibleGeometry()
    {
        var sortingSettings = new SortingSettings(this.camera){criteria = SortingCriteria.CommonOpaque};
        var drawingSettings = new DrawingSettings(unlitShaderTagId, sortingSettings);
        var filteringSettings = new FilteringSettings(RenderQueueRange.opaque);
        
        this.context.DrawRenderers(this.cullingResults, ref drawingSettings, ref filteringSettings);
        
        this.context.DrawSkybox(this.camera);
        
        sortingSettings.criteria = SortingCriteria.CommonTransparent;
        drawingSettings.sortingSettings = sortingSettings;
        filteringSettings.renderQueueRange = RenderQueueRange.transparent;

        context.DrawRenderers(
            cullingResults, ref drawingSettings, ref filteringSettings
        );
    }

    void SetUp()
    {
        this.context.SetupCameraProperties(this.camera);
        CameraClearFlags flags = this.camera.clearFlags;
        buffer.ClearRenderTarget(flags <= CameraClearFlags.Depth, flags == CameraClearFlags.Color, flags == CameraClearFlags.Color ? this.camera.backgroundColor.linear : Color.clear);
        buffer.BeginSample(SampleName);
        this.ExecuteBuffer();
        
    }

    bool Cull()
    {
        if (this.camera.TryGetCullingParameters(out ScriptableCullingParameters p))
        {
            this.cullingResults = context.Cull(ref p);
            return true;
        }
        return false;
    }

    void ExecuteBuffer()
    {
        this.context.ExecuteCommandBuffer(this.buffer);
        buffer.Clear();
    }

    void Submit()
    {
        buffer.EndSample(SampleName);
        this.ExecuteBuffer();
        this.context.Submit();
    }
}
