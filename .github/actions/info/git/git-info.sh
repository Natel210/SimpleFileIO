#!/bin/bash

# Read arguments
event_name="${1:-unknown_event}"
ref="${2:-unknown_ref}"
head_ref="${3:-unknown_head_ref}"
base_ref="${4:-unknown_base_ref}"

echo "Git Info Script"
echo "Event Name: $event_name"
echo "Ref: $ref"
echo "Head Ref: $head_ref"
echo "Base Ref: $base_ref"

# Handle different events
case "$event_name" in
  push)
    echo "Handling Push Event"
    .github/scripts/info/git/process-push.sh "$ref"
    ;;
  pull_request)
    echo "Handling Pull Request Event"
    .github/scripts/info/git/process-pull-request.sh "$head_ref" "$base_ref"
    ;;
  *)
    echo "Unknown event type: $event_name"
    exit 1
    ;;
esac
