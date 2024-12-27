#!/bin/bash

json_data="$1"
result_file="$2"

if [ -z "$json_data" ]; then
  echo "\033[38;5;196mNo JSON data provided.\n\033[38;5;196mUsage: $0 '<JSON String>' [result_file]\033[0m"
  exit 1
fi

total_count=0
missing_count=0
output=""

# Get Json Data
while read -r pair; do
  total_count=$((total_count + 1))
  key=$(echo "$pair" | jq -r '.key')
  value=$(echo "$pair" | jq -r '.value')
  if [ -f "$value" ] || [ -d "$value" ]; then
    output+="\n\033[38;5;245m● $key\n\033[38;5;245m  - $value : Exist\033[0m"
  else
    missing_count=$((missing_count + 1))
    output+="\n\033[0m● $key\n\033[0m  - $value : \033[38;5;196mNot Exist\033[0m"
  fi
done <<< $(echo $json_data | jq -c '.[]')

if [ "$missing_count" -eq 0 ]; then
  summary="\033[38;5;46mAll OK.. ($total_count/$total_count)\033[0m"
else
  summary="\033[38;5;196mNot Exist File Count $missing_count.. ($((total_count - missing_count))/$total_count)\033[0m"
fi

result="$summary$output"

if [ -z "$result_file" ]; then
  echo -e "$result"
else
  # Ensure result file directory exists
  result_dir=$(dirname "$result_file")
  if [ ! -d "$result_dir" ]; then
      mkdir -p "$result_dir"
  fi
  echo -e "$result" > "$result_file"
fi
