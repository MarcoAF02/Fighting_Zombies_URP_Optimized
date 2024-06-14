PBR Double Sided (PBR Double Sided Cutout) - regular surface shader, that uses Albedo, Normal, SpecularSmoothness and AmbientOcclusion maps and does not cull backfaces

Fresnel - used for fire part on torch. This shader animate uv of noise that sampled with model uv or triplanar uv and masked by FresnelMask texture.
NoiseSoftness controls softness of noise
FresnelScale and FresnelPower - is scale and power of fresnel
Color 0 and Color 1 - blending color by noise

Fire - used for fire particles. This shader animate uv of noise that sampled with model uv.
Used custom data from UV.zw.
UV.z - used for rotation of noise uv
UV.w - used for noise softness multiplier
NoiseSoftness - softness of noise
NoiseSpeed - speed of noise uv animation
DepthFade - SoftParticles parameter, that fades particle when it is too close to surface
Rotation - rotation amount multiplier (Rotation * UV.z)
Offset - uv offset before rotation
AlphaSoftness - softness of alpha