#!/bin/bash

# Input: Prefix (default is "v") and number of top versions to display (default is all if empty)
prefix=${1:-v}
num_top=${2}

# Fetch all tags
git fetch --tags


if [[ -z "$num_top" ]]; then
    echo "num_top is empty... showing all versions"
    output_all_tags=true
elif [[ "$num_top" =~ ^[0-9]+$ ]]; then
    echo "num_top is a valid number: $num_top"
    output_all_tags=false
else
    echo -e "\033[38;5;226mInvalid num_top value (${num_top})... showing all versions\033[0m"
    output_all_tags=true
fi

# Get all tags with the prefix, sort them by version
all_version_tags=$(git tag | grep -E "^$prefix[0-9]+\.[0-9]+\.[0-9]+$" | sort -rV)

# If no tags are found
if [[ -z "$all_version_tags" ]]; then
    echo -e "\033[38;5;226mNo tags found with prefix '$prefix'\033[0m"
    exit 0
fi

# Determine the tags to display
if [[ "$output_all_tags" = "true" ]]; then
    result_version_tags="$all_version_tags"
else
    tag_count=$(echo "$all_version_tags" | wc -l)
    output_tags_num=$((tag_count < num_top ? tag_count : num_top))
    result_version_tags=$(echo "$all_version_tags" | head -n "$output_tags_num")
fi

# Output all matching tags in the specified order with commit hashes
echo "$result_version_tags" | while read -r tag; do
    commit_hash=$(git rev-list -n 1 "$tag")
    printf "\033[38;5;245m> %s (%s)\033[0m\n" "$tag" "$commit_hash"
done
