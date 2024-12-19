#!/bin/bash

json_data="$1"
result_file="$2"

TEXT_WHITE="\033[38;5;15m"
TEXT_RED="\033[38;5;196m"
TEXT_BLUE="\033[38;5;27m"
TEXT_GREEN="\033[38;5;46m"
TEXT_LIGHT_GRAY="\033[38;5;245m"
TEXT_DARK_RED="\033[38;5;124m"
TEXT_DARK_BLUE="\033[38;5;20m"
TEXT_DARK_GREEN="\033[38;5;28m"

BACKGROUND_LIGHT_GRAY="\033[48;5;245m"
BACKGROUND_DARK_RED="\033[48;5;52m"
BACKGROUND_DARK_BLUE="\033[48;5;19m"
BACKGROUND_DARK_GREEN="\033[48;5;22m"

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
    output+="\n${TEXT_LIGHT_GRAY}● $key\n${TEXT_LIGHT_GRAY}  - $path : ${TEXT_DARK_BLUE}Exist${RESET}"
  else
    missing_count=$((missing_count + 1))
    output+="\n${TEXT_LIGHT_GRAY}● $key\n${TEXT_LIGHT_GRAY}  - $path : ${TEXT_DARK_RED}Not Exist${RESET}"
  fi
done <<< "$FILES"

if [ "$missing_count" -eq 0 ]; then
  summary="${BACKGROUND_DARK_GREEN}${TEXT_GREEN}All OK.. ($total_count/$total_count)${RESET}"
else
  summary="${BACKGROUND_DARK_RED}${TEXT_RED}Not Exist File Count $missing_count.. ($((total_count - missing_count))/$total_count)${RESET}"
fi

result="$summary$output"

if [ -z "$result_file" ]; then
  echo -e "$result"
else
  echo -e "$result" > "$result_file"
fi
