#!/bin/bash

# Input: Prefix (default is "v")
prefix=${1:-v}

git fetch --tags

echo "::group::All Versions"
# List all tags, format them, and combine into a single line
all_versions=$(git tag | grep "^$prefix" | sort -rV | sed 's/^/> /')
echo "$all_versions"
echo "::endgroup::"
