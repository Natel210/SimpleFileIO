#!/bin/bash

project_file_path=$1
build_configuration=$2

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

if [ -z "$project_file_path" ] || [ -z "$build_configuration" ]; then
    echo -e "${background_dark_red}${text_red}No arguments.\n ${background_dark_red}${text_red}Usage: $0 <Project File Path> <Build Configuration>${reset}"
    exit 1
fi

echo "Building project: $project_file_path with configuration: $build_configuration"
dotnet build "$project_file_path" -c "$build_configuration"

if [ $? -ne 0 ]; then
    echo -e "${background_dark_red}${text_red}Build failed.${reset}"
    exit 1
fi

echo -e "${background_dark_green}${text_green}Build Test Completed Successfully.${reset}"