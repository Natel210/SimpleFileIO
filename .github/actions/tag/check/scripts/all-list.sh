#!/bin/bash

# Input: Prefix (default is "v")
prefix=${1:-v}

echo "::group::All Tags"
# List all tags, format them, and combine into a single line
all_tags=$(git tag | grep "^$prefix" | sort -V | sed 's/^/> /' | tr '\n' ' ')
echo "$all_tags"
echo "::endgroup::"
