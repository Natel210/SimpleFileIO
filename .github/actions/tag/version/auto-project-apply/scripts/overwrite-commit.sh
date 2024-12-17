#!/bin/bash
set -e

# Input parameters
ACTION=$1         # "disable" or "enable"
GITHUB_ORDER_USER=$2    # GitHub username or organization
GITHUB_REPO=$3    # Repository name
BRANCH_NAME=$4    # Branch name
GH_PAT=${GH_PAT}  # GitHub Personal Access Token
BACKUP_FILE="branch_protection_backup_${BRANCH_NAME}.json"

# Check action input
if [[ "$ACTION" != "disable" && "$ACTION" != "enable" ]]; then
  echo "Usage: $0 [disable|enable] <github_user> <github_repo> <branch_name>"
  exit 1
fi

# API URL
API_URL="https://api.github.com/repos/$GITHUB_USER/$GITHUB_REPO/branches/$BRANCH_NAME/protection"

# Function to backup current protection rules
backup_protection() {
  echo "Backing up current branch protection rules for branch: $BRANCH_NAME..."
  curl -s -H "Authorization: token $GH_PAT" -H "Accept: application/vnd.github.v3+json" \
    "$API_URL" > "$BACKUP_FILE"

  if [[ -s "$BACKUP_FILE" ]]; then
    echo "Branch protection rules backed up to $BACKUP_FILE."
  else
    echo "::error::Failed to backup branch protection rules!"
    exit 1
  fi
}

# Function to disable branch protection
disable_protection() {
  echo "Disabling branch protection for branch: $BRANCH_NAME..."
  curl -s -X DELETE -H "Authorization: token $GH_PAT" -H "Accept: application/vnd.github.v3+json" \
    "$API_URL"
  echo "Branch protection disabled."
}

# Function to restore branch protection
restore_protection() {
  if [[ -f "$BACKUP_FILE" ]]; then
    echo "Restoring branch protection rules for branch: $BRANCH_NAME from backup..."
    curl -s -X PUT -H "Authorization: token $GH_PAT" -H "Accept: application/vnd.github.v3+json" \
      "$API_URL" -d @"$BACKUP_FILE"
    echo "Branch protection rules restored."
  else
    echo "::error::Backup file not found! Cannot restore protection."
    exit 1
  fi
}

# Main action
if [[ "$ACTION" == "disable" ]]; then
  backup_protection
  disable_protection
elif [[ "$ACTION" == "enable" ]]; then
  restore_protection
fi
