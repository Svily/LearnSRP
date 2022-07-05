#ifndef CUSTOM_LIGHTING_INCLUDED
#define CUSTOM_LIGHTING_INCLUDED

#define MIN_REFLECTIVITY 0.04

float OneMinusReflectivity (float metallic) {
    float range = 1.0 - MIN_REFLECTIVITY;
    return range - metallic * range;
}

BRDF GetBRDF(Surface surface, bool applyAlphaToDiffuse = false)
{
    float oneMinusReflectivity = OneMinusReflectivity(surface.metallic);
    
    BRDF brdf;
    brdf.diffuse = surface.color * oneMinusReflectivity;
    if (applyAlphaToDiffuse)
    {
        brdf.diffuse *= surface.alpha;
    }
    brdf.specular =  lerp(MIN_REFLECTIVITY, surface.color, surface.metallic);
    float perceptualRoughness = PerceptualSmoothnessToPerceptualRoughness(surface.smoothness);
    brdf.roughness = PerceptualRoughnessToRoughness(perceptualRoughness);
    return brdf;
}

float3 IncomingLight (Surface surface, Light light) {
    return saturate(dot(surface.normal, light.direction)) * light.color;
}

float3 GetLighting (Surface surface, BRDF brdf,Light light) {
    return IncomingLight(surface, light) * DirectBRDF(surface, brdf, light);
}

float3 GetLighting (Surface surface, BRDF brdf) {
    float3 color = 0.0;
    for (int i = 0; i < GetDirectionalLightCount(); i++) {
        color += GetLighting(surface, brdf, GetDirectionalLight(i));
    }
    return color;
}







#endif


