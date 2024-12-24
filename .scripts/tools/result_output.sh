#!/bin/bash

header_name="$1"
result_file="$2"

source ./.scripts/tools/colors.sh

# Ensure header is provided
if [ -z "$header_name" ]; then
  echo -e "${background_dark_red}${text_red}Error: Header name is required.${reset}"
  exit 1
fi

# Print the header
echo -e "${text_light_gray}┌──────────────────────────────────────────────────────────────────────────────┐${reset}"
printf "${text_light_gray}|  %-74s${text_light_gray}│${reset}\n" "${text_white}${header_name}"
echo -e "${text_light_gray}└──────────────────────────────────────────────────────────────────────────────┘${reset}"

# Check if file exists and display its content
if [ -f "$result_file" ]; then
  cat "$result_file"
else
  echo -e "${background_dark_red}${text_red}File not found: $result_file${reset}"
fi