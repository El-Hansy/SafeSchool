#!/usr/bin/env bash
set -euo pipefail

flutter build apk --release \
  --build-name="${SAFE_SCHOOL_VERSION_NAME:-12.0.0}" \
  --build-number="${SAFE_SCHOOL_VERSION_CODE:-1200}"
