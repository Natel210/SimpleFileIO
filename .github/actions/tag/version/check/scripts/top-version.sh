#!/bin/bash

# Input: Prefix (default is "v")
prefix=${1:-v}

git fetch --tags

# Find the highest tag
highest_version=$(git tag | grep "^$prefix" | grep -E "^$prefix[0-9]+\.[0-9]+\.[0-9]+$" | sort -V | tail -n 1)

if [[ -z "$highest_version" ]]; then
  echo "::warning::No valid tags found. Defaulting to ${prefix}0.0.1"
  highest_version="${prefix}0.0.1"
  exit 0
fi

echo -e "Highest Version : \033[34m${highest_version}\033[0m"