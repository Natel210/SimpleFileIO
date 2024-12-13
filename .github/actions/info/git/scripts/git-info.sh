#!/bin/bash


# Read arguments
scripts_path="${1}"
event_name="${2}"
ref="${3:-unknown_ref}"
head_ref="${4:-unknown_head_ref}"
base_ref="${5:-unknown_base_ref}"

echo "---- Enter Git Info Script----"
echo -e "::group::Argument (event:\033[34m$event_name\033[0m)"
echo "> Script Directory : $scripts_path"
echo "> Event Name: $event_name"
echo "> Ref: $ref"
echo "> Head Ref: $head_ref"
echo "> Base Ref: $base_ref"
echo "::endgroup::"

# Handle different events
case "$event_name" in
  push)
    "$scripts_path/process-push.sh" "$ref"
    ;;
  pull_request)
    "$scripts_path/process-pull-request.sh" "$head_ref" "$base_ref"
    ;;
  *)
    echo -e "\033[31mUnknown event type: $event_name\033[0m"
    echo "---- End Git Info Script----"
    echo " "
    exit 1
    ;;
esac
echo "---- End Git Info Script----"