// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader "2Ginge/GritBuildupShader" {
    Properties {
        _Tint ("Tint", Color) = (1,1,1,1)
        _Albedo ("Albedo", 2D) = "white" {}
		_AlbedoTimeScale("Albedo Animation Scale", Range(-1,1)) = 0
        _Normal ("Normal", 2D) = "bump" {}
		_NormalTimeScale("Normal Animation Scale", Range(-1,1)) = 0
        _AO ("AO", 2D) = "white" {}
		_AOTimeScale("AO Animation Scale", Range(-1,1)) = 0
        _Height ("Height", 2D) = "gray" {}
		_HeightTimeScale("Height Animation Scale", Range(-1,1)) = 0
        _Gloss ("Gloss", Range(0, 1)) = 0
        _Metallic ("Metallic", Range(0, 1)) = 0
        _AOScale ("AO Scale", Range(0, 1)) = 0
        _UVs ("UVs", Vector) = (1,1,0,0)
        _LayerDirection ("Layer Direction", Vector) = (0,1,0,0)
        _Layer ("Layer", 2D) = "white" {}
        _LayerGloss ("Layer Gloss", Range(0, 1)) = 0
        _LayerMetallic ("Layer Metallic", Range(0, 1)) = 0
        _Displacement ("Displacement", Range(0.001, 1)) = 0.001
        [HDR]_LayerTint ("Layer Tint", Color) = (1,1,1,1)
        _TessellationValue ("Tessellation Value", Range(0.1, 10)) = 1
        _LayerAmountCreviceDisp ("Layer (Amount Crevice Disp)", Vector) = (0,0,0,0)
        _Power ("Power", Float ) = 0
        [MaterialToggle] _DisplaceBase ("Displace Base", Float ) = 0.001
        [MaterialToggle] _DisplaceLayer ("Displace Layer", Float ) = 0
        [MaterialToggle] _WorldonLocaloff ("World_on_Local_off", Float ) = 0
        _DistanceTesselation ("Distance Tessellation", Range(0, 100)) = 1
        _TessFade ("Tessellation Fade", Float ) = 10
    }
    SubShader {
        Tags {
            "RenderType"="Opaque"
        }
        LOD 200
        Pass {
            Name "FORWARD"
            Tags {
                "LightMode"="ForwardBase"
            }
            
            
            CGPROGRAM
            #pragma hull hull
            #pragma domain domain
            #pragma vertex tessvert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDBASE
            #define SHOULD_SAMPLE_SH ( defined (LIGHTMAP_OFF) && defined(DYNAMICLIGHTMAP_OFF) )
            #define _GLOSSYENV 1
            #include "UnityCG.cginc"
            #include "AutoLight.cginc"
            #include "Lighting.cginc"
            #include "Tessellation.cginc"
            #include "UnityPBSLighting.cginc"
            #include "UnityStandardBRDF.cginc"
            #pragma multi_compile_fwdbase_fullshadows
            #pragma multi_compile LIGHTMAP_OFF LIGHTMAP_ON
            #pragma multi_compile DIRLIGHTMAP_OFF DIRLIGHTMAP_COMBINED DIRLIGHTMAP_SEPARATE
            #pragma multi_compile DYNAMICLIGHTMAP_OFF DYNAMICLIGHTMAP_ON
            #pragma multi_compile_fog
            #pragma only_renderers d3d9 d3d11 glcore gles 
            #pragma target 5.0
			uniform float _AlbedoTimeScale, _NormalTimeScale, _AOTimeScale,_HeightTimeScale;
            uniform float _Metallic;
            uniform float _Gloss;
            uniform sampler2D _AO;
			uniform float4 _AO_ST;
            uniform sampler2D _Albedo;
            uniform float4 _Albedo_ST;
            uniform sampler2D _Normal;
            uniform float4 _Normal_ST;
			uniform float4 _Height_ST;
            uniform float4 _UVs;
            uniform float _AOScale;
            uniform float4 _LayerDirection;
            uniform sampler2D _Layer; uniform float4 _Layer_ST;
            uniform fixed _DisplaceBase;
            uniform sampler2D _Height;
			
            uniform float _Displacement;
            uniform float4 _LayerTint;
            uniform float _LayerMetallic;
            uniform float _LayerGloss;
            uniform fixed _DisplaceLayer;
            uniform float _TessellationValue;
            uniform float4 _LayerAmountCreviceDisp;
            uniform float _Power;
            uniform fixed _WorldonLocaloff;
            uniform float _DistanceTesselation;
            uniform float _TessFade;
            uniform float4 _Tint;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float4 tangent : TANGENT;
                float2 texcoord0 : TEXCOORD0;
                float2 texcoord1 : TEXCOORD1;
                float2 texcoord2 : TEXCOORD2;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float2 uv1 : TEXCOORD1;
                float2 uv2 : TEXCOORD2;
                float4 uvAN : TEXCOORD9;
                float4 uvSH : TEXCOORD8;
                float4 posWorld : TEXCOORD3;
                float3 normalDir : TEXCOORD4;
                float3 tangentDir : TEXCOORD5;
                float3 bitangentDir : TEXCOORD6;
                LIGHTING_COORDS(7,8)
                UNITY_FOG_COORDS(9)
                #if defined(LIGHTMAP_ON) || defined(UNITY_SHOULD_SAMPLE_SH)
                    float4 ambientOrLightmapUV : TEXCOORD10;
                #endif
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.uv1 = v.texcoord1;
                o.uv2 = v.texcoord2;
                o.uvAN = float4(TRANSFORM_TEX(v.texcoord0, _Albedo).xy,TRANSFORM_TEX(v.texcoord0, _Normal).xy);
				o.uvSH = float4(TRANSFORM_TEX(v.texcoord0, _AO).xy,TRANSFORM_TEX(v.texcoord0, _Height).xy);
                #ifdef LIGHTMAP_ON
                    o.ambientOrLightmapUV.xy = v.texcoord1.xy * unity_LightmapST.xy + unity_LightmapST.zw;
                    o.ambientOrLightmapUV.zw = 0;
                #endif
                #ifdef DYNAMICLIGHTMAP_ON
                    o.ambientOrLightmapUV.zw = v.texcoord2.xy * unity_DynamicLightmapST.xy + unity_DynamicLightmapST.zw;
                #endif
                o.normalDir = UnityObjectToWorldNormal(v.normal);
                o.tangentDir = normalize( mul( unity_ObjectToWorld, float4( v.tangent.xyz, 0.0 ) ).xyz );
                o.bitangentDir = normalize(cross(o.normalDir, o.tangentDir) * v.tangent.w);
                float2 node_9152 = (o.uv0*float2(_UVs.r,_UVs.g));
                float2 Uvs = node_9152;
                float2 node_712 = Uvs;
                float4 node_3154 = tex2Dlod(_Height,float4(node_712 + o.uvSH.zw + float2(_UVs.b * _Time.x,_UVs.a * _Time.x) * _HeightTimeScale,0.0,0));
                float3 node_9139 = (((length(node_3154.rgb)-0.5)*2.0)*lerp( 0.001, _Displacement, _DisplaceBase )*v.normal);
                v.vertex.xyz += node_9139;
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                float3 lightColor = _LightColor0.rgb;
                o.pos = UnityObjectToClipPos(v.vertex );
                UNITY_TRANSFER_FOG(o,o.pos);
                TRANSFER_VERTEX_TO_FRAGMENT(o)
                return o;
            }
            #ifdef UNITY_CAN_COMPILE_TESSELLATION
                struct TessVertex {
                    float4 vertex : INTERNALTESSPOS;
                    float3 normal : NORMAL;
                    float4 tangent : TANGENT;
                    float2 texcoord0 : TEXCOORD0;
                    float2 texcoord1 : TEXCOORD1;
                    float2 texcoord2 : TEXCOORD2;
                };
                struct OutputPatchConstant {
                    float edge[3]         : SV_TessFactor;
                    float inside          : SV_InsideTessFactor;
                    float3 vTangent[4]    : TANGENT;
                    float2 vUV[4]         : TEXCOORD;
                    float3 vTanUCorner[4] : TANUCORNER;
                    float3 vTanVCorner[4] : TANVCORNER;
                    float4 vCWts          : TANWEIGHTS;
                };
                TessVertex tessvert (VertexInput v) {
                    TessVertex o;
                    o.vertex = v.vertex;
                    o.normal = v.normal;
                    o.tangent = v.tangent;
                    o.texcoord0 = v.texcoord0;
                    o.texcoord1 = v.texcoord1;
                    o.texcoord2 = v.texcoord2;
                    return o;
                }
                void displacement (inout VertexInput v){
                    float2 node_9152 = (v.texcoord0*float2(_UVs.r,_UVs.g));
                    float4 node_4631 = tex2Dlod(_AO,float4(node_9152 + TRANSFORM_TEX(v.texcoord0, _AO).xy + float2(_UVs.b * _Time.x,_UVs.a * _Time.x) * _AOTimeScale,0.0,0));
                    float node_8836 = saturate(_LayerAmountCreviceDisp.r);
                    float MossRatio = saturate(pow((saturate((max(0,dot(normalize(lerp( mul( unity_ObjectToWorld, float4(_LayerDirection.rgb,0) ).xyz.rgb, _LayerDirection.rgb, _WorldonLocaloff )),v.normal))+lerp((length(node_4631.rgb)*(-1.0)),1.0,node_8836)))*length(saturate(((1.0 - node_4631.rgb)+saturate(_LayerAmountCreviceDisp.g))))*node_8836),_Power));
                    float _DisplaceLayer_var = lerp( 0.0, (clamp(_LayerAmountCreviceDisp.b,0.001,1.0)*sqrt(MossRatio)), _DisplaceLayer );
                    float3 MossDisplacement = (_DisplaceLayer_var*v.normal);
                    v.vertex.xyz += MossDisplacement;
                }
                float Tessellation(TessVertex v){
                    float2 node_9152 = (v.texcoord0*float2(_UVs.r,_UVs.g));
                    float2 Uvs = node_9152;
                    float2 node_712 = Uvs;
                    float4 node_3154 = tex2Dlod(_Height,float4(node_712 + TRANSFORM_TEX(v.texcoord0, _Height).xy + float2(_UVs.b * _Time.x,_UVs.a * _Time.x) * _HeightTimeScale,0.0,0));
                    float3 node_9139 = (((length(node_3154.rgb)-0.5)*2.0)*lerp( 0.001, _Displacement, _DisplaceBase )*v.normal);
                    float4 node_4631 = tex2Dlod(_AO,float4(node_9152 + TRANSFORM_TEX(v.texcoord0, _AO).xy + float2(_UVs.b * _Time.x,_UVs.a * _Time.x) * _AOTimeScale,0.0,0));
                    float node_8836 = saturate(_LayerAmountCreviceDisp.r);
                    float MossRatio = saturate(pow((saturate((max(0,dot(normalize(lerp( mul( unity_ObjectToWorld, float4(_LayerDirection.rgb,0) ).xyz.rgb, _LayerDirection.rgb, _WorldonLocaloff )),v.normal))+lerp((length(node_4631.rgb)*(-1.0)),1.0,node_8836)))*length(saturate(((1.0 - node_4631.rgb)+saturate(_LayerAmountCreviceDisp.g))))*node_8836),_Power));
                    float _DisplaceLayer_var = lerp( 0.0, (clamp(_LayerAmountCreviceDisp.b,0.001,1.0)*sqrt(MossRatio)), _DisplaceLayer );
                    float node_4962 = 1.0;
                    return clamp((_TessellationValue*(length(node_9139)+_DisplaceLayer_var)*100.0*(_DistanceTesselation*clamp((-1*(distance(mul(unity_ObjectToWorld, v.vertex).rgb,_WorldSpaceCameraPos)-_TessFade)),0.0,node_4962))),node_4962,10000.0);
                }
                float4 Tessellation(TessVertex v, TessVertex v1, TessVertex v2){
                    float tv = Tessellation(v);
                    float tv1 = Tessellation(v1);
                    float tv2 = Tessellation(v2);
                    return float4( tv1+tv2, tv2+tv, tv+tv1, tv+tv1+tv2 ) / float4(2,2,2,3);
                }
                OutputPatchConstant hullconst (InputPatch<TessVertex,3> v) {
                    OutputPatchConstant o = (OutputPatchConstant)0;
                    float4 ts = Tessellation( v[0], v[1], v[2] );
                    o.edge[0] = ts.x;
                    o.edge[1] = ts.y;
                    o.edge[2] = ts.z;
                    o.inside = ts.w;
                    return o;
                }
                [domain("tri")]
                [partitioning("fractional_odd")]
                [outputtopology("triangle_cw")]
                [patchconstantfunc("hullconst")]
                [outputcontrolpoints(3)]
                TessVertex hull (InputPatch<TessVertex,3> v, uint id : SV_OutputControlPointID) {
                    return v[id];
                }
                [domain("tri")]
                VertexOutput domain (OutputPatchConstant tessFactors, const OutputPatch<TessVertex,3> vi, float3 bary : SV_DomainLocation) {
                    VertexInput v = (VertexInput)0;
                    v.vertex = vi[0].vertex*bary.x + vi[1].vertex*bary.y + vi[2].vertex*bary.z;
                    v.normal = vi[0].normal*bary.x + vi[1].normal*bary.y + vi[2].normal*bary.z;
                    v.tangent = vi[0].tangent*bary.x + vi[1].tangent*bary.y + vi[2].tangent*bary.z;
                    v.texcoord0 = vi[0].texcoord0*bary.x + vi[1].texcoord0*bary.y + vi[2].texcoord0*bary.z;
                    v.texcoord1 = vi[0].texcoord1*bary.x + vi[1].texcoord1*bary.y + vi[2].texcoord1*bary.z;
                    displacement(v);
                    VertexOutput o = vert(v);
                    return o;
                }
            #endif
            float4 frag(VertexOutput i) : COLOR {
                i.normalDir = normalize(i.normalDir);
                float3x3 tangentTransform = float3x3( i.tangentDir, i.bitangentDir, i.normalDir);
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                float2 node_9152 = (i.uv0*float2(_UVs.r,_UVs.g));
                float2 Uvs = node_9152;
                float2 node_9237 = Uvs;
                float3 node_739 = UnpackNormal(tex2D(_Normal,node_9237 + i.uvAN.zw + float2(_UVs.b * _Time.x,_UVs.a * _Time.x) * _NormalTimeScale));
                float3 normalLocal = node_739.rgb;
                float3 normalDirection = normalize(mul( normalLocal, tangentTransform )); // Perturbed normals
                float3 viewReflectDirection = reflect( -viewDirection, normalDirection );
                float3 lightDirection = normalize(_WorldSpaceLightPos0.xyz);
                float3 lightColor = _LightColor0.rgb;
                float3 halfDirection = normalize(viewDirection+lightDirection);
////// Lighting:
                float attenuation = LIGHT_ATTENUATION(i);
                float3 attenColor = attenuation * _LightColor0.xyz;
                float Pi = 3.141592654;
                float InvPi = 0.31830988618;
///////// Gloss:
                float4 node_4631 = tex2D(_AO,node_9152 + i.uvSH.xy + float2(_UVs.b * _Time.x,_UVs.a * _Time.x) * _AOTimeScale);
                float node_8836 = saturate(_LayerAmountCreviceDisp.r);
                float MossRatio = saturate(pow((saturate((max(0,dot(normalize(lerp( mul( unity_ObjectToWorld, float4(_LayerDirection.rgb,0) ).xyz.rgb, _LayerDirection.rgb, _WorldonLocaloff )),i.normalDir))+lerp((length(node_4631.rgb)*(-1.0)),1.0,node_8836)))*length(saturate(((1.0 - node_4631.rgb)+saturate(_LayerAmountCreviceDisp.g))))*node_8836),_Power));
                float node_4093 = MossRatio;
                float Gloss = lerp(_Gloss,_LayerGloss,node_4093);
                float gloss = Gloss;
                float perceptualRoughness = 1.0 - Gloss;
                float roughness = perceptualRoughness * perceptualRoughness;
                float specPow = exp2( gloss * 10.0 + 1.0 );
/////// GI Data:
                UnityLight light;
                #ifdef LIGHTMAP_OFF
                    light.color = lightColor;
                    light.dir = lightDirection;
                    light.ndotl = LambertTerm (normalDirection, light.dir);
                #else
                    light.color = half3(0.f, 0.f, 0.f);
                    light.ndotl = 0.0f;
                    light.dir = half3(0.f, 0.f, 0.f);
                #endif
                UnityGIInput d;
                d.light = light;
                d.worldPos = i.posWorld.xyz;
                d.worldViewDir = viewDirection;
                d.atten = attenuation;
                #if defined(LIGHTMAP_ON) || defined(DYNAMICLIGHTMAP_ON)
                    d.ambient = 0;
                    d.lightmapUV = i.ambientOrLightmapUV;
                #else
                    d.ambient = i.ambientOrLightmapUV;
                #endif
                #if UNITY_SPECCUBE_BLENDING || UNITY_SPECCUBE_BOX_PROJECTION
                    d.boxMin[0] = unity_SpecCube0_BoxMin;
                    d.boxMin[1] = unity_SpecCube1_BoxMin;
                #endif
                #if UNITY_SPECCUBE_BOX_PROJECTION
                    d.boxMax[0] = unity_SpecCube0_BoxMax;
                    d.boxMax[1] = unity_SpecCube1_BoxMax;
                    d.probePosition[0] = unity_SpecCube0_ProbePosition;
                    d.probePosition[1] = unity_SpecCube1_ProbePosition;
                #endif
                d.probeHDR[0] = unity_SpecCube0_HDR;
                d.probeHDR[1] = unity_SpecCube1_HDR;
                Unity_GlossyEnvironmentData ugls_en_data;
                ugls_en_data.roughness = 1.0 - gloss;
                ugls_en_data.reflUVW = viewReflectDirection;
                UnityGI gi = UnityGlobalIllumination(d, 1, normalDirection, ugls_en_data );
                lightDirection = gi.light.dir;
                lightColor = gi.light.color;
////// Specular:
                float NdotL = saturate(dot( normalDirection, lightDirection ));
                float LdotH = saturate(dot(lightDirection, halfDirection));
                float Metallic = lerp(_Metallic,_LayerMetallic,node_4093);
                float3 specularColor = Metallic;
                float specularMonochrome;
                float2 node_9703 = Uvs;
                float4 node_9930 = tex2D(_Albedo,node_9703 + i.uvAN.xy + float2(_UVs.b * _Time.x,_UVs.a * _Time.x) * _AlbedoTimeScale);
                float4 _Layer_var = tex2D(_Layer,TRANSFORM_TEX(i.uv0, _Layer));
                float3 diffuseColor = lerp((lerp(float3(1,1,1),node_4631.rgb,_AOScale)*node_9930.rgb*_Tint.rgb),(_LayerTint.rgb*_Layer_var.rgb),MossRatio); // Need this for specular when using metallic
                diffuseColor = DiffuseAndSpecularFromMetallic( diffuseColor, specularColor, specularColor, specularMonochrome );
                specularMonochrome = 1.0-specularMonochrome;
                float NdotV = abs(dot( normalDirection, viewDirection ));
                float NdotH = saturate(dot( normalDirection, halfDirection ));
                float VdotH = saturate(dot( viewDirection, halfDirection ));
                float visTerm = SmithJointGGXVisibilityTerm( NdotL, NdotV, roughness );
                float normTerm = GGXTerm(NdotH, roughness);
                float specularPBL = (visTerm*normTerm) * UNITY_PI;
                #ifdef UNITY_COLORSPACE_GAMMA
                    specularPBL = sqrt(max(1e-4h, specularPBL));
                #endif
                specularPBL = max(0, specularPBL * NdotL);
                #if defined(_SPECULARHIGHLIGHTS_OFF)
                    specularPBL = 0.0;
                #endif
                half surfaceReduction;
                #ifdef UNITY_COLORSPACE_GAMMA
                    surfaceReduction = 1.0-0.28*roughness*perceptualRoughness;
                #else
                    surfaceReduction = 1.0/(roughness*roughness + 1.0);
                #endif
                specularPBL *= any(specularColor) ? 1.0 : 0.0;
                float3 directSpecular = attenColor*specularPBL*FresnelTerm(specularColor, LdotH);
                half grazingTerm = saturate( gloss + specularMonochrome );
                float3 indirectSpecular = (gi.indirect.specular);
                indirectSpecular *= FresnelLerp (specularColor, grazingTerm, NdotV);
                indirectSpecular *= surfaceReduction;
                float3 specular = (directSpecular + indirectSpecular);
/////// Diffuse:
                NdotL = max(0.0,dot( normalDirection, lightDirection ));
                half fd90 = 0.5 + 2 * LdotH * LdotH * (1-gloss);
                float nlPow5 = Pow5(1-NdotL);
                float nvPow5 = Pow5(1-NdotV);
                float3 directDiffuse = ((1 +(fd90 - 1)*nlPow5) * (1 + (fd90 - 1)*nvPow5) * NdotL) * attenColor;
                float3 indirectDiffuse = float3(0,0,0);
                indirectDiffuse += gi.indirect.diffuse;
                float3 diffuse = (directDiffuse + indirectDiffuse) * diffuseColor;
/// Final Color:
                float3 finalColor = diffuse + specular;
                fixed4 finalRGBA = fixed4(finalColor,1);
                UNITY_APPLY_FOG(i.fogCoord, finalRGBA);
                return finalRGBA;
            }
            ENDCG
        }
        Pass {
            Name "FORWARD_DELTA"
            Tags {
                "LightMode"="ForwardAdd"
            }
            Blend One One
            
            
            CGPROGRAM
            #pragma hull hull
            #pragma domain domain
            #pragma vertex tessvert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDADD
            #define SHOULD_SAMPLE_SH ( defined (LIGHTMAP_OFF) && defined(DYNAMICLIGHTMAP_OFF) )
            #define _GLOSSYENV 1
            #include "UnityCG.cginc"
            #include "AutoLight.cginc"
            #include "Lighting.cginc"
            #include "Tessellation.cginc"
            #include "UnityPBSLighting.cginc"
            #include "UnityStandardBRDF.cginc"
            #pragma multi_compile_fwdadd_fullshadows
            #pragma multi_compile LIGHTMAP_OFF LIGHTMAP_ON
            #pragma multi_compile DIRLIGHTMAP_OFF DIRLIGHTMAP_COMBINED DIRLIGHTMAP_SEPARATE
            #pragma multi_compile DYNAMICLIGHTMAP_OFF DYNAMICLIGHTMAP_ON
            #pragma multi_compile_fog
            #pragma only_renderers d3d9 d3d11 glcore gles 
            #pragma target 5.0
			uniform float _AlbedoTimeScale, _NormalTimeScale, _AOTimeScale,_HeightTimeScale;
            uniform float _Metallic;
            uniform float _Gloss;
            uniform sampler2D _AO;
			uniform float4 _AO_ST;
            uniform sampler2D _Albedo;
            uniform float4 _Albedo_ST;
            uniform sampler2D _Normal;
            uniform float4 _Normal_ST;
			uniform float4 _Height_ST;
            uniform float4 _UVs;
            uniform float _AOScale;
            uniform float4 _LayerDirection;
            uniform sampler2D _Layer; uniform float4 _Layer_ST;
            uniform fixed _DisplaceBase;
            uniform sampler2D _Height;
            uniform float _Displacement;
            uniform float4 _LayerTint;
            uniform float _LayerMetallic;
            uniform float _LayerGloss;
            uniform fixed _DisplaceLayer;
            uniform float _TessellationValue;
            uniform float4 _LayerAmountCreviceDisp;
            uniform float _Power;
            uniform fixed _WorldonLocaloff;
            uniform float _DistanceTesselation;
            uniform float _TessFade;
            uniform float4 _Tint;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float4 tangent : TANGENT;
                float2 texcoord0 : TEXCOORD0;
                float2 texcoord1 : TEXCOORD1;
                float2 texcoord2 : TEXCOORD2;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float2 uv1 : TEXCOORD1;
                float2 uv2 : TEXCOORD2;
				float4 uvAN : TEXCOORD9;
                float4 uvSH : TEXCOORD8;
                float4 posWorld : TEXCOORD3;
                float3 normalDir : TEXCOORD4;
                float3 tangentDir : TEXCOORD5;
                float3 bitangentDir : TEXCOORD6;
                LIGHTING_COORDS(7,8)
                UNITY_FOG_COORDS(9)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.uv1 = v.texcoord1;
                o.uv2 = v.texcoord2;
				o.uvAN = float4(TRANSFORM_TEX(v.texcoord0, _Albedo).xy,TRANSFORM_TEX(v.texcoord0, _Normal).xy);
				o.uvSH = float4(TRANSFORM_TEX(v.texcoord0, _AO).xy,TRANSFORM_TEX(v.texcoord0, _Height).xy);
                o.normalDir = UnityObjectToWorldNormal(v.normal);
                o.tangentDir = normalize( mul( unity_ObjectToWorld, float4( v.tangent.xyz, 0.0 ) ).xyz );
                o.bitangentDir = normalize(cross(o.normalDir, o.tangentDir) * v.tangent.w);
                float2 node_9152 = (o.uv0*float2(_UVs.r,_UVs.g));
                float2 Uvs = node_9152;
                float2 node_712 = Uvs;
                float4 node_3154 = tex2Dlod(_Height,float4(node_712 + o.uvSH.zw + float2(_UVs.b * _Time.x,_UVs.a * _Time.x) * _HeightTimeScale,0.0,0));
                float3 node_9139 = (((length(node_3154.rgb)-0.5)*2.0)*lerp( 0.001, _Displacement, _DisplaceBase )*v.normal);
                v.vertex.xyz += node_9139;
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                float3 lightColor = _LightColor0.rgb;
                o.pos = UnityObjectToClipPos(v.vertex );
                UNITY_TRANSFER_FOG(o,o.pos);
                TRANSFER_VERTEX_TO_FRAGMENT(o)
                return o;
            }
            #ifdef UNITY_CAN_COMPILE_TESSELLATION
                struct TessVertex {
                    float4 vertex : INTERNALTESSPOS;
                    float3 normal : NORMAL;
                    float4 tangent : TANGENT;
                    float2 texcoord0 : TEXCOORD0;
                    float2 texcoord1 : TEXCOORD1;
                    float2 texcoord2 : TEXCOORD2;
                };
                struct OutputPatchConstant {
                    float edge[3]         : SV_TessFactor;
                    float inside          : SV_InsideTessFactor;
                    float3 vTangent[4]    : TANGENT;
                    float2 vUV[4]         : TEXCOORD;
                    float3 vTanUCorner[4] : TANUCORNER;
                    float3 vTanVCorner[4] : TANVCORNER;
                    float4 vCWts          : TANWEIGHTS;
                };
                TessVertex tessvert (VertexInput v) {
                    TessVertex o;
                    o.vertex = v.vertex;
                    o.normal = v.normal;
                    o.tangent = v.tangent;
                    o.texcoord0 = v.texcoord0;
                    o.texcoord1 = v.texcoord1;
                    o.texcoord2 = v.texcoord2;
                    return o;
                }
                void displacement (inout VertexInput v){
                    float2 node_9152 = (v.texcoord0*float2(_UVs.r,_UVs.g));
                    float4 node_4631 = tex2Dlod(_AO,float4(node_9152 + TRANSFORM_TEX(v.texcoord0, _AO).xy + float2(_UVs.b * _Time.x,_UVs.a * _Time.x) * _AOTimeScale,0.0,0));
                    float node_8836 = saturate(_LayerAmountCreviceDisp.r);
                    float MossRatio = saturate(pow((saturate((max(0,dot(normalize(lerp( mul( unity_ObjectToWorld, float4(_LayerDirection.rgb,0) ).xyz.rgb, _LayerDirection.rgb, _WorldonLocaloff )),v.normal))+lerp((length(node_4631.rgb)*(-1.0)),1.0,node_8836)))*length(saturate(((1.0 - node_4631.rgb)+saturate(_LayerAmountCreviceDisp.g))))*node_8836),_Power));
                    float _DisplaceLayer_var = lerp( 0.0, (clamp(_LayerAmountCreviceDisp.b,0.001,1.0)*sqrt(MossRatio)), _DisplaceLayer );
                    float3 MossDisplacement = (_DisplaceLayer_var*v.normal);
                    v.vertex.xyz += MossDisplacement;
                }
                float Tessellation(TessVertex v){
                    float2 node_9152 = (v.texcoord0*float2(_UVs.r,_UVs.g));
                    float2 Uvs = node_9152;
                    float2 node_712 = Uvs;
                    float4 node_3154 = tex2Dlod(_Height,float4(node_712 + TRANSFORM_TEX(v.texcoord0, _Height).xy + float2(_UVs.b * _Time.x,_UVs.a * _Time.x) * _HeightTimeScale,0.0,0));
                    float3 node_9139 = (((length(node_3154.rgb)-0.5)*2.0)*lerp( 0.001, _Displacement, _DisplaceBase )*v.normal);
                    float4 node_4631 = tex2Dlod(_AO,float4(node_9152 + TRANSFORM_TEX(v.texcoord0, _AO).xy + float2(_UVs.b * _Time.x,_UVs.a * _Time.x) * _AOTimeScale,0.0,0));
                    float node_8836 = saturate(_LayerAmountCreviceDisp.r);
                    float MossRatio = saturate(pow((saturate((max(0,dot(normalize(lerp( mul( unity_ObjectToWorld, float4(_LayerDirection.rgb,0) ).xyz.rgb, _LayerDirection.rgb, _WorldonLocaloff )),v.normal))+lerp((length(node_4631.rgb)*(-1.0)),1.0,node_8836)))*length(saturate(((1.0 - node_4631.rgb)+saturate(_LayerAmountCreviceDisp.g))))*node_8836),_Power));
                    float _DisplaceLayer_var = lerp( 0.0, (clamp(_LayerAmountCreviceDisp.b,0.001,1.0)*sqrt(MossRatio)), _DisplaceLayer );
                    float node_4962 = 1.0;
                    return clamp((_TessellationValue*(length(node_9139)+_DisplaceLayer_var)*100.0*(_DistanceTesselation*clamp((-1*(distance(mul(unity_ObjectToWorld, v.vertex).rgb,_WorldSpaceCameraPos)-_TessFade)),0.0,node_4962))),node_4962,10000.0);
                }
                float4 Tessellation(TessVertex v, TessVertex v1, TessVertex v2){
                    float tv = Tessellation(v);
                    float tv1 = Tessellation(v1);
                    float tv2 = Tessellation(v2);
                    return float4( tv1+tv2, tv2+tv, tv+tv1, tv+tv1+tv2 ) / float4(2,2,2,3);
                }
                OutputPatchConstant hullconst (InputPatch<TessVertex,3> v) {
                    OutputPatchConstant o = (OutputPatchConstant)0;
                    float4 ts = Tessellation( v[0], v[1], v[2] );
                    o.edge[0] = ts.x;
                    o.edge[1] = ts.y;
                    o.edge[2] = ts.z;
                    o.inside = ts.w;
                    return o;
                }
                [domain("tri")]
                [partitioning("fractional_odd")]
                [outputtopology("triangle_cw")]
                [patchconstantfunc("hullconst")]
                [outputcontrolpoints(3)]
                TessVertex hull (InputPatch<TessVertex,3> v, uint id : SV_OutputControlPointID) {
                    return v[id];
                }
                [domain("tri")]
                VertexOutput domain (OutputPatchConstant tessFactors, const OutputPatch<TessVertex,3> vi, float3 bary : SV_DomainLocation) {
                    VertexInput v = (VertexInput)0;
                    v.vertex = vi[0].vertex*bary.x + vi[1].vertex*bary.y + vi[2].vertex*bary.z;
                    v.normal = vi[0].normal*bary.x + vi[1].normal*bary.y + vi[2].normal*bary.z;
                    v.tangent = vi[0].tangent*bary.x + vi[1].tangent*bary.y + vi[2].tangent*bary.z;
                    v.texcoord0 = vi[0].texcoord0*bary.x + vi[1].texcoord0*bary.y + vi[2].texcoord0*bary.z;
                    v.texcoord1 = vi[0].texcoord1*bary.x + vi[1].texcoord1*bary.y + vi[2].texcoord1*bary.z;
                    displacement(v);
                    VertexOutput o = vert(v);
                    return o;
                }
            #endif
            float4 frag(VertexOutput i) : COLOR {
                i.normalDir = normalize(i.normalDir);
                float3x3 tangentTransform = float3x3( i.tangentDir, i.bitangentDir, i.normalDir);
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                float2 node_9152 = (i.uv0*float2(_UVs.r,_UVs.g));
                float2 Uvs = node_9152;
                float2 node_9237 = Uvs;
                float3 node_739 = UnpackNormal(tex2D(_Normal,node_9237 + i.uvAN.zw + float2(_UVs.b * _Time.x,_UVs.a * _Time.x) * _NormalTimeScale));
                float3 normalLocal = node_739.rgb;
                float3 normalDirection = normalize(mul( normalLocal, tangentTransform )); // Perturbed normals
                float3 lightDirection = normalize(lerp(_WorldSpaceLightPos0.xyz, _WorldSpaceLightPos0.xyz - i.posWorld.xyz,_WorldSpaceLightPos0.w));
                float3 lightColor = _LightColor0.rgb;
                float3 halfDirection = normalize(viewDirection+lightDirection);
////// Lighting:
                float attenuation = LIGHT_ATTENUATION(i);
                float3 attenColor = attenuation * _LightColor0.xyz;
                float Pi = 3.141592654;
                float InvPi = 0.31830988618;
///////// Gloss:
                float4 node_4631 = tex2D(_AO,node_9152 + i.uvSH.xy + float2(_UVs.b * _Time.x,_UVs.a * _Time.x) * _AOTimeScale);
                float node_8836 = saturate(_LayerAmountCreviceDisp.r);
                float MossRatio = saturate(pow((saturate((max(0,dot(normalize(lerp( mul( unity_ObjectToWorld, float4(_LayerDirection.rgb,0) ).xyz.rgb, _LayerDirection.rgb, _WorldonLocaloff )),i.normalDir))+lerp((length(node_4631.rgb)*(-1.0)),1.0,node_8836)))*length(saturate(((1.0 - node_4631.rgb)+saturate(_LayerAmountCreviceDisp.g))))*node_8836),_Power));
                float node_4093 = MossRatio;
                float Gloss = lerp(_Gloss,_LayerGloss,node_4093);
                float gloss = Gloss;
                float perceptualRoughness = 1.0 - Gloss;
                float roughness = perceptualRoughness * perceptualRoughness;
                float specPow = exp2( gloss * 10.0 + 1.0 );
////// Specular:
                float NdotL = saturate(dot( normalDirection, lightDirection ));
                float LdotH = saturate(dot(lightDirection, halfDirection));
                float Metallic = lerp(_Metallic,_LayerMetallic,node_4093);
                float3 specularColor = Metallic;
                float specularMonochrome;
                float2 node_9703 = Uvs;
                float4 node_9930 = tex2D(_Albedo,node_9703 + i.uvAN.xy + float2(_UVs.b * _Time.x,_UVs.a * _Time.x) * _AlbedoTimeScale);
                float4 _Layer_var = tex2D(_Layer,TRANSFORM_TEX(i.uv0, _Layer));
                float3 diffuseColor = lerp((lerp(float3(1,1,1),node_4631.rgb,_AOScale)*node_9930.rgb*_Tint.rgb),(_LayerTint.rgb*_Layer_var.rgb),MossRatio); // Need this for specular when using metallic
                diffuseColor = DiffuseAndSpecularFromMetallic( diffuseColor, specularColor, specularColor, specularMonochrome );
                specularMonochrome = 1.0-specularMonochrome;
                float NdotV = abs(dot( normalDirection, viewDirection ));
                float NdotH = saturate(dot( normalDirection, halfDirection ));
                float VdotH = saturate(dot( viewDirection, halfDirection ));
                float visTerm = SmithJointGGXVisibilityTerm( NdotL, NdotV, roughness );
                float normTerm = GGXTerm(NdotH, roughness);
                float specularPBL = (visTerm*normTerm) * UNITY_PI;
                #ifdef UNITY_COLORSPACE_GAMMA
                    specularPBL = sqrt(max(1e-4h, specularPBL));
                #endif
                specularPBL = max(0, specularPBL * NdotL);
                #if defined(_SPECULARHIGHLIGHTS_OFF)
                    specularPBL = 0.0;
                #endif
                specularPBL *= any(specularColor) ? 1.0 : 0.0;
                float3 directSpecular = attenColor*specularPBL*FresnelTerm(specularColor, LdotH);
                float3 specular = directSpecular;
/////// Diffuse:
                NdotL = max(0.0,dot( normalDirection, lightDirection ));
                half fd90 = 0.5 + 2 * LdotH * LdotH * (1-gloss);
                float nlPow5 = Pow5(1-NdotL);
                float nvPow5 = Pow5(1-NdotV);
                float3 directDiffuse = ((1 +(fd90 - 1)*nlPow5) * (1 + (fd90 - 1)*nvPow5) * NdotL) * attenColor;
                float3 diffuse = directDiffuse * diffuseColor;
/// Final Color:
                float3 finalColor = diffuse + specular;
                fixed4 finalRGBA = fixed4(finalColor * 1,0);
                UNITY_APPLY_FOG(i.fogCoord, finalRGBA);
                return finalRGBA;
            }
            ENDCG
        }
        Pass {
            Name "ShadowCaster"
            Tags {
                "LightMode"="ShadowCaster"
            }
            Offset 1, 1
            Cull Back
            
            CGPROGRAM
            #pragma hull hull
            #pragma domain domain
            #pragma vertex tessvert
            #pragma fragment frag
            #define UNITY_PASS_SHADOWCASTER
            #define SHOULD_SAMPLE_SH ( defined (LIGHTMAP_OFF) && defined(DYNAMICLIGHTMAP_OFF) )
            #define _GLOSSYENV 1
            #include "UnityCG.cginc"
            #include "Lighting.cginc"
            #include "Tessellation.cginc"
            #include "UnityPBSLighting.cginc"
            #include "UnityStandardBRDF.cginc"
            #pragma fragmentoption ARB_precision_hint_fastest
            #pragma multi_compile_shadowcaster
            #pragma multi_compile LIGHTMAP_OFF LIGHTMAP_ON
            #pragma multi_compile DIRLIGHTMAP_OFF DIRLIGHTMAP_COMBINED DIRLIGHTMAP_SEPARATE
            #pragma multi_compile DYNAMICLIGHTMAP_OFF DYNAMICLIGHTMAP_ON
            #pragma multi_compile_fog
            #pragma only_renderers d3d9 d3d11 glcore gles 
            #pragma target 5.0
			uniform float _AlbedoTimeScale, _NormalTimeScale, _AOTimeScale,_HeightTimeScale;
			uniform sampler2D _AO;
			uniform float4 _AO_ST;
            uniform sampler2D _Albedo;
            uniform float4 _Albedo_ST;
            uniform sampler2D _Normal;
            uniform float4 _Normal_ST;
			uniform float4 _Height_ST;
            uniform float4 _UVs;
            uniform float4 _LayerDirection;
            uniform fixed _DisplaceBase;
            uniform sampler2D _Height;
            uniform float _Displacement;
            uniform fixed _DisplaceLayer;
            uniform float _TessellationValue;
            uniform float4 _LayerAmountCreviceDisp;
            uniform float _Power;
            uniform fixed _WorldonLocaloff;
            uniform float _DistanceTesselation;
            uniform float _TessFade;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float4 tangent : TANGENT;
                float2 texcoord0 : TEXCOORD0;
                float2 texcoord1 : TEXCOORD1;
                float2 texcoord2 : TEXCOORD2;
            };
            struct VertexOutput {
                V2F_SHADOW_CASTER;
                float2 uv0 : TEXCOORD1;
                float2 uv1 : TEXCOORD2;
                float2 uv2 : TEXCOORD3;
                float4 uvAN : TEXCOORD9;
                float4 uvSH : TEXCOORD8;
                float4 posWorld : TEXCOORD4;
                float3 normalDir : TEXCOORD5;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.uv1 = v.texcoord1;
                o.uv2 = v.texcoord2;
				o.uvAN = float4(TRANSFORM_TEX(v.texcoord0, _Albedo).xy,TRANSFORM_TEX(v.texcoord0, _Normal).xy);
				o.uvSH = float4(TRANSFORM_TEX(v.texcoord0, _AO).xy,TRANSFORM_TEX(v.texcoord0, _Height).xy);
                o.normalDir = UnityObjectToWorldNormal(v.normal);
                float2 node_9152 = (o.uv0*float2(_UVs.r,_UVs.g));
                float2 Uvs = node_9152;
                float2 node_712 = Uvs;
                float4 node_3154 = tex2Dlod(_Height,float4(node_712 + o.uvSH.zw + float2(_UVs.b * _Time.x,_UVs.a * _Time.x) * _HeightTimeScale,0.0,0));
                float3 node_9139 = (((length(node_3154.rgb)-0.5)*2.0)*lerp( 0.001, _Displacement, _DisplaceBase )*v.normal);
                v.vertex.xyz += node_9139;
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                o.pos = UnityObjectToClipPos(v.vertex );
                TRANSFER_SHADOW_CASTER(o)
                return o;
            }
            #ifdef UNITY_CAN_COMPILE_TESSELLATION
                struct TessVertex {
                    float4 vertex : INTERNALTESSPOS;
                    float3 normal : NORMAL;
                    float4 tangent : TANGENT;
                    float2 texcoord0 : TEXCOORD0;
                    float2 texcoord1 : TEXCOORD1;
                    float2 texcoord2 : TEXCOORD2;
                };
                struct OutputPatchConstant {
                    float edge[3]         : SV_TessFactor;
                    float inside          : SV_InsideTessFactor;
                    float3 vTangent[4]    : TANGENT;
                    float2 vUV[4]         : TEXCOORD;
                    float3 vTanUCorner[4] : TANUCORNER;
                    float3 vTanVCorner[4] : TANVCORNER;
                    float4 vCWts          : TANWEIGHTS;
                };
                TessVertex tessvert (VertexInput v) {
                    TessVertex o;
                    o.vertex = v.vertex;
                    o.normal = v.normal;
                    o.tangent = v.tangent;
                    o.texcoord0 = v.texcoord0;
                    o.texcoord1 = v.texcoord1;
                    o.texcoord2 = v.texcoord2;
                    return o;
                }
                void displacement (inout VertexInput v){
                    float2 node_9152 = (v.texcoord0*float2(_UVs.r,_UVs.g));
                    float4 node_4631 = tex2Dlod(_AO,float4(node_9152 + TRANSFORM_TEX(v.texcoord0, _AO).xy + float2(_UVs.b * _Time.x,_UVs.a * _Time.x) * _AOTimeScale,0.0,0));
                    float node_8836 = saturate(_LayerAmountCreviceDisp.r);
                    float MossRatio = saturate(pow((saturate((max(0,dot(normalize(lerp( mul( unity_ObjectToWorld, float4(_LayerDirection.rgb,0) ).xyz.rgb, _LayerDirection.rgb, _WorldonLocaloff )),v.normal))+lerp((length(node_4631.rgb)*(-1.0)),1.0,node_8836)))*length(saturate(((1.0 - node_4631.rgb)+saturate(_LayerAmountCreviceDisp.g))))*node_8836),_Power));
                    float _DisplaceLayer_var = lerp( 0.0, (clamp(_LayerAmountCreviceDisp.b,0.001,1.0)*sqrt(MossRatio)), _DisplaceLayer );
                    float3 MossDisplacement = (_DisplaceLayer_var*v.normal);
                    v.vertex.xyz += MossDisplacement;
                }
                float Tessellation(TessVertex v){
                    float2 node_9152 = (v.texcoord0*float2(_UVs.r,_UVs.g));
                    float2 Uvs = node_9152;
                    float2 node_712 = Uvs;
                    float4 node_3154 = tex2Dlod(_Height,float4(node_712+ TRANSFORM_TEX(v.texcoord0, _Height).xy + float2(_UVs.b * _Time.x,_UVs.a * _Time.x) * _HeightTimeScale,0.0,0));
                    float3 node_9139 = (((length(node_3154.rgb)-0.5)*2.0)*lerp( 0.001, _Displacement, _DisplaceBase )*v.normal);
                    float4 node_4631 = tex2Dlod(_AO,float4(node_9152+ TRANSFORM_TEX(v.texcoord0, _AO).xy + float2(_UVs.b * _Time.x,_UVs.a * _Time.x) * _AOTimeScale,0.0,0));
                    float node_8836 = saturate(_LayerAmountCreviceDisp.r);
                    float MossRatio = saturate(pow((saturate((max(0,dot(normalize(lerp( mul( unity_ObjectToWorld, float4(_LayerDirection.rgb,0) ).xyz.rgb, _LayerDirection.rgb, _WorldonLocaloff )),v.normal))+lerp((length(node_4631.rgb)*(-1.0)),1.0,node_8836)))*length(saturate(((1.0 - node_4631.rgb)+saturate(_LayerAmountCreviceDisp.g))))*node_8836),_Power));
                    float _DisplaceLayer_var = lerp( 0.0, (clamp(_LayerAmountCreviceDisp.b,0.001,1.0)*sqrt(MossRatio)), _DisplaceLayer );
                    float node_4962 = 1.0;
                    return clamp((_TessellationValue*(length(node_9139)+_DisplaceLayer_var)*100.0*(_DistanceTesselation*clamp((-1*(distance(mul(unity_ObjectToWorld, v.vertex).rgb,_WorldSpaceCameraPos)-_TessFade)),0.0,node_4962))),node_4962,10000.0);
                }
                float4 Tessellation(TessVertex v, TessVertex v1, TessVertex v2){
                    float tv = Tessellation(v);
                    float tv1 = Tessellation(v1);
                    float tv2 = Tessellation(v2);
                    return float4( tv1+tv2, tv2+tv, tv+tv1, tv+tv1+tv2 ) / float4(2,2,2,3);
                }
                OutputPatchConstant hullconst (InputPatch<TessVertex,3> v) {
                    OutputPatchConstant o = (OutputPatchConstant)0;
                    float4 ts = Tessellation( v[0], v[1], v[2] );
                    o.edge[0] = ts.x;
                    o.edge[1] = ts.y;
                    o.edge[2] = ts.z;
                    o.inside = ts.w;
                    return o;
                }
                [domain("tri")]
                [partitioning("fractional_odd")]
                [outputtopology("triangle_cw")]
                [patchconstantfunc("hullconst")]
                [outputcontrolpoints(3)]
                TessVertex hull (InputPatch<TessVertex,3> v, uint id : SV_OutputControlPointID) {
                    return v[id];
                }
                [domain("tri")]
                VertexOutput domain (OutputPatchConstant tessFactors, const OutputPatch<TessVertex,3> vi, float3 bary : SV_DomainLocation) {
                    VertexInput v = (VertexInput)0;
                    v.vertex = vi[0].vertex*bary.x + vi[1].vertex*bary.y + vi[2].vertex*bary.z;
                    v.normal = vi[0].normal*bary.x + vi[1].normal*bary.y + vi[2].normal*bary.z;
                    v.tangent = vi[0].tangent*bary.x + vi[1].tangent*bary.y + vi[2].tangent*bary.z;
                    v.texcoord0 = vi[0].texcoord0*bary.x + vi[1].texcoord0*bary.y + vi[2].texcoord0*bary.z;
                    v.texcoord1 = vi[0].texcoord1*bary.x + vi[1].texcoord1*bary.y + vi[2].texcoord1*bary.z;
                    displacement(v);
                    VertexOutput o = vert(v);
                    return o;
                }
            #endif
            float4 frag(VertexOutput i) : COLOR {
                i.normalDir = normalize(i.normalDir);
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                float3 normalDirection = i.normalDir;
                SHADOW_CASTER_FRAGMENT(i)
            }
            ENDCG
        }
        Pass {
            Name "Meta"
            Tags {
                "LightMode"="Meta"
            }
            Cull Off
            
            CGPROGRAM
            #pragma hull hull
            #pragma domain domain
            #pragma vertex tessvert
            #pragma fragment frag
            #define UNITY_PASS_META 1
            #define SHOULD_SAMPLE_SH ( defined (LIGHTMAP_OFF) && defined(DYNAMICLIGHTMAP_OFF) )
            #define _GLOSSYENV 1
            #include "UnityCG.cginc"
            #include "Lighting.cginc"
            #include "Tessellation.cginc"
            #include "UnityPBSLighting.cginc"
            #include "UnityStandardBRDF.cginc"
            #include "UnityMetaPass.cginc"
            #pragma fragmentoption ARB_precision_hint_fastest
            #pragma multi_compile_shadowcaster
            #pragma multi_compile LIGHTMAP_OFF LIGHTMAP_ON
            #pragma multi_compile DIRLIGHTMAP_OFF DIRLIGHTMAP_COMBINED DIRLIGHTMAP_SEPARATE
            #pragma multi_compile DYNAMICLIGHTMAP_OFF DYNAMICLIGHTMAP_ON
            #pragma multi_compile_fog
            #pragma only_renderers d3d9 d3d11 glcore gles 
            #pragma target 5.0
			uniform float _AlbedoTimeScale, _NormalTimeScale, _AOTimeScale,_HeightTimeScale;
            uniform float _Metallic;
            uniform float _Gloss;
            uniform sampler2D _AO;
			uniform float4 _AO_ST;
			uniform sampler2D _Albedo;
            uniform float4 _Albedo_ST;
            uniform sampler2D _Normal;
            uniform float4 _Normal_ST;
            uniform float4 _UVs;
            uniform float _AOScale;
            uniform float4 _LayerDirection;
            uniform sampler2D _Layer; uniform float4 _Layer_ST;
            uniform fixed _DisplaceBase;
            uniform sampler2D _Height;
			uniform float4 _Height_ST;
            uniform float _Displacement;
            uniform float4 _LayerTint;
            uniform float _LayerMetallic;
            uniform float _LayerGloss;
            uniform fixed _DisplaceLayer;
            uniform float _TessellationValue;
            uniform float4 _LayerAmountCreviceDisp;
            uniform float _Power;
            uniform fixed _WorldonLocaloff;
            uniform float _DistanceTesselation;
            uniform float _TessFade;
            uniform float4 _Tint;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float4 tangent : TANGENT;
                float2 texcoord0 : TEXCOORD0;
                float2 texcoord1 : TEXCOORD1;
                float2 texcoord2 : TEXCOORD2;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float2 uv1 : TEXCOORD1;
                float2 uv2 : TEXCOORD2;
				float4 uvAN : TEXCOORD9;
                float4 uvSH : TEXCOORD8;
                float4 posWorld : TEXCOORD3;
                float3 normalDir : TEXCOORD4;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.uv1 = v.texcoord1;
                o.uv2 = v.texcoord2;
                o.normalDir = UnityObjectToWorldNormal(v.normal);
                float2 node_9152 = (o.uv0*float2(_UVs.r,_UVs.g));
                float2 Uvs = node_9152;
                float2 node_712 = Uvs;
                float4 node_3154 = tex2Dlod(_Height,float4(node_712 + TRANSFORM_TEX(v.texcoord0, _Height).xy + float2(_UVs.b * _Time.x,_UVs.a * _Time.x) * _HeightTimeScale,0,0));
                float3 node_9139 = (((length(node_3154.rgb)-0.5)*2.0)*lerp( 0.001, _Displacement, _DisplaceBase )*v.normal);
                v.vertex.xyz += node_9139;
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                o.pos = UnityMetaVertexPosition(v.vertex, v.texcoord1.xy, v.texcoord2.xy, unity_LightmapST, unity_DynamicLightmapST );
                return o;
            }
            #ifdef UNITY_CAN_COMPILE_TESSELLATION
                struct TessVertex {
                    float4 vertex : INTERNALTESSPOS;
                    float3 normal : NORMAL;
                    float4 tangent : TANGENT;
                    float2 texcoord0 : TEXCOORD0;
                    float2 texcoord1 : TEXCOORD1;
                    float2 texcoord2 : TEXCOORD2;
                };
                struct OutputPatchConstant {
                    float edge[3]         : SV_TessFactor;
                    float inside          : SV_InsideTessFactor;
                    float3 vTangent[4]    : TANGENT;
                    float2 vUV[4]         : TEXCOORD;
                    float3 vTanUCorner[4] : TANUCORNER;
                    float3 vTanVCorner[4] : TANVCORNER;
                    float4 vCWts          : TANWEIGHTS;
                };
                TessVertex tessvert (VertexInput v) {
                    TessVertex o;
                    o.vertex = v.vertex;
                    o.normal = v.normal;
                    o.tangent = v.tangent;
                    o.texcoord0 = v.texcoord0;
                    o.texcoord1 = v.texcoord1;
                    o.texcoord2 = v.texcoord2;
                    return o;
                }
                void displacement (inout VertexInput v){
                    float2 node_9152 = (v.texcoord0*float2(_UVs.r,_UVs.g));
                    float4 node_4631 = tex2Dlod(_AO,float4(node_9152 + TRANSFORM_TEX(v.texcoord0, _AO).xy + float2(_UVs.b * _Time.x,_UVs.a * _Time.x) * _AOTimeScale,0,0));
                    float node_8836 = saturate(_LayerAmountCreviceDisp.r);
                    float MossRatio = saturate(pow((saturate((max(0,dot(normalize(lerp( mul( unity_ObjectToWorld, float4(_LayerDirection.rgb,0) ).xyz.rgb, _LayerDirection.rgb, _WorldonLocaloff )),v.normal))+lerp((length(node_4631.rgb)*(-1.0)),1.0,node_8836)))*length(saturate(((1.0 - node_4631.rgb)+saturate(_LayerAmountCreviceDisp.g))))*node_8836),_Power));
                    float _DisplaceLayer_var = lerp( 0.0, (clamp(_LayerAmountCreviceDisp.b,0.001,1.0)*sqrt(MossRatio)), _DisplaceLayer );
                    float3 MossDisplacement = (_DisplaceLayer_var*v.normal);
                    v.vertex.xyz += MossDisplacement;
                }
                float Tessellation(TessVertex v){
                    float2 node_9152 = (v.texcoord0*float2(_UVs.r,_UVs.g));
                    float2 Uvs = node_9152;
                    float2 node_712 = Uvs;
                    float4 node_3154 = tex2Dlod(_Height,float4(node_712 + TRANSFORM_TEX(v.texcoord0, _Height).xy + float2(_UVs.b * _Time.x,_UVs.a * _Time.x) * _HeightTimeScale,0,0));
                    float3 node_9139 = (((length(node_3154.rgb)-0.5)*2.0)*lerp( 0.001, _Displacement, _DisplaceBase )*v.normal);
                    float4 node_4631 = tex2Dlod(_AO,float4(node_9152 + TRANSFORM_TEX(v.texcoord0, _AO).xy + float2(_UVs.b * _Time.x,_UVs.a * _Time.x) * _AOTimeScale,0,0));
                    float node_8836 = saturate(_LayerAmountCreviceDisp.r);
                    float MossRatio = saturate(pow((saturate((max(0,dot(normalize(lerp( mul( unity_ObjectToWorld, float4(_LayerDirection.rgb,0) ).xyz.rgb, _LayerDirection.rgb, _WorldonLocaloff )),v.normal))+lerp((length(node_4631.rgb)*(-1.0)),1.0,node_8836)))*length(saturate(((1.0 - node_4631.rgb)+saturate(_LayerAmountCreviceDisp.g))))*node_8836),_Power));
                    float _DisplaceLayer_var = lerp( 0.0, (clamp(_LayerAmountCreviceDisp.b,0.001,1.0)*sqrt(MossRatio)), _DisplaceLayer );
                    float node_4962 = 1.0;
                    return clamp((_TessellationValue*(length(node_9139)+_DisplaceLayer_var)*100.0*(_DistanceTesselation*clamp((-1*(distance(mul(unity_ObjectToWorld, v.vertex).rgb,_WorldSpaceCameraPos)-_TessFade)),0.0,node_4962))),node_4962,10000.0);
                }
                float4 Tessellation(TessVertex v, TessVertex v1, TessVertex v2){
                    float tv = Tessellation(v);
                    float tv1 = Tessellation(v1);
                    float tv2 = Tessellation(v2);
                    return float4( tv1+tv2, tv2+tv, tv+tv1, tv+tv1+tv2 ) / float4(2,2,2,3);
                }
                OutputPatchConstant hullconst (InputPatch<TessVertex,3> v) {
                    OutputPatchConstant o = (OutputPatchConstant)0;
                    float4 ts = Tessellation( v[0], v[1], v[2] );
                    o.edge[0] = ts.x;
                    o.edge[1] = ts.y;
                    o.edge[2] = ts.z;
                    o.inside = ts.w;
                    return o;
                }
                [domain("tri")]
                [partitioning("fractional_odd")]
                [outputtopology("triangle_cw")]
                [patchconstantfunc("hullconst")]
                [outputcontrolpoints(3)]
                TessVertex hull (InputPatch<TessVertex,3> v, uint id : SV_OutputControlPointID) {
                    return v[id];
                }
                [domain("tri")]
                VertexOutput domain (OutputPatchConstant tessFactors, const OutputPatch<TessVertex,3> vi, float3 bary : SV_DomainLocation) {
                    VertexInput v = (VertexInput)0;
                    v.vertex = vi[0].vertex*bary.x + vi[1].vertex*bary.y + vi[2].vertex*bary.z;
                    v.normal = vi[0].normal*bary.x + vi[1].normal*bary.y + vi[2].normal*bary.z;
                    v.tangent = vi[0].tangent*bary.x + vi[1].tangent*bary.y + vi[2].tangent*bary.z;
                    v.texcoord0 = vi[0].texcoord0*bary.x + vi[1].texcoord0*bary.y + vi[2].texcoord0*bary.z;
                    v.texcoord1 = vi[0].texcoord1*bary.x + vi[1].texcoord1*bary.y + vi[2].texcoord1*bary.z;
                    displacement(v);
                    VertexOutput o = vert(v);
                    return o;
                }
            #endif
            float4 frag(VertexOutput i) : SV_Target {
                i.normalDir = normalize(i.normalDir);
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                float3 normalDirection = i.normalDir;
                UnityMetaInput o;
                UNITY_INITIALIZE_OUTPUT( UnityMetaInput, o );
                
                o.Emission = 0;
                
                float2 node_9152 = (i.uv0*float2(_UVs.r,_UVs.g));
                float4 node_4631 = tex2D(_AO,node_9152+ i.uvSH.xy + float2(_UVs.b * _Time.x,_UVs.a * _Time.x) * _AOTimeScale);
                float2 Uvs = node_9152;
                float2 node_9703 = Uvs;
                float4 node_9930 = tex2D(_Albedo,node_9703 + i.uvAN.xy + float2(_UVs.b * _Time.x,_UVs.a * _Time.x) * _AlbedoTimeScale);
                float4 _Layer_var = tex2D(_Layer,TRANSFORM_TEX(i.uv0, _Layer));
                float node_8836 = saturate(_LayerAmountCreviceDisp.r);
                float MossRatio = saturate(pow((saturate((max(0,dot(normalize(lerp( mul( unity_ObjectToWorld, float4(_LayerDirection.rgb,0) ).xyz.rgb, _LayerDirection.rgb, _WorldonLocaloff )),i.normalDir))+lerp((length(node_4631.rgb)*(-1.0)),1.0,node_8836)))*length(saturate(((1.0 - node_4631.rgb)+saturate(_LayerAmountCreviceDisp.g))))*node_8836),_Power));
                float3 diffColor = lerp((lerp(float3(1,1,1),node_4631.rgb,_AOScale)*node_9930.rgb*_Tint.rgb),(_LayerTint.rgb*_Layer_var.rgb),MossRatio);
                float specularMonochrome;
                float3 specColor;
                float node_4093 = MossRatio;
                float Metallic = lerp(_Metallic,_LayerMetallic,node_4093);
                diffColor = DiffuseAndSpecularFromMetallic( diffColor, Metallic, specColor, specularMonochrome );
                float Gloss = lerp(_Gloss,_LayerGloss,node_4093);
                float roughness = 1.0 - Gloss;
                o.Albedo = diffColor + specColor * roughness * roughness * 0.5;
                
                return UnityMetaFragment( o );
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}
