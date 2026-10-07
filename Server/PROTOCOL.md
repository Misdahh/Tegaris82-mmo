# Network protocol

Line-delimited JSON is used for the initial development server so it is easy to inspect.

Client -> server:
- InputCommand: tick, x, z, sprint, attack, yaw
- CombatEvent: attackerId, targetId, damage, sequence (server ignores client damage value in production)

Server -> client:
- login: assigned player id
- snapshot: authoritative player transform, HP and level

Production upgrade path:
1. Replace JSON/TCP with a binary packet protocol over UDP/QUIC.
2. Add encryption/session authentication.
3. Add interest management and world cells.
4. Add PostgreSQL persistence and Redis cache.
5. Add region/channel/shard services.
