#!/bin/bash

json_data="$1"
result_file="$2"

source ./.scripts/tools/colors.sh

if [ -z "$json_data" ]; then
  echo "${background_dark_red}${text_red}No JSON data provided.\n${background_dark_red}${text_red} Usage: $0 '<JSON String>' [result_file]${reset}"
  exit 1
fi

FILES=$(echo "$json_data" | jq -r 'to_entries[] | "\(.key)\t\(.value)"')

if [ -z "$FILES" ]; then
  echo "${background_dark_red}${text_red}Invalid JSON structure.${reset}"
  exit 1
fi

total_count=0
missing_count=0
output=""

while IFS=$'\t' read -r key path; do
  total_count=$((total_count + 1))
  if [ -f "$path" ] || [ -d "$path" ]; then
    output+="\n${text_light_gray}● $key\n${text_light_gray}  - $path : ${text_dark_blue}Exist${reset}"
  else
    missing_count=$((missing_count + 1))
    output+="\n${text_light_gray}● $key\n${text_light_gray}  - $path : ${text_dark_red}Not Exist${reset}"
  fi
done <<< "$FILES"

if [ "$missing_count" -eq 0 ]; then
  summary="${background_dark_green}${text_green}All OK.. ($total_count/$total_count)${reset}"
else
  summary="${background_dark_red}${text_red}Not Exist File Count $missing_count.. ($((total_count - missing_count))/$total_count)${reset}"
fi

result="$summary$output"

if [ -z "$result_file" ]; then
  echo -e "${background_light_gray}${text_white}Result to Console${reset}"
  echo -e "$result"
else
  # Ensure result file directory exists
  result_dir=$(dirname "$result_file")
  if [ ! -d "$result_dir" ]; then
      mkdir -p "$result_dir"
  fi
  echo -e "${background_light_gray}${text_white}Result to File ${result_file}${reset}"
  echo -e "$result" > "$result_file"
fi
