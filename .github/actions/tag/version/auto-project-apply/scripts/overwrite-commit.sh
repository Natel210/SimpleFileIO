#!/bin/bash

# Input variables
TARGET_COMMIT=$1
DLL_FILE=$2
TESTER_FILE=$3
USER_NAME=${4:-"Default User"}
USER_EMAIL=${5:-"default@example.com"}

# Validate inputs
if [ -z "$TARGET_COMMIT" ] || [ -z "$DLL_FILE" ]; then
  echo "::warning::Target commit and DLL file path are required."
  exit 0
fi

if [ ! -f "$DLL_FILE" ]; then
  echo "::warning::DLL file not found at path: $DLL_FILE"
  exit 0
fi

# Set Git user information
echo "Using Git user.name: $USER_NAME"
echo "Using Git user.email: $USER_EMAIL"
git config --global user.name "$USER_NAME"
git config --global user.email "$USER_EMAIL"

# Start interactive rebase
echo "Starting interactive rebase for commit: $TARGET_COMMIT"
if ! git rebase -i "$TARGET_COMMIT"^; then
  echo "::error::Rebase failed for commit: $TARGET_COMMIT"
  exit 1
fi

# Stage files and amend the commit
git add "$DLL_FILE"
if [ -n "$TESTER_FILE" ] && [ -f "$TESTER_FILE" ]; then
  git add "$TESTER_FILE"
fi

if ! git commit --amend --no-edit; then
  echo "::error::Failed to amend the commit"
  exit 1
fi

if ! git rebase --continue; then
  echo "::error::Failed to continue rebase"
  exit 1
fi

# Force push changes
if ! git push --force; then
  echo "::error::Force push failed"
  exit 1
fi

echo "Commit overwrite complete."
exit 0
