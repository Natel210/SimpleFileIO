#!/bin/bash

echo -e "\033[36m● Git Info -----------------------------\033[0m"

# Read arguments
scripts_path="${1}"
event_name="${2}"
ref="${3:-unknown_ref}"
head_ref="${4:-unknown_head_ref}"
base_ref="${5:-unknown_base_ref}"

is_error=0

# Handle different events
case "$event_name" in
  push)
    "$scripts_path/git-info-push.sh" "$ref"
    ;;
  pull_request)
    "$scripts_path/git-info-pull-request.sh" "$head_ref" "$base_ref"
    ;;
  *)
    echo -e "::error::\033[31mUnknown event type: $event_name\033[0m"
    is_error=1
    ;;
esac

echo "::group::Arguments"
echo "> Script Directory : $scripts_path"
echo "> Event Name : $event_name"
echo "> Ref : $ref"
echo "> Head Ref : $head_ref"
echo "> Base Ref : $base_ref"
echo "::endgroup::"
echo -e "\033[36m----------------------------------------\033[0m"

if [[ "$is_error" != 0 ]]; then
    exit 1
fi
