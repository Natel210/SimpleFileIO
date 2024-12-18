#!/bin/bash

if [ "$#" -lt 1 ]; then
  echo "::error::No arguments. \n Usage: $0 '<JSON String>'"
  exit 1
fi

JSON_FILE="$1"
RESULT_FILE="$2"

if [ -z "$RESULT_FILE" ]; then
  echo "$JSON_DATA" | jq -r 'to_entries[] | "\(.key) : \(.value)"'
else
  touch $RESULT_FILE
  echo "$JSON_DATA" | jq -r 'to_entries[] | "\(.key) : \(.value)"' > "$RESULT_FILE"
  cat "$RESULT_FILE"
fi