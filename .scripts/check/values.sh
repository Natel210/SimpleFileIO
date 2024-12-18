#!/bin/bash

# Check if jq is installed
if ! command -v jq &> /dev/null; then
  echo "Error: jq is not installed. Please install jq and try again."
  exit 1
fi

# Load the JSON config from file or string
if [ -f "$1" ]; then
  CONFIG=$(cat "$1")
else
  CONFIG="$1"
fi

RESULT_FILE="${2:-values.result}"

SINGLE_LINE_JSON=$(echo "$CONFIG" | jq -c .)

# Validate JSON
if ! echo "$SINGLE_LINE_JSON" | jq empty; then
  echo "Error: Invalid JSON format"
  exit 1
fi

# Process JSON and format output
echo "$SINGLE_LINE_JSON" | jq -r '. | to_entries[] | "> [\(.key)] : \(.value)"' > $RESULT_FILE
