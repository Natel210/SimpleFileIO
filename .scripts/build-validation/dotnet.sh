#!/bin/bash

project_file_path=$1
build_configuration=$2
result_file="$3"

source ./.scripts/colors.sh

if [ -z "$project_file_path" ] || [ -z "$build_configuration" ]; then
    echo -e "${background_dark_red}${text_red}No arguments.\n ${background_dark_red}${text_red}Usage: $0 <Project File Path> <Build Configuration>${reset}"
    exit 1
fi
is_error=0
output= "${background_light_gray}${text_white}Building project: $project_file_path with configuration: $build_configuration${reset}\n"
build_output+=$(dotnet build "$project_file_path" -c "$build_configuration" 2>&1)
build_exit_code=$?

if [ $build_exit_code -ne 0 ]; then
    output+= "${background_dark_red}${text_red}Build failed.${reset}\n"
    summary= "${background_dark_red}${text_red}Build failed.${reset}"
    is_error=1
else
    summary="${background_dark_green}${text_green}Build Test Completed Successfully.${reset}"
fi

result="$summary$output${text_light_gray}$build_output${reset}"

if [ -z "$result_file" ]; then
  echo -e "$result"
else
  echo -e "$result" > "$result_file"
fi

if [ $is_error != 0 ]
    exit 1
fi