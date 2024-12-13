#!/bin/bash

echo -e "\033[36m● Env Check ----------------------------\033[0m"

# Load the JSON config
CONFIG="$1"

# Start GitHub Actions log group
echo "::group::Env Values"

# Process JSON and format output
echo "$CONFIG" | jq -r '. | to_entries[] | "> [\(.key)] : \(.value)"'

# End GitHub Actions log group
echo "::endgroup::"
echo -e "\033[36m----------------------------------------\033[0m"