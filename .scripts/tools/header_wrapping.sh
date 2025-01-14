#!/bin/bash

# Arguments
header_name="$1"
result_file="$2"
header_length="$3"

# Default header length if not provided
if [ -z "$header_length" ]; then
  header_length=80
fi

# Validate header length
if ! [[ "$header_length" =~ ^[0-9]+$ ]]; then
  echo -e "\033[38;5;196mError: Header length must be a positive integer.\033[0m"
  exit 1
fi

# Generate dynamic header
line=$(printf '─%.0s' $(seq 1 "$header_length"))
padding=$((header_length - ${#header_name} - 2))
left_padding=$((padding / 2))
right_padding=$((padding - left_padding))

# Print the header
echo -e "\033[38;5;245m┌${line}┐\033[0m"
printf "\033[38;5;245m│\033[0m%${left_padding}s%s%${right_padding}s\033[38;5;245m│\033[0m\n" "" "${header_name}" ""
echo -e "\033[38;5;245m└${line}┘\033[0m"

# Check if file exists and display its content
if [ -f "$result_file" ]; then
  cat "$result_file"
else
  echo -e "\033[38;5;196mFile not found: $result_file\033[0m"
fi
