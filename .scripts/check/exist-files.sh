#!/bin/bash

JSON_DATA="$1"
RESULT_FILE="$2"

# ANSI 색상 코드
GREEN="\033[32m"
BLUE="\033[34m"
RED="\033[31m"
RESET="\033[0m"

# JSON 데이터 확인
if [ -z "$JSON_DATA" ]; then
  echo "::error::No JSON data provided. Usage: $0 '<JSON String>' [result_file]"
  exit 1
fi

# JSON 파싱 및 파일 존재 확인
FILES=$(echo "$JSON_DATA" | jq -r 'to_entries[] | "\(.key)\t\(.value)"')

if [ -z "$FILES" ]; then
  echo "::error::Invalid JSON structure."
  exit 1
fi

# 파일 검사 및 출력 생성
total_count=0
missing_count=0
output=""

while IFS=$'\t' read -r key path; do
  total_count=$((total_count + 1))
  if [ -f "$path" ] || [ -d "$path" ]; then
    output+="● $key\n    - Path : $path\n    - Exist : ${BLUE}Exist${RESET}\n"
  else
    missing_count=$((missing_count + 1))
    output+="● $key\n    - Path : $path\n    - Exist : ${RED}Not Exist${RESET}\n"
  fi
done <<< "$FILES"

# 전체 상태 메시지
if [ "$missing_count" -eq 0 ]; then
  summary="${GREEN}All OK.. ($total_count/$total_count)${RESET}\n"
else
  summary="${RED}Not Exist File Count $missing_count.. ($((total_count - missing_count))/$total_count)${RESET}\n"
fi

# 최종 출력 생성
result="$summary\n$output"

# 출력 또는 파일 쓰기
if [ -z "$RESULT_FILE" ]; then
  echo -e "$result"
else
  echo -e "$result" > "$RESULT_FILE"
fi
