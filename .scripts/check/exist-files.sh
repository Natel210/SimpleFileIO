#!/bin/bash

JSON_DATA="$1"
RESULT_FILE="$2"

GREEN="\033[32m"
BLUE="\033[34m"
RED="\033[31m"
RESET="\033[0m"

if [ -z "$JSON_DATA" ]; then
  echo "::error::No JSON data provided. Usage: $0 '<JSON String>' [result_file]"
  exit 1
fi

FILES=$(echo "$JSON_DATA" | jq -r 'to_entries[] | "\(.key)\t\(.value)"')

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
    output+="● $key\n    - Path : $path\n    - Exist : ${BLUE}Exist${RESET}\n"
  else
    missing_count=$((missing_count + 1))
    output+="● $key\n    - Path : $path\n    - Exist : ${RED}Not Exist${RESET}\n"
  fi
done <<< "$FILES"

if [ "$missing_count" -eq 0 ]; then
  summary="${GREEN}All OK.. ($total_count/$total_count)${RESET}\n"
else
  summary="${RED}Not Exist File Count $missing_count.. ($((total_count - missing_count))/$total_count)${RESET}\n"
fi

result="$summary\n$output"

if [ -z "$RESULT_FILE" ]; then
  echo -e "$result"
else
  echo -e "$result" > "$RESULT_FILE"
fi
