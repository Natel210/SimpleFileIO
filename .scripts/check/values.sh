#!/bin/bash


JSON_FILE="$1"
RESULT_FILE="$2"

# JSON 데이터에서 키와 값을 파싱하여 출력
if [ -z "$RESULT_FILE" ]; then
  # RESULT_FILE이 없으면 표준 출력으로 출력
  echo "$JSON_DATA" | jq -r 'to_entries[] | "\(.key) : \(.value)"'
else
  # RESULT_FILE이 있으면 파일로 저장
  echo "$JSON_DATA" | jq -r 'to_entries[] | "\(.key) : \(.value)"' > "$RESULT_FILE"
  cat "$RESULT_FILE"
fi