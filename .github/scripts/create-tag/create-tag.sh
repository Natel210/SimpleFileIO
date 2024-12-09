#!/bin/bash
echo "Fetching the latest tag..."

# Fetch remote tags to ensure we have the latest state
git fetch --tags 2>/dev/null

# Get the latest tag or default to v0.0.0
latest_tag=$(git describe --tags $(git rev-list --tags --max-count=1) 2>/dev/null || echo "v0.0.0")
echo "Latest tag: $latest_tag"

# Extract version and increment
if [[ "$latest_tag" =~ ^v([0-9]+)\.([0-9]+)\.([0-9]+)$ ]]; then
  major=${BASH_REMATCH[1]}
  minor=${BASH_REMATCH[2]}
  patch=${BASH_REMATCH[3]}
  # Increment the patch version
  new_patch=$((patch + 1))
  new_tag="v$major.$minor.$new_patch"
else
  # Default to v0.0.1 if no valid tag exists
  new_tag="v0.0.1"
fi

echo "New tag: $new_tag"

# Create and push the new tag, suppressing success output
if git tag "$new_tag" 2>/dev/null; then
  if git push origin "$new_tag" 2>/dev/null; then
    echo "New tag $new_tag created and pushed successfully."
  else
    echo "Error: Failed to push the new tag $new_tag to the remote repository." >&2
    exit 1
  fi
else
  echo "Error: Failed to create the new tag $new_tag locally." >&2
  exit 1
fi