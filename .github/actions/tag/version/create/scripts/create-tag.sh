#!/bin/bash

# Input: Prefix (default is "v")
prefix=${1:-v}

# Fetch remote tags to ensure we have the latest state
echo "Fetching the latest tags..."
git fetch --tags

# Find the highest tag
highest_tag=$(git tag | grep "^$prefix" | grep -E "^$prefix[0-9]+\.[0-9]+\.[0-9]+$" | sort -V | tail -n 1 || echo "${prefix}0.0.0")
# echo "Highest Tag Found: $highest_tag"

# Calculate new tag
if [[ "$highest_tag" =~ ^${prefix}([0-9]+)\.([0-9]+)\.([0-9]+)$ ]]; then
  major=${BASH_REMATCH[1]}
  minor=${BASH_REMATCH[2]}
  patch=${BASH_REMATCH[3]}
  new_patch=$((patch + 1))
  new_tag="${prefix}$major.$minor.$new_patch"
else
  new_tag="${prefix}0.0.1"
fi

echo "New Tag: $new_tag"

# Create and push new tag
if git tag "$new_tag"; then
  if git push origin "$new_tag"; then
    echo "New tag $new_tag created and pushed successfully."
  else
    echo "Error: Failed to push the new tag $new_tag to the remote repository." >&2
    exit 1
  fi
else
  echo "Error: Failed to create the new tag $new_tag locally." >&2
  exit 1
fi

if [[ -n "$GITHUB_ENV" ]]; then
  echo "NEW_TAG=$new_tag" >> "$GITHUB_ENV"
fi

if [[ -n "$GITHUB_STATE" ]]; then
  echo "new_tag=$new_tag" >> "$GITHUB_STATE"
fi

# Output the new tag (ensure only the tag itself is printed)
echo "$new_tag"
