# Quicksilvra Deployment Overview

## 📦 Environments

### 🔧 Configuration (UAT and PROD)
- Env file: `/etc/quicksilvra/quicksilvra-env.sh`

### 🚀 Deployment Scripts
- Location: `/opt/deploy`

### 🔐 Certbot (TLS/SSL certificates)
- Files: `/etc/quicksilvra/certbot/`

---

## 🧠 Architectural Decisions

- [Decision 1 – Container Runtime User, Group, and Volume Permissions](./Decisions/Decision1-ContainerRuntime-User-Group-VolumePermission.md)

This decision outlines the security setup for running containers as non-root users, and explains how UID/GID mapping and volume access are managed on both UAT (SENESCHAL) and PROD (ILION).
