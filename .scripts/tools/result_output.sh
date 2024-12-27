#!/bin/bash

# Arguments
header_name="$1"
result_file="$2"

# Ensure header is provided
if [ -z "$header_name" ]; then
  echo -e "\033[38;5;196mError: Header name is required.\033[0m"
  exit 1
fi

# Print the header
echo -e "\033[38;5;245m┌──────────────────────────────────────────────────────────────────────────────┐\033[0m"
printf "\033[38;5;245m|\033[0m  %-76s\033[38;5;245m│\033[0m\n" "${header_name}"
echo -e "\033[38;5;245m└──────────────────────────────────────────────────────────────────────────────┘\033[0m"

# Check if file exists and display its content
if [ -f "$result_file" ]; then
  cat "$result_file"
else
  echo -e "\033[38;5;196mFile not found: $result_file\033[0m"
fi