#!/bin/bash


JSON_FILE="$1"
RESULT_FILE="$2"

while IFS="=" read -r key value; do
  echo "${key} : ${value}"
done < <(cat $JSON_FILE | jq -r 'to_entries[] | "\(.key)=\(.value)"')