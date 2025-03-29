### Decision 1: Container Runtime User, Group, and Volume Permissions

**Date:** 2025-03-29  
**Applies to:** UAT (SENESCHAL), PROD (ILION)  
**Scope:** IdentityServer container (`quicksilvra-idp-uat`, `quicksilvra-idp-prod`)  
**Status:** Applied

---

### 🎯 Objective
Ensure the IdentityServer container runs securely as a non-root user, while maintaining the ability to persist and access data protection keys and certificates across container restarts, on both local and remote environments (SENESCHAL and ILION).

---

### 🧩 Problem
By default, Docker containers run as `root`. However:
- This introduces security risks.
- The container must write data protection keys to a volume (`/etc/quicksilvra/data-protection/keys`).
- It must also read a certificate file (`localhost.pfx`) from a mounted volume.
- Permissions must be consistent between container and host.

---

### ✅ Decision
Run the container with a fixed non-root user (`appuser`) and group (`appgroup`) inside the image, and ensure the host has a matching group (GID) and compatible permissions on required paths.

---

### 🔐 Linux Host Configuration

#### Group
- Created a dedicated group `appgroup` with **GID 39000** on both SENESCHAL and ILION:
  ```bash
  sudo groupadd --system --gid 39000 appgroup
  ```

#### User (host-only, optional)
- Created optional `appuser` with **UID 10000** and GID 39000 on SENESCHAL (for ownership alignment and future flexibility):
  ```bash
  sudo useradd --system --uid 10000 --gid 39000 --no-create-home --shell /usr/sbin/nologin appuser
  ```

#### Permissions
- Gave group read access to `localhost.pfx`:
  ```bash
  sudo chown root:appgroup /etc/quicksilvra/data-protection/localhost.pfx
  sudo chmod 640 /etc/quicksilvra/data-protection/localhost.pfx
  ```
- Gave group write access to `keys` directory:
  ```bash
  sudo chown root:appgroup /etc/quicksilvra/data-protection/keys
  sudo chmod 770 /etc/quicksilvra/data-protection/keys
  ```

---

### 🐳 Dockerfile Adjustments
```Dockerfile
ARG APP_UID=10000
ARG APP_GID=39000
ARG APP_USER=appuser
ARG APP_GROUP=appgroup

RUN addgroup --gid ${APP_GID} ${APP_GROUP} && \
    adduser --system --uid ${APP_UID} --ingroup ${APP_GROUP} ${APP_USER}

USER ${APP_USER}
```
- Defaults: `UID=10000`, `GID=39000`
- Can be overridden via `--build-arg` in CI/CD if needed

---

### 📦 Docker Compose Runtime
- Volumes mount host paths to the same location inside the container:
```yaml
volumes:
  - ${Quicksilvra_DataProtectionConfiguration__DataProtectionPath}:${Quicksilvra_DataProtectionConfiguration__DataProtectionPath}
  - ${Quicksilvra_DataProtectionConfiguration__X509__Path}:${Quicksilvra_DataProtectionConfiguration__X509__Path}:ro
```

---

### 🔄 Benefits
- Runs securely as non-root
- Compatible with host volume permissions
- Ready for long-term persistence of keys and secrets
- Easy to replicate across environments (SENESCHAL ↔ ILION)

---

### 📝 Future Considerations
- Automate UID/GID provisioning with Ansible or shell scripts
- Store secrets (pfx/key) in a secrets manager (Vault, AWS Secrets Manager)
- Validate permission access inside container at entrypoint for debugging


