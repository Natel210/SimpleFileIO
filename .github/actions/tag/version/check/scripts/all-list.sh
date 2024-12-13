#!/bin/bash

# Input: Prefix (default is "v")
prefix=${1:-v}

git fetch --tags

echo "::group::All"
# List all tags, format them, and combine into a single line
all_tags=$(git tag | grep "^$prefix" | sort -rV | sed 's/^/> /')
echo "$all_tags"
echo "::endgroup::"
