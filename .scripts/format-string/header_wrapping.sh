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
  echo -e "\033[38;5;196mError: Header length must be a positive integer. The header_length is set to the default value of 80.\033[0m"
  header_length=80
fi

# Remove ANSI color codes from header name (if any)
plain_header_name=$(echo -e "$header_name" | sed -E 's/\x1b\[[0-9;]*m//g')
header_name_length=${#plain_header_name}

# Ensure header length is greater than the header name length
if (( header_length < header_name_length + 2 )); then
  echo -e "\033[38;5;196mError: Header length is too short for the header name.\033[0m"
  exit 1
fi

# Generate dynamic header
top_bottom_line_bar_count=$((header_length - 2))
top_bottom_line=$(printf '─%.0s' $(seq 1 "$top_bottom_line_bar_count"))
middle_line_content_count=$((header_length - 2))

# Print the header
echo -e "\033[38;5;245m┌${top_bottom_line}┐\033[0m"
printf "\033[38;5;245m│\033[0m%-${middle_line_content_count}s\033[38;5;245m│\033[0m\n" "${plain_header_name}"
echo -e "\033[38;5;245m└${top_bottom_line}┘\033[0m"

# Check if file exists and display its content
if [ -f "$result_file" ]; then
  while IFS= read -r line; do
  echo -e "$line"
done < "$result_file"
else
  echo -e "\033[38;5;196mFile not found: $result_file\033[0m"
fi
