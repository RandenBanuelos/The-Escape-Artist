#define DynamicRadialMasks_TORUS_8_ADVANCED_ADDITIVE_ID1_LOCAL_LOOP_COUNT 8


float4 DynamicRadialMasks_Torus_8_Advanced_Additive_ID1_Local_Position[DynamicRadialMasks_TORUS_8_ADVANCED_ADDITIVE_ID1_LOCAL_LOOP_COUNT];	
float  DynamicRadialMasks_Torus_8_Advanced_Additive_ID1_Local_Radius[DynamicRadialMasks_TORUS_8_ADVANCED_ADDITIVE_ID1_LOCAL_LOOP_COUNT];
float  DynamicRadialMasks_Torus_8_Advanced_Additive_ID1_Local_Intensity[DynamicRadialMasks_TORUS_8_ADVANCED_ADDITIVE_ID1_LOCAL_LOOP_COUNT];
float  DynamicRadialMasks_Torus_8_Advanced_Additive_ID1_Local_NoiseStrength[DynamicRadialMasks_TORUS_8_ADVANCED_ADDITIVE_ID1_LOCAL_LOOP_COUNT];
float  DynamicRadialMasks_Torus_8_Advanced_Additive_ID1_Local_EdgeSize[DynamicRadialMasks_TORUS_8_ADVANCED_ADDITIVE_ID1_LOCAL_LOOP_COUNT];
float  DynamicRadialMasks_Torus_8_Advanced_Additive_ID1_Local_Smooth[DynamicRadialMasks_TORUS_8_ADVANCED_ADDITIVE_ID1_LOCAL_LOOP_COUNT];


#include "../../Core/Core.cginc"



////////////////////////////////////////////////////////////////////////////////
//                                                                            //
//                                Main Method                                 //
//                                                                            //
////////////////////////////////////////////////////////////////////////////////
float DynamicRadialMasks_Torus_8_Advanced_Additive_ID1_Local(float3 positionWS, float noise)
{
    float retValue = 0; 

	int i = 0;
	for(i = 0; i < DynamicRadialMasks_TORUS_8_ADVANCED_ADDITIVE_ID1_LOCAL_LOOP_COUNT; i++)
	{
		retValue += ShaderExtensions_DynamicRadialMasks_Torus_Advanced(positionWS,
																	noise,
																	DynamicRadialMasks_Torus_8_Advanced_Additive_ID1_Local_Position[i].xyz, 
																	DynamicRadialMasks_Torus_8_Advanced_Additive_ID1_Local_Radius[i],         
																	DynamicRadialMasks_Torus_8_Advanced_Additive_ID1_Local_Intensity[i],
																	DynamicRadialMasks_Torus_8_Advanced_Additive_ID1_Local_NoiseStrength[i],  
																	DynamicRadialMasks_Torus_8_Advanced_Additive_ID1_Local_EdgeSize[i],		
																	DynamicRadialMasks_Torus_8_Advanced_Additive_ID1_Local_Smooth[i]);
	}		

    return retValue;
}

////////////////////////////////////////////////////////////////////////////////
//                                                                            //
//                               Helper Methods                               //
//                                                                            //
////////////////////////////////////////////////////////////////////////////////
void DynamicRadialMasks_Torus_8_Advanced_Additive_ID1_Local_float(float3 positionWS, float noise, out float retValue)
{
    retValue = DynamicRadialMasks_Torus_8_Advanced_Additive_ID1_Local(positionWS, noise); 		
}

void DynamicRadialMasks_Torus_8_Advanced_Additive_ID1_Local_half(half3 positionWS, half noise, out half retValue)
{
    retValue = DynamicRadialMasks_Torus_8_Advanced_Additive_ID1_Local(positionWS, noise); 		
}
