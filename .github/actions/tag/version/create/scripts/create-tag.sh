#!/bin/bash

# Input: Prefix (default is "v")
prefix=${1:-v}

# Fetch remote tags to ensure we have the latest state
echo "Fetching the latest tags..."
git fetch --tags

# Find the highest tag
latest_version=$(git tag | grep "^$prefix" | grep -E "^$prefix[0-9]+\.[0-9]+\.[0-9]+$" | sort -V | tail -n 1 || echo "${prefix}0.0.0")
echo -e "Latest Version : \033[34m${latest_version}\033[0m"

# Calculate new tag
if [[ "$latest_version" =~ ^${prefix}([0-9]+)\.([0-9]+)\.([0-9]+)$ ]]; then
  major=${BASH_REMATCH[1]}
  minor=${BASH_REMATCH[2]}
  patch=${BASH_REMATCH[3]}
  new_patch=$((patch + 1))
  new_version="${prefix}$major.$minor.$new_patch"
else
  new_version="${prefix}0.0.1"
fi

echo "Trying to update version [${latest_version}] -> [${new_version}]"

# Create and push new tag
check_error_code=0
if git tag "$new_version" > /dev/null; then
  if git push origin "$new_version" > /dev/null; then
    check_error_code=0
  else
    check_error_code=1
  fi
else
  check_error_code=2
fi

# Handle error codes
if [[ "$check_error_code" -eq 0 ]]; then
  echo -e "::notice::Operation successful : New version \033[34m[${new_version}]\033[0m"
elif [[ "$check_error_code" -eq 1 ]]; then
  echo "::error::Failed to push new version to origin : [${new_version}]"
  exit 1
elif [[ "$check_error_code" -eq 2 ]]; then
  echo "::error::Failed to create tag locally : [${new_version}]"
  exit 1
else
  echo "::error::Unknown error occurred during tag creation : [${new_version}]"
  exit 1
fi

# Export the new version to GitHub environment and state
if [[ -n "$GITHUB_ENV" ]]; then
  echo "NEW_VERSION=$new_version" >> "$GITHUB_ENV"
fi