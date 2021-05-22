// ================================================================================
// Kameras
// 1. Koordinate Neigung (kleiner Wert fährt Kamera nach oben), 3. Koordinate Rotation
// ================================================================================

// Hubschrauber-Sicht
#declare Camera_0 = camera 
{       location  <0,0,0>
        rotate    <99,0,246>       
        translate <50,-10,10>
        angle      28
        right x*image_width/image_height
}
 
 // Grundriss-Kontrolle
#declare Camera_GR = camera 
{       orthographic
        location <5,7,20>
        look_at   <5,7,0>
        angle      70
        right x*image_width/image_height
}

// Im Wohnzimmer 
#declare Camera_WZ_1 = camera 
{       location  <0,0,0>
        rotate    <90,0,215-180>       
        translate <5,9,2>
        angle      70
        right x*image_width/image_height
}
 
 // Vor dem Wohnzimmer 
#declare Camera_WZ_2 = camera 
{       location  <0,0,0>
        rotate    <90,0,215>       
        translate <10.5,-1.5,2>
        angle      90
        right x*image_width/image_height
}