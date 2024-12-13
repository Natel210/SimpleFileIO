#!/bin/bash

# Input: Prefix (default is "v")
prefix=${1:-v}

echo "::group::All Tags"
git tag | grep "^v" | sort -V | sed 's/^/> /'
echo "::endgroup::"

# Save tags as a single line for environment variable
tags=$(git tag | grep "^v" | sort -V | tr '\n' ' ')
echo "ALL_TAGS=$tags" >> $GITHUB_ENV
echo "::set-output name=all_tags::$tags"
