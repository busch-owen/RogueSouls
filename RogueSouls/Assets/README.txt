README

THIS IS NECESSARY WHEN CREATING ANY NEW SCENES IN ORDER FOR THE GAME TO FUNCTION PROPERLY

MANUAL APPROACH

THINGS TO ADD TO YOUR SCENE

- ADD EVERYTHING FROM THE LEVEL FOLDER IN PREFABS
- ADD THE HANDLERS PREFAB AND SET UP EACH HANDLER ACCORDINGLY
- ADD A PLAYER PREFAB - NO SET UP NECESSARY (I RECOMMEND HIDING *NOT DISABLING* THE WARP OVERLAY OBJECT)
- ADD A CAMERA PREFAB TO THE SCENE - NO SET UP NECESSARY
- ADD A CROSSHAIR PREFAB TO THE SCENE - NO SET UP NECESSARY
- ADD A GLOBAL VOLUME OBJECT (CREATE/VOLUME/GLOBAL VOLUME) AND THEN APPLY POST PROCESS VOLUME PROFILE TO THE VOLUME COMPONENT
- ADD A DEATH SCREEN CANVAS

PRESET APPROACH

- DUPLICATE THE "DEFAULT_LEVEL" SCENE AND RENAME TO MATCH WHATEVER LEVEL YOU ARE MAKING

FAQ

Q: "Why is my entire screen black in the editor?"
A: This is the warp overlay, simply just hide it from the heirarchy

Q: "Why don't my enemies chase me?"
A: There's two possible solutions to this problem: 
	S1: Your enemies have no speed value, please assign a speed on the entity stats script. 
	S2: You haven't baked your Nav Mesh. To bake your nav mesh, click on the nav mesh in the level folder in your scene, then click "Bake"