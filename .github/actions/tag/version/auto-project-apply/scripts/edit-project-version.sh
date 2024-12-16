#!/bin/bash

# Input variables
VERSION=$1
COMMIT_HASH=$2
DLL_FILE=$3
TESTER_FILE=$4

# Validate inputs
if [ -z "$VERSION" ] || [ -z "$COMMIT_HASH" ] || [ -z "$DLL_FILE" ]; then
  echo "::warning::Version, CommitHash, and DLL file path are required."
  exit 0
fi

if [ ! -f "$DLL_FILE" ]; then
  echo "::warning::DLL file not found at path: $DLL_FILE"
  exit 0
fi

# Update DLL file with version and commit hash
echo "Updating $DLL_FILE with Version=$VERSION and CommitHash=$COMMIT_HASH"
if grep -q "<Version>" "$DLL_FILE"; then
  sed -i "s|<Version>.*</Version>|<Version>$VERSION</Version>|g" "$DLL_FILE"
else
  sed -i "/<PropertyGroup>/a \    <Version>$VERSION</Version>" "$DLL_FILE"
fi

if grep -q "<CommitHash>" "$DLL_FILE"; then
  sed -i "s|<CommitHash>.*</CommitHash>|<CommitHash>$COMMIT_HASH</CommitHash>|g" "$DLL_FILE"
else
  sed -i "/<PropertyGroup>/a \    <CommitHash>$COMMIT_HASH</CommitHash>" "$DLL_FILE"
fi

# Optionally update tester file
if [ -n "$TESTER_FILE" ] && [ -f "$TESTER_FILE" ]; then
  echo "Updating $TESTER_FILE with Version=$VERSION and CommitHash=$COMMIT_HASH"
  if grep -q "<Version>" "$TESTER_FILE"; then
    sed -i "s|<Version>.*</Version>|<Version>$VERSION</Version>|g" "$TESTER_FILE"
  else
    sed -i "/<PropertyGroup>/a \    <Version>$VERSION</Version>" "$TESTER_FILE"
  fi

  if grep -q "<CommitHash>" "$TESTER_FILE"; then
    sed -i "s|<CommitHash>.*</CommitHash>|<CommitHash>$COMMIT_HASH</CommitHash>|g" "$TESTER_FILE"
  else
    sed -i "/<PropertyGroup>/a \    <CommitHash>$COMMIT_HASH</CommitHash>" "$TESTER_FILE"
  fi
fi

echo "File updates complete."
exit 0
