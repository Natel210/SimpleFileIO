#!/bin/bash

project_file_path=$1
build_configuration=$2
result_file="$3"

source ./.scripts/colors.sh

# 인자 유효성 검사
if [ -z "$project_file_path" ] || [ -z "$build_configuration" ]; then
    echo -e "${background_dark_red}${text_red}No arguments.\n ${background_dark_red}${text_red}Usage: $0 <Project File Path> <Build Configuration>${reset}"
    exit 1
fi

is_error=0
output="${background_light_gray}${text_white}Building project: $project_file_path with configuration: $build_configuration${reset}\n"

# 빌드 실행 및 출력 저장
build_output=$(dotnet build "$project_file_path" -c "$build_configuration" 2>&1)
build_exit_code=$?

# 빌드 결과 처리
if [ $build_exit_code -ne 0 ]; then
    output+="${background_dark_red}${text_red}Build failed.${reset}\n"
    output+="${text_red}Error Output:${reset}\n$build_output"
    summary="${background_dark_red}${text_red}Build failed.${reset}"
    is_error=1
else
    summary="${background_dark_green}${text_green}Build Test Completed Successfully.${reset}"
    output+="${text_light_gray}$build_output${reset}"
fi

# 결과 결합
result="$summary\n$output"

# 결과 출력 또는 파일 저장
if [ -z "$result_file" ]; then
  echo -e "$result"
else
  echo -e "$result" > "$result_file"
fi

# 오류 종료 처리
if [ $is_error -ne 0 ]; then
    exit 1
fi
