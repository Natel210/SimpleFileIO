#!/bin/bash

# Input: Prefix (default is "v")
prefix=${1:-v}

git fetch --tags

# Find the highest tag
latest_version=$(git tag | grep "^$prefix" | grep -E "^$prefix[0-9]+\.[0-9]+\.[0-9]+$" | sort -V | tail -n 1)

if [[ -z "$latest_version" ]]; then
  echo ${prefix}0.0.1
else
  echo ${latest_version}
fi