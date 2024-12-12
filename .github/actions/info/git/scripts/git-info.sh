#!/bin/bash


# Read arguments
scripts_path="${1}"
event_name="${2}"
ref="${3:-unknown_ref}"
head_ref="${4:-unknown_head_ref}"
base_ref="${5:-unknown_base_ref}"

echo "::group::Git Info ($event_name)"
echo "Git Info"
echo "|- Script Directory"
echo "|   |- $scripts_path"
echo "|- Git Info Script"
echo "|   |- Event Name: $event_name"
echo "|   |- Ref: $ref"
echo "|   |- Head Ref: $head_ref"
echo "|   |- Base Ref: $base_ref"

# Handle different events
case "$event_name" in
  push)
    echo "::group::Push Info (Branch : $ref)"
    "$scripts_path/process-push.sh" "$ref"
    echo "|   |"
    echo "::endgroup::"
    echo "::endgroup::"
    ;;
  pull_request)
    echo "::group::Pull Request Info (source : $head_ref, target : $base_ref)"
    "$scripts_path/process-pull-request.sh" "$head_ref" "$base_ref"
    echo "================================================================================"
    echo "::endgroup::"
    echo "::endgroup::"
    ;;
  *)
    echo "|- Unknown event type: $event_name"
    echo "================================================================================"
    echo "::endgroup::"
    exit 1
    ;;
esac
