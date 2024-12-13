#!/bin/bash

# Check if at least one argument is provided
if [ "$#" -lt 1 ]; then
  echo "Error: No arguments provided."
  echo "Usage: $0 <file1> [file2] [file3] ..."
  exit 1
fi

# Initialize error flag
error_flag=0

echo "Check Files"

# Iterate through all provided arguments
for file in "$@"; do
  if [ -f "$file" ]; then
    echo "> File found: $file"
  else
    echo "Error: File not found - $file"
    error_flag=1
  fi
done

echo ""
echo "Result"

# Exit with error if any file is missing
if [ $error_flag -eq 1 ]; then
  echo "> One or more files are missing."
  exit 1
else
  echo "> All files are present."
fi
