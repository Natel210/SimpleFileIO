#!/bin/bash

# Load the JSON config
CONFIG="$1"

# Start GitHub Actions log group
echo "::group::Env Values"

# Process JSON and format output
echo "$CONFIG" | jq -r '. | to_entries[] | "> [\(.key)] : \(.value)"'

# End GitHub Actions log group
echo "::endgroup::"