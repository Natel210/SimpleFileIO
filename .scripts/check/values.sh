#!/bin/bash

source ./.scripts/tools/colors.sh

if [ "$#" -lt 1 ]; then
  echo "${background_dark_red}${text_red}No arguments. \n${background_dark_red}${text_red} Usage: $0 '<JSON String>'${reset}"
  exit 1
fi

json_data="$1"
result_file="$2"



if [ -z "$result_file" ]; then
  echo -e "${background_light_gray}${text_white}Result to Console${reset}"
  echo -e "$json_data" | jq -r 'to_entries[] | "\(.key) : \(.value)"'
else
  # Ensure result file directory exists
  result_dir=$(dirname "$result_file")
  if [ ! -d "$result_dir" ]; then
      mkdir -p "$result_dir"
  fi
  echo -e "${background_light_gray}${text_white}Result to File ${result_file}${reset}"
  echo -e "$json_data" | jq -r 'to_entries[] | "\(.key) : \(.value)"' > "$result_file"
fi