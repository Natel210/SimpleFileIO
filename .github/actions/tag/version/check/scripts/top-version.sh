#!/bin/bash

# Input: Prefix (default is "v")
prefix=${1:-v}

# Find the highest tag
highest_tag=$(git tag | grep "^$prefix" | grep -E "^$prefix[0-9]+\.[0-9]+\.[0-9]+$" | sort -V | tail -n 1)

if [[ -z "$highest_tag" ]]; then
  echo "No valid tags found. Defaulting to ${prefix}0.0.0"
  highest_tag="${prefix}0.0.0"
fi

echo -e "Highest Tag : \033[34m${highest_tag}\033[0m"