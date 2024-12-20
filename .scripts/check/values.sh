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
  echo -e "${background_light_gray}${text_white}Result to File ${result_file}${reset}"
  touch $result_file
  echo -e "$json_data" | jq -r 'to_entries[] | "\(.key) : \(.value)"' > "$result_file"
fi