#!/bin/bash

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

if [ "$#" -lt 1 ]; then
  echo "${background_dark_red}${text_red}No arguments. \n${background_dark_red}${text_red} Usage: $0 '<JSON String>'${reset}"
  exit 1
fi

json_data="$1"
result_file="$2"



if [ -z "$result_file" ]; then
  echo -e "$json_data" | jq -r 'to_entries[] | "\(.key) : \(.value)"'
else
  touch $result_file
  echo -e "$json_data" | jq -r 'to_entries[] | "\(.key) : \(.value)"' > "$result_file"
  cat "$result_file"
fi