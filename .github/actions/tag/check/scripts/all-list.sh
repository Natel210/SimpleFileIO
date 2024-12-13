#!/bin/bash

# Input: Prefix (default is "v")
prefix=${1:-v}

echo "::group::All Tags"
echo "Listing all Git tags..."

# List all tags, format with `>`, and filter by prefix
git tag | grep "^$prefix" | sort -V | sed 's/^/> /'

echo "::endgroup::"
