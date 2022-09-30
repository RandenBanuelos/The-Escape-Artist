Known Issues (in no particular order):
--Despite high frame counts, the game runs surprisingly hot, taking up far more CPU & GPU usage than 
  is neccessary and skyrocketing internal temperatures
----Possible Causes:
------Using Unity's built-in Profiler, rendering is taking up ~85% of CPU / GPU performance
------All lighting is currently done in realtime; disabling all lights seems to cut GPU use
      by about 30-40%, yet the temperature spike still remains
------Some models, especially the theme park attractions, are fairly high-poly; disabling them
      doesn't seem to do much besides a minute increase in frames
------Some textures, similarly, are quite high-resolution, yet downscaling them also results in
      minimal improvement

--The button mechanic of the first area gets a bit boring after a while when it just moves walls

--There is a small "hiccup" when moving from the shack doorway into the house's entry hallway
----The illusion is created by teleporting the player from the shack to the house upon entering,
    potentially causing the frame lag
------The shack environment, and the teleport portals, are also disabled upon teleporting to block
      backtracking and preserve performance


Planned Updates (in no particular order):
--Add SFX and ambient sounds / music
--Set dress the house
--Try to incorporate other button functionalities, such as opening alcoves, flipping open paintings, etc.
  (i.e. implement more of the dynamic room changes like how Dishonored 2's Clockwork Mansion does for its
  transformations)
