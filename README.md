<p style="text-align:center">
  <img src="assets/spinningLogoFinal.gif" alt="Quicksilvra Logo" style="width:150px"/>
</p>

# Quicksilvra Overview

## 🌐 Network Endpoints

Overview of the environments used in the **Quicksilvra** infrastructure.

---

### 🧪 Development Environment

| Service                              | URL                                   |
|--------------------------------------|----------------------------------------|
| 🆔 Quicksilvra IDP (Identity Server) | [http://localhost:5001](http://localhost:5001) |
| 🎯 Quicksilvra Web BFF (Backend-for-Frontend)    | [http://localhost:5002](http://localhost:5002) |

---

### 🧷 UAT Environment

| Service                               | URL                                                                |
|----------------------------------------|--------------------------------------------------------------------|
| 🆔 Quicksilvra IDP                                  | [https://uat.idp.quicksilvra.com](https://uat.idp.quicksilvra.com) |
| 🎯 Quicksilvra Web BFF                             | [https://uat.app.quicksilvra.com](https://uat.app.quicksilvra.com) |

---

### 🚀 Production Environment

| Service                               | URL                                                        |
|----------------------------------------|------------------------------------------------------------|
| 🆔 Quicksilvra IDP                                  | [https://idp.quicksilvra.com](https://idp.quicksilvra.com) |
| 🎯 Quicksilvra Web BFF                             | [https://app.quicksilvra.com](https://app.quicksilvra.com) |

---

### ⚙️ Technical Notes

- In **UAT** and **PROD**, all services are exposed via a **reverse proxy (NGINX)** with:
    - HTTPS termination
    - HTTP downgrade to internal containers
    - Routing based on domain and/or path
- In **Development**, services are directly accessible on standard local ports (`5001`, `5002`, ... ).


#### 

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
