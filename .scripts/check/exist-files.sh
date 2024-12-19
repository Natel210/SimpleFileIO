#!/bin/bash

json_data="$1"
result_file="$2"

TEXT_BLACK="\033[38;5;0m"
TEXT_RED="\033[38;5;196m"
TEXT_GREEN="\033[38;5;46m"
TEXT_BLUE="\033[38;5;51m"
BACKGROUND_DARK_GRAY="\033[48;5;235m"
BACKGROUND_DARK_GREEN="\033[48;5;22m"
BACKGROUND_DARK_RED="\033[48;5;52m"
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
    output+="\n● $key\n  - $path : ${TEXT_BLUE}Exist${RESET}"
  else
    missing_count=$((missing_count + 1))
    output+="\n● $key\n  - $path : ${TEXT_RED}Not Exist${RESET}"
  fi
done <<< "$FILES"

if [ "$missing_count" -eq 0 ]; then
  summary="${BACKGROUND_DARK_GREEN}${TEXT_BLACK}All OK.. ($total_count/$total_count)${RESET}\n"
else
  summary="${BACKGROUND_DARK_RED}${TEXT_BLACK}Not Exist File Count $missing_count.. ($((total_count - missing_count))/$total_count)${RESET}\n"
fi

result="$summary$output"

if [ -z "$result_file" ]; then
  echo -e "$result"
else
  echo -e "$result" > "$result_file"
fi
