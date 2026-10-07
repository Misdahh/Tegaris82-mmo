# Kaisar MMO Dedicated Server

The server is authoritative. It owns player IDs, position validation, HP and combat decisions.

This folder contains the server protocol foundation. Run it as a separate dedicated process in production.

For deployment, use .NET 8/9 and place the executable behind a firewall/load balancer. Add PostgreSQL/Redis through the repository interface before launch.
