# Character art pipeline

The gameplay layer is intentionally independent from the final human mesh.

## Required final asset
- FBX or GLB/GLTF humanoid
- real skeleton and skin weights
- T-pose/A-pose
- separate material slots for skin, hair, robe, armor
- animation clips: idle, walk, run, jump, fall, attack, hit, death
- optional facial blendshapes

Import in Unity with Animation Type = Humanoid. Put the visual prefab under a `PlayerAvatar` child of the network player.

Do not replace the authoritative PlayerController/Combat/Inventory systems when replacing art.
