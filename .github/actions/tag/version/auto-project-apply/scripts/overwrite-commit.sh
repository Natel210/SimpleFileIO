#!/bin/bash
set -e

# Input variables
DLL_FILE=$1        # DLL file path
TESTER_FILE=$2     # Tester file path (optional)
USER_NAME=$3       # Git user name
USER_EMAIL=$4      # Git user email

# Validate inputs
if [[ -z "$DLL_FILE" || -z "$USER_NAME" || -z "$USER_EMAIL" ]]; then
  echo "Usage: $0 <dll_file> [<tester_file>] <user_name> <user_email>"
  exit 1
fi

# Set Git user information
git config --global user.name "$USER_NAME"
git config --global user.email "$USER_EMAIL"

# Add modified project files to staging
echo "Staging changes for files..."
git add "$DLL_FILE"
if [[ -n "$TESTER_FILE" && -f "$TESTER_FILE" ]]; then
  git add "$TESTER_FILE"
fi

# Amend the current commit
echo "Amending the current commit with new changes..."
git commit --amend --no-edit

# Force push to overwrite the existing commit
echo "Force pushing the updated commit to the remote branch..."
git push origin HEAD --force

echo "Commit has been successfully updated."
exit 0
