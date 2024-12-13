#!/bin/bash

# Input: Prefix (default is "v")
prefix=${1:-v}

# Fetch remote tags to ensure we have the latest state
echo "Fetching the latest tags..."
git fetch --tags

# Find the highest tag
highest_version=$(git tag | grep "^$prefix" | grep -E "^$prefix[0-9]+\.[0-9]+\.[0-9]+$" | sort -V | tail -n 1 || echo "${prefix}0.0.0")
# echo "Highest Tag Found: $highest_version"

# Calculate new tag
if [[ "$highest_version" =~ ^${prefix}([0-9]+)\.([0-9]+)\.([0-9]+)$ ]]; then
  major=${BASH_REMATCH[1]}
  minor=${BASH_REMATCH[2]}
  patch=${BASH_REMATCH[3]}
  new_patch=$((patch + 1))
  new_version="${prefix}$major.$minor.$new_patch"
else
  new_version="${prefix}0.0.1"
fi

echo "try update version [${highest_version}] -> [${new_version}]"

# Create and push new tag
if git tag "$new_version" > /dev/null; then
  if git push origin "$new_version" > /dev/null; then
    echo "::notice::new version[${new_version}]"
  else
    echo "::error::push origin fail [${new_version}]"
    exit 1
  fi
else
  echo "::error::create tag fail [${new_version}]"
  exit 1
fi

if [[ -n "$GITHUB_ENV" ]]; then
  echo "NEW_TAG=$new_version" >> "$GITHUB_ENV"
fi

if [[ -n "$GITHUB_STATE" ]]; then
  echo "new_tag=$new_version" >> "$GITHUB_STATE"
fi
