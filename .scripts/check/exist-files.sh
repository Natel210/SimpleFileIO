#!/bin/bash

json_data="$1"
result_file="$2"

GREEN="\033[32m"
BLUE="\033[34m"
RED="\033[31m"
RESET="\033[0m"

if [ -z "$json_data" ]; then
  echo "::error::No JSON data provided. Usage: $0 '<JSON String>' [result_file]"
  exit 1
fi

FILES=$(echo "$json_data" | jq -r 'to_entries[] | "\(.key)\t\(.value)"')

if [ -z "$FILES" ]; then
  echo "::error::Invalid JSON structure."
  exit 1
fi

total_count=0
missing_count=0
output=""

while IFS=$'\t' read -r key path; do
  total_count=$((total_count + 1))
  if [ -f "$path" ] || [ -d "$path" ]; then
    output+="\n● $key\n  - $path : ${BLUE}Exist${RESET}"
  else
    missing_count=$((missing_count + 1))
    output+="\n● $key\n  - $path : ${RED}Not Exist${RESET}"
  fi
done <<< "$FILES"

if [ "$missing_count" -eq 0 ]; then
  summary="${GREEN}All OK.. ($total_count/$total_count)${RESET}\n"
else
  summary="${RED}Not Exist File Count $missing_count.. ($((total_count - missing_count))/$total_count)${RESET}\n"
fi

result="$summary$output"

if [ -z "$result_file" ]; then
  echo -e "$result"
else
  echo -e "$result" > "$result_file"
fi
