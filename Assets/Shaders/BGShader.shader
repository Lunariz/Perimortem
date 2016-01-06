// Shader created with Shader Forge Beta 0.36 
// Shader Forge (c) Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:0.36;sub:START;pass:START;ps:flbk:,lico:1,lgpr:1,nrmq:1,limd:1,uamb:True,mssp:True,lmpd:False,lprd:False,enco:False,frtr:True,vitr:True,dbil:False,rmgx:True,rpth:0,hqsc:True,hqlp:False,tesm:0,blpr:0,bsrc:0,bdst:0,culm:0,dpts:2,wrdp:True,ufog:True,aust:True,igpj:False,qofs:0,qpre:1,rntp:1,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,ofsf:0,ofsu:0,f2p0:False;n:type:ShaderForge.SFN_Final,id:1,x:32528,y:32790|diff-16-OUT;n:type:ShaderForge.SFN_Color,id:2,x:33222,y:32622,ptlb:Color_Top,ptin:_Color_Top,glob:False,c1:0.4078431,c2:0.322,c3:0.5058824,c4:1;n:type:ShaderForge.SFN_Color,id:3,x:33370,y:32884,ptlb:Color_Mid,ptin:_Color_Mid,glob:False,c1:0.4862745,c2:0.284,c3:0.3921569,c4:1;n:type:ShaderForge.SFN_Color,id:4,x:33191,y:33116,ptlb:Color_Bot,ptin:_Color_Bot,glob:False,c1:0.8156863,c2:0.5,c3:0.6745098,c4:1;n:type:ShaderForge.SFN_Multiply,id:5,x:33030,y:32932|A-47-OUT,B-3-RGB;n:type:ShaderForge.SFN_Tex2d,id:11,x:33354,y:32615,ptlb:Blend_Top,ptin:_Blend_Top,tex:d553a3941891b3d4e81813f0f677434d,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Color,id:12,x:32999,y:32561,ptlb:Color_BG,ptin:_Color_BG,glob:False,c1:0.2867528,c2:0.06033738,c3:0.4558824,c4:1;n:type:ShaderForge.SFN_Tex2d,id:13,x:33502,y:32884,ptlb:Blend_Mid,ptin:_Blend_Mid,tex:6b1f023ec04b95043b708ea275729932,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Tex2d,id:14,x:33363,y:33237,ptlb:Blend_Bot,ptin:_Blend_Bot,tex:9e55e591d2d5fde45812c473052b525a,ntxv:3,isnm:False;n:type:ShaderForge.SFN_Multiply,id:15,x:33035,y:32768|A-2-RGB,B-40-OUT;n:type:ShaderForge.SFN_Add,id:16,x:32838,y:32872|A-12-RGB,B-15-OUT,C-5-OUT,D-33-OUT;n:type:ShaderForge.SFN_Multiply,id:22,x:33190,y:33265|A-14-RGB,B-14-A;n:type:ShaderForge.SFN_Multiply,id:33,x:33018,y:33140|A-4-RGB,B-22-OUT;n:type:ShaderForge.SFN_Multiply,id:40,x:33222,y:32768|A-11-RGB,B-11-A;n:type:ShaderForge.SFN_Multiply,id:47,x:33404,y:33047|A-13-RGB,B-13-A;proporder:2-3-4-11-12-13-14;pass:END;sub:END;*/

Shader "Shader Forge/BGShader" {
    Properties {
        _Color_Top ("Color_Top", Color) = (0.4078431,0.322,0.5058824,1)
        _Color_Mid ("Color_Mid", Color) = (0.4862745,0.284,0.3921569,1)
        _Color_Bot ("Color_Bot", Color) = (0.8156863,0.5,0.6745098,1)
        _Blend_Top ("Blend_Top", 2D) = "white" {}
        _Color_BG ("Color_BG", Color) = (0.2867528,0.06033738,0.4558824,1)
        _Blend_Mid ("Blend_Mid", 2D) = "white" {}
        _Blend_Bot ("Blend_Bot", 2D) = "bump" {}
    }
    SubShader {
        Tags {
            "RenderType"="Opaque"
        }
        Pass {
            Name "ForwardBase"
            Tags {
                "LightMode"="ForwardBase"
            }
            
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDBASE
            #include "UnityCG.cginc"
            #include "AutoLight.cginc"
            #pragma multi_compile_fwdbase_fullshadows
            #pragma exclude_renderers xbox360 ps3 flash d3d11_9x 
            #pragma target 3.0
            uniform float4 _LightColor0;
            uniform float4 _Color_Top;
            uniform float4 _Color_Mid;
            uniform float4 _Color_Bot;
            uniform sampler2D _Blend_Top; uniform float4 _Blend_Top_ST;
            uniform float4 _Color_BG;
            uniform sampler2D _Blend_Mid; uniform float4 _Blend_Mid_ST;
            uniform sampler2D _Blend_Bot; uniform float4 _Blend_Bot_ST;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 texcoord0 : TEXCOORD0;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float4 posWorld : TEXCOORD1;
                float3 normalDir : TEXCOORD2;
                LIGHTING_COORDS(3,4)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o;
                o.uv0 = v.texcoord0;
                o.normalDir = mul(float4(v.normal,0), _World2Object).xyz;
                o.posWorld = mul(_Object2World, v.vertex);
                o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
                TRANSFER_VERTEX_TO_FRAGMENT(o)
                return o;
            }
            fixed4 frag(VertexOutput i) : COLOR {
                i.normalDir = normalize(i.normalDir);
/////// Normals:
                float3 normalDirection =  i.normalDir;
                float3 lightDirection = normalize(_WorldSpaceLightPos0.xyz);
////// Lighting:
                float attenuation = LIGHT_ATTENUATION(i);
                float3 attenColor = attenuation * _LightColor0.xyz;
/////// Diffuse:
                float NdotL = dot( normalDirection, lightDirection );
                float3 diffuse = max( 0.0, NdotL) * attenColor + UNITY_LIGHTMODEL_AMBIENT.rgb;
                float3 finalColor = 0;
                float3 diffuseLight = diffuse;
                float2 node_59 = i.uv0;
                float4 node_11 = tex2D(_Blend_Top,TRANSFORM_TEX(node_59.rg, _Blend_Top));
                float4 node_13 = tex2D(_Blend_Mid,TRANSFORM_TEX(node_59.rg, _Blend_Mid));
                float4 node_14 = tex2D(_Blend_Bot,TRANSFORM_TEX(node_59.rg, _Blend_Bot));
                finalColor += diffuseLight * (_Color_BG.rgb+(_Color_Top.rgb*(node_11.rgb*node_11.a))+((node_13.rgb*node_13.a)*_Color_Mid.rgb)+(_Color_Bot.rgb*(node_14.rgb*node_14.a)));
/// Final Color:
                return fixed4(finalColor,1);
            }
            ENDCG
        }
        Pass {
            Name "ForwardAdd"
            Tags {
                "LightMode"="ForwardAdd"
            }
            Blend One One
            
            
            Fog { Color (0,0,0,0) }
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDADD
            #include "UnityCG.cginc"
            #include "AutoLight.cginc"
            #pragma multi_compile_fwdadd_fullshadows
            #pragma exclude_renderers xbox360 ps3 flash d3d11_9x 
            #pragma target 3.0
            uniform float4 _LightColor0;
            uniform float4 _Color_Top;
            uniform float4 _Color_Mid;
            uniform float4 _Color_Bot;
            uniform sampler2D _Blend_Top; uniform float4 _Blend_Top_ST;
            uniform float4 _Color_BG;
            uniform sampler2D _Blend_Mid; uniform float4 _Blend_Mid_ST;
            uniform sampler2D _Blend_Bot; uniform float4 _Blend_Bot_ST;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 texcoord0 : TEXCOORD0;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float4 posWorld : TEXCOORD1;
                float3 normalDir : TEXCOORD2;
                LIGHTING_COORDS(3,4)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o;
                o.uv0 = v.texcoord0;
                o.normalDir = mul(float4(v.normal,0), _World2Object).xyz;
                o.posWorld = mul(_Object2World, v.vertex);
                o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
                TRANSFER_VERTEX_TO_FRAGMENT(o)
                return o;
            }
            fixed4 frag(VertexOutput i) : COLOR {
                i.normalDir = normalize(i.normalDir);
/////// Normals:
                float3 normalDirection =  i.normalDir;
                float3 lightDirection = normalize(lerp(_WorldSpaceLightPos0.xyz, _WorldSpaceLightPos0.xyz - i.posWorld.xyz,_WorldSpaceLightPos0.w));
////// Lighting:
                float attenuation = LIGHT_ATTENUATION(i);
                float3 attenColor = attenuation * _LightColor0.xyz;
/////// Diffuse:
                float NdotL = dot( normalDirection, lightDirection );
                float3 diffuse = max( 0.0, NdotL) * attenColor;
                float3 finalColor = 0;
                float3 diffuseLight = diffuse;
                float2 node_60 = i.uv0;
                float4 node_11 = tex2D(_Blend_Top,TRANSFORM_TEX(node_60.rg, _Blend_Top));
                float4 node_13 = tex2D(_Blend_Mid,TRANSFORM_TEX(node_60.rg, _Blend_Mid));
                float4 node_14 = tex2D(_Blend_Bot,TRANSFORM_TEX(node_60.rg, _Blend_Bot));
                finalColor += diffuseLight * (_Color_BG.rgb+(_Color_Top.rgb*(node_11.rgb*node_11.a))+((node_13.rgb*node_13.a)*_Color_Mid.rgb)+(_Color_Bot.rgb*(node_14.rgb*node_14.a)));
/// Final Color:
                return fixed4(finalColor * 1,0);
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
