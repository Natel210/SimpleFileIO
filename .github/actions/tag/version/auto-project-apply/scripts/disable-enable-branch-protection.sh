#!/bin/bash
set -e

ACTION=$1
GITHUB_USER=$2
GITHUB_REPO=$3
BRANCH_NAME=$4
GH_PAT=${GH_PAT}
BACKUP_FILE="branch_protection_backup_${BRANCH_NAME}.json"

API_URL="https://api.github.com/repos/$GITHUB_USER/$GITHUB_REPO/branches/$BRANCH_NAME/protection"

if [[ "$ACTION" == "disable" ]]; then
  echo "Backing up branch protection rules..."
  curl -s -H "Authorization: token $GH_PAT" -H "Accept: application/vnd.github.v3+json" "$API_URL" > "$BACKUP_FILE"
  echo "Disabling branch protection..."
  curl -s -X DELETE -H "Authorization: token $GH_PAT" -H "Accept: application/vnd.github.v3+json" "$API_URL"

elif [[ "$ACTION" == "enable" ]]; then
  echo "Restoring branch protection rules..."
  curl -s -X PUT -H "Authorization: token $GH_PAT" -H "Accept: application/vnd.github.v3+json" -d @"$BACKUP_FILE" "$API_URL"
else
  echo "Usage: $0 [disable|enable] <github_user> <github_repo> <branch_name>"
  exit 1
fi
