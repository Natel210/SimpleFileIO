#!/bin/bash

VERSION=$1
COMMIT_HASH=$2
DLL_FILE=$3
TESTER_FILE=$4

if [ -f "$DLL_FILE" ]; then
  sed -i "s|<Version>.*</Version>|<Version>$VERSION</Version>|g" "$DLL_FILE"
  sed -i "s|<CommitHash>.*</CommitHash>|<CommitHash>$COMMIT_HASH</CommitHash>|g" "$DLL_FILE"
fi

if [ -n "$TESTER_FILE" ] && [ -f "$TESTER_FILE" ]; then
  sed -i "s|<Version>.*</Version>|<Version>$VERSION</Version>|g" "$TESTER_FILE"
  sed -i "s|<CommitHash>.*</CommitHash>|<CommitHash>$COMMIT_HASH</CommitHash>|g" "$TESTER_FILE"
fi

echo "Project version and commit hash updated successfully."
