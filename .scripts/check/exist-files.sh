#!/bin/bash

json_data="$1"
result_file="$2"

text_white="\033[38;5;15m"
text_red="\033[38;5;196m"
text_blue="\033[38;5;27m"
text_green="\033[38;5;46m"
text_light_gray="\033[38;5;245m"
text_dark_red="\033[38;5;124m"
text_dark_blue="\033[38;5;20m"
text_dark_green="\033[38;5;28m"

background_light_gray="\033[48;5;245m"
background_dark_red="\033[48;5;52m"
background_dark_blue="\033[48;5;19m"
background_dark_green="\033[48;5;22m"

reset="\033[0m"

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
  echo -e "$result"
else
  echo -e "$result" > "$result_file"
fi
