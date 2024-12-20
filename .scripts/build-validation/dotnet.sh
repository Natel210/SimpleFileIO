#!/bin/bash

project_file_path=$1
build_configuration=$2
result_file="$3"

source ./.scripts/colors.sh

if [ -z "$project_file_path" ] || [ -z "$build_configuration" ]; then
    echo -e "${background_dark_red}${text_red}No arguments.\n ${background_dark_red}${text_red}Usage: $0 <Project File Path> <Build Configuration> <Result File>${reset}"
    exit 1
fi

is_error=0
output="${background_light_gray}${text_white}Building project: $project_file_path with configuration: $build_configuration${reset}\n"

# Execute dotnet build and capture output
build_output=$(dotnet build "$project_file_path" -c "$build_configuration" 2>&1)
build_exit_code=$?

if [ $build_exit_code -ne 0 ]; then
    output+="${background_dark_red}${text_red}Build failed.${reset}\n"
    output+="${text_red}Error Output:${reset}\n"
    while IFS= read -r line; do
        output+="${text_light_gray} - $line${reset}\n"
    done <<< "$build_output"

    summary="${background_dark_red}${text_red}Build failed.${reset}"
    is_error=1
else
    summary="${background_dark_green}${text_green}Build Test Completed Successfully.${reset}"
    while IFS= read -r line; do
        output+="${text_light_gray} - $line${reset}\n"
    done <<< "$build_output"
fi

result="$output$summary"

if [ -n "$result_file" ]; then
    # Ensure result file directory exists
    result_dir=$(dirname "$result_file")
    if [ ! -d "$result_dir" ]; then
        mkdir -p "$result_dir"
    fi
    touch $result_file
    echo -e "${background_light_gray}${text_white}Result to File ${result_file}${reset}"
    echo -e "$result" > "$result_file"
else
    echo -e "${background_light_gray}${text_white}Result to Console${reset}"
    echo -e "$result"
fi

if [ $is_error -ne 0 ]; then
    exit 1
fi
