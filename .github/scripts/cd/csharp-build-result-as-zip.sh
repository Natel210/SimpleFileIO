#!/bin/bash

# Check if the correct number of arguments is provided
if [ "$#" -ne 2 ]; then
  echo "Error: Invalid number of arguments."
  echo "Usage: $0 <target_path> <target_name>"
  exit 1
fi

# Read paths from command-line arguments
target_path="$1"
target_name="$2"

# Ensure all arguments are provided
if [[ -z "${target-path}" || -z "${target_name}"  ]]; then
  echo "Error: Missing required arguments."
  echo "Usage: $0 <target_path> <target_name>"
  exit 1
fi

old_path="${pow}"

cd "${target_path}"
zip -r "${target_name}.zip" .

cd "${old_path}"