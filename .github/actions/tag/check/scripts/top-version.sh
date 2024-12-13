#!/bin/bash

# Input: Prefix (default is "v")
prefix=${1:-v}

echo "::group::Highest Tag"
echo "Finding the highest Git tag with prefix '$prefix'..."

# Find the highest tag
highest_tag=$(git tag | grep "^$prefix" | grep -E "^$prefix[0-9]+\.[0-9]+\.[0-9]+$" | sort -V | tail -n 1)

if [[ -z "$highest_tag" ]]; then
  echo "No valid tags found. Defaulting to v0.0.0"
  highest_tag="v0.0.0"
fi

echo "Highest Tag: $highest_tag"
echo "::endgroup::"

# Output the highest tag
echo "$highest_tag"
