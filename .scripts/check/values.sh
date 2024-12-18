#!/bin/bash

# need "jq"

# Load the JSON config
CONFIG="$1"
RESULT_FILE="${2:-values.result}"
# Process JSON and format output
echo "$CONFIG" | jq -r '. | to_entries[] | "> [\(.key)] : \(.value)"' > $RESULT_FILE