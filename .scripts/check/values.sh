#!/bin/bash

source ./.scripts/tools/colors.sh

if [ "$#" -lt 1 ]; then
  echo "${background_dark_red}${text_red}No arguments. \n${background_dark_red}${text_red} Usage: $0 '<JSON String>'${reset}"
  exit 1
fi

json_data="$1"
result_file="$2"

# Create Temp
temp_file=$(mktemp)

# Get Json Data
echo $json_data | jq -c '.[]' | while read -r pair; do
  key=$(echo "$pair" | jq -r '.key')
  value=$(echo "$pair" | jq -r '.value')
  echo "$key : $value" >> "$temp_file"
done

# Check Result Save File Path
if [ -z "$result_file" ]; then
  cat "$temp_file"
else
  result_dir=$(dirname "$result_file")
  if [ ! -d "$result_dir" ]; then
      mkdir -p "$result_dir"
  fi
  cp "$temp_file" "$result_file"
fi  

# Remove Temp
rm -f "$temp_file"