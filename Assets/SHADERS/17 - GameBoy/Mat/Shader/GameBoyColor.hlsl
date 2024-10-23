#define H 255

void GameBoyColor_float(in float3 col, out float3 Out)
{
    if(col.r > 0.75)    
    {
        // high screen color palette
        col = half3(202./H, 220./H, 159./H);
    }
    else if (col.r < 0.75 && col.r > 0.50)   
    {
        // high-middle screen color palette
        col = half3(139./H, 172./H, 15./H); 
    }
    else if (col.r > 0.30 && col.r <= 0.50)  
    {
        // middle screen color palette
        col = half3(48./H, 98./H, 48./H);
    }
    else    
    {
        // low screen color palette
        col = half3(15./H, 56./H, 15./H);
    }  

    Out = col;
}