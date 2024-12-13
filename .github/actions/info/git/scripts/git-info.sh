#!/bin/bash


# Read arguments
scripts_path="${1}"
event_name="${2}"
ref="${3:-unknown_ref}"
head_ref="${4:-unknown_head_ref}"
base_ref="${5:-unknown_base_ref}"

echo -e "⤷ - \033[32mGit Info\033[0m -"
echo -e "::group::  - Arguments"
echo "    > Script Directory : $scripts_path"
echo -e "    > Event Name: \033[32m$event_name\033[0m"
echo "    > Ref: $ref"
echo "    > Head Ref: $head_ref"
echo "    > Base Ref: $base_ref"
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
    echo -e "⤷ \033[31mUnknown event type: $event_name\033[0m"
    echo " "
    exit 1
    ;;
esac
echo "---- ----"