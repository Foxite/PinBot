# PinBot
Allows your users to pin messages without the need to grant them sensitive permissions such as Manage Messages.

## Docker deployment
See Dockerfile in PinBot folder. Environment variables:

- BOT_TOKEN (required)
- ROLE_ID (optional, users must have this role to use PinBot)
- IGNORE_CHANNELS (optional, IDs of channels that get ignored)
