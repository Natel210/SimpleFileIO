#!/bin/bash

if [ "$#" -lt 1 ]; then
  echo "\033[38;5;196mNo arguments. \n\033[38;5;196mUsage: $0 '<JSON String>'\033[0m"
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
  echo -e "\033[38;5;245m$key : $value\033[0m" >> "$temp_file"
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