// RAL-Farben in POVRAY:
// http://www.hoeberth.de/sonstiges/POV/RAL-Farbsystem.htm

#declare Texture_Dach =
      texture { pigment{ color Scarlet*1.3}
                normal { gradient z scallop_wave scale<1,1,0.15>} 
                finish { phong 1}
              } 
      texture { pigment{ gradient x 
                         color_map{[0.00 color Clear]
                                   [0.90 color Clear]
                                   [0.95 color White*0.1]
                                   [1.00 color White*0.1]}
                          scale 0.25}
                 finish { phong 1}
              }
              
#declare Wall_Texture = 
      texture { pigment{ color rgb <0.5,0.5,0.5>}
                 normal { bumps 0.1 scale 0.005} 
                 finish { phong 0.2}
              }
              
#declare Texture_Innen = 
      texture { pigment{ color rgb <1,1,1>}
                 normal { bumps 0.1 scale 0.005} 
                 finish { phong 0.5}
                 
              }  
              
#declare Decke_Texture = 
      texture { pigment{ color rgb <1,1,1>}
              }
                 
#declare Texture_Glas = 
material {texture{ pigment{ rgbf <0.98, 0.98, 0.98, 0.95> }
                   finish { diffuse 0.1 reflection 0.1  
                            specular 0.8 roughness 0.0003 phong 1 phong_size 400}
                 } 
         interior{ior 1.45   caustics 0.85}  
         }
         


// ================================================================================
// Himmel und Erde
// ================================================================================l
// sky_sphere 
// {
//     pigment {
//         gradient z
//         color_map {
//             [0.0 rgb <0.6,0.7,1.0>]
//             [0.7 rgb <0.0,0.1,0.8>]
//         }
//     }
// }

// ========================================
// Ebene
// plane {
//     z, 0
//     texture{pigment{ color rgb<0.35,0.65,0.0>*0.9 }
// 	         normal { bumps 0.75 scale 0.015 }
//                  finish { phong 0.1 }
//                }
// }