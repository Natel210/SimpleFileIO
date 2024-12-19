#!/bin/bash

project_file_path=$1
build_configuration=$2

source ./.scripts/colors.sh

if [ -z "$project_file_path" ] || [ -z "$build_configuration" ]; then
    echo -e "${background_dark_red}${text_red}No arguments.\n ${background_dark_red}${text_red}Usage: $0 <Project File Path> <Build Configuration>${reset}"
    exit 1
fi

echo -e "Building project: $project_file_path with configuration: $build_configuration"
dotnet build "$project_file_path" -c "$build_configuration"

if [ $? -ne 0 ]; then
    echo -e "${background_dark_red}${text_red}Build failed.${reset}"
    exit 1
fi

echo -e "${background_dark_green}${text_green}Build Test Completed Successfully.${reset}"