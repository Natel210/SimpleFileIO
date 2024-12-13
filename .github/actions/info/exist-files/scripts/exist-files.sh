#!/bin/bash

# Check if at least one argument is provided
if [ "$#" -lt 1 ]; then
  echo -e  "⤷ \033[31mError: No arguments provided.\033[0m"
  echo "  Usage: $0 <file1> [file2] [file3] ..."
  exit 1
fi

# Initialize error flag
error_flag=0

echo -e "::group:: -- Check Files List"
# Iterate through all provided arguments
for file in "$@"; do
  if [ -f "$file" ]; then
    echo "> File found: $file"
  else
    echo -e "\033[31mError: File not found - $file\033[0m"
    error_flag=1
  fi
done
echo "::endgroup::"

# Exit with error if any file is missing
if [ $error_flag -eq 1 ]; then
  echo -e "⤷ \033[31mOne or more files are missing.\033[0m"
  exit 1
else
  echo -e "⤷ \033[32mAll files are present.\033[0m"
fi
