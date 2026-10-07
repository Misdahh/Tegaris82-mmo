# TEGARIS82 — Unity MMO Production Foundation

This is a real Unity MMO project foundation, not a screenshot or a procedural character demo.

## Target
- Unity 6 LTS
- Android + PC
- Authoritative dedicated server architecture
- Third-person humanoid character pipeline
- Combat, inventory/equipment, quests, persistence interfaces
- TCP session transport foundation with server-side authority

## Important
The project deliberately does NOT pretend that a procedural mannequin is a finished film-quality human. Put your final rigged humanoid `.fbx`/`.glb` into `Assets/KaisarMMO/Art/Characters/` and connect it to `PlayerAvatar`. The gameplay/network code does not depend on the visual mesh.

## Run
1. Open the root folder in Unity 6 LTS.
2. Open `Assets/KaisarMMO/Scenes/KaisarWorld.unity`.
3. Press Play. The bootstrap creates the playable client shell.
4. Build/run the dedicated server separately from `Server/KaisarMMOServer` with a .NET runtime.

## MMO architecture
Client -> Login/session -> Authoritative server -> World state -> persistence.
Clients never authoritatively decide damage, item ownership, quest rewards, or player position validation.

## Next production asset pass
- photoreal/film-quality male and female humanoids
- facial rig and hair
- robe/armor modular clothing
- motion-capture animation set
- palace/city/world environment
- VFX/audio
- account service and database
- sharding/channel architecture
