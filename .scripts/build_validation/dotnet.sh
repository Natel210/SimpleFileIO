#!/bin/bash

project_file_path=$1
build_configuration=$2
result_file="$3"

if [ -z "$project_file_path" ] || [ -z "$build_configuration" ]; then
    echo -e "\033[38;5;196mNo arguments.\n \033[38;5;196mUsage: $0 <Project File Path> <Build Configuration> <Result File>\033[0m"
    exit 1
fi

is_error=0
output="\033[0mBuilding project: $project_file_path with configuration: $build_configuration os:Linux \033[0m \n"

# Execute dotnet build and capture output
build_output=$(dotnet build "$project_file_path" -c "$build_configuration" 2>&1)
build_exit_code=$?

if [ $build_exit_code -ne 0 ]; then
    output+="\033[38;5;196mBuild failed.\033[0m\n"
    output+="\033[38;5;245mError Output:\033[0m\n"
    while IFS= read -r line; do
        output+="\033[38;5;245m - $line\033[0m\n"
    done <<< "$build_output"

    summary="\033[38;5;196mBuild failed.\033[0m"
    is_error=1
else
    summary="\033[38;5;46mBuild Test Completed Successfully.\033[0m"
    while IFS= read -r line; do
        output+="\033[38;5;245m - $line\033[0m\n"
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
    echo -e "$result" > "$result_file"
else
    echo -e "$result"
fi

if [ $is_error -ne 0 ]; then
    exit 1
fi
