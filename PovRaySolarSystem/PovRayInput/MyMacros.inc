#macro Rahmen_Garten(Bottom,Left,GlassDepth,RH_EG)

  #local Width  =0.05;
  #local Outside=0.01;
  #local Inside =0.01;
  #local DD     =0.001;
  
  box {<Bottom+DD,Left,0>,<Bottom-GlassDepth-DD,Left-Width,RH_EG> texture{pigment{ color rgb <0,0,0>/255}}}       // im Glas
  
  box {<Bottom+Outside,Left,0>,<Bottom,Left-Width,RH_EG> texture{pigment{ color rgb <054,061,067>/255}}}         // aussen
  
  box {<Bottom-GlassDepth,Left,0>,<Bottom-GlassDepth-Inside,Left-Width,RH_EG> texture{pigment{ color rgb <054,061,067>/255}}}         // innen
  
#end



#macro Rahmen_Hang(Bottom,Left,GlassDepth,RH_EG)

  #local Width  =0.05;
  #local Outside=0.01;
  #local Inside =0.01;
  #local DD     =0.001;
  
  box {<Bottom,Left-DD,0>,<Bottom-Width,Left-GlassDepth-DD,RH_EG> texture{pigment{ color rgb <0,0,0>/255}}}       // im Glas
  
  box {<Bottom,Left-Inside,0>,<Bottom-Width,Left,RH_EG> texture{pigment{ color rgb <054,061,067>/255}}}         // aussen
  
  box {<Bottom,Left+GlassDepth,0>,<Bottom-Width,Left+GlassDepth+Outside,RH_EG> texture{pigment{ color rgb <054,061,067>/255}}}         // innen
  
#end
