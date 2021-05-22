// Grundgerüst
#include "colors.inc"  
#include "shapes.inc"
#include "woods.inc" 
#include "textures.inc"

#include "MyTextures.inc"    
#include "MyCameras.inc"
#include "MyMacros.inc"

#include "Moon.inc"
           
// http://www.f-lohmueller.de/pov_tut/animate/anim112d.htm

global_settings 
{
    assumed_gamma 1.0

}  
                
#declare Moon_Radius = 1.161781241850256E-8; 
#declare Earth_Radius = 4.263429666582815E-8;
#declare Sun_Radius = 0.00465047;

#declare Moon  =
sphere{ <0,0,0>,Moon_Radius
        texture{Wall_Texture}}
#declare Earth =
sphere{ <0,0,0>,Earth_Radius
        texture{Wall_Texture}}
#declare Sun   =
light_source{ <0,0,0>
              color rgb<0,0,0>
 looks_like{
 sphere{ <0,0,0>,Sun_Radius
         texture{
                 pigment{ color Red }
                 finish{ ambient 1
                          diffuse 0}
                } // end of texture
       } // end of sphere
   } // end of looks_like
 } // end of light_source
//--------------------------------

 
// Kamera     
#declare GR_Debug=0;
camera {Camera_0}
  
// ================================================================================
// Sonne
// ================================================================================ 

#declare time         = clock;                         // Hier clock einsetzen für Animation

light_source 
{   
    
    <0,0,0>
    color rgb <1,1,1>*1.2
}    

light_source 
{   
    
    <Moon_X[time],Moon_Y[time],Moon_Z[time]>
    color rgb <1,1,1>*1.2
}