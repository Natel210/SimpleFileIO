#!/bin/bash

# Read arguments from command line
event_name="${1:-unknown_event}"
ref="${2:-unknown_ref}"
head_ref="${3:-unknown_head_ref}"
base_ref="${4:-unknown_base_ref}"

echo "Git Info Script"
echo "Event Name: $event_name"
echo "Ref: $ref"
echo "Head Ref: $head_ref"
echo "Base Ref: $base_ref"

# Handle different GitHub events
if [[ "$event_name" == "push" ]]; then
  echo "Event: Push"
  if [[ "$ref" == refs/heads/* ]]; then
    branch_name=${ref#refs/heads/}
    echo "Branch Name: $branch_name"
  elif [[ "$ref" == refs/tags/* ]]; then
    tag_name=${ref#refs/tags/}
    echo "Tag Name: $tag_name"
  else
    echo "Unknown ref type: $ref"
    exit 1
  fi
elif [[ "$event_name" == "pull_request" ]]; then
  echo "Event: Pull Request"
  echo "Source Branch: $head_ref"
  echo "Target Branch: $base_ref"
else
  echo "Unknown event type: $event_name"
  exit 1
fi
