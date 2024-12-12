#!/bin/bash

ref="${1:-unknown_ref}"

if [[ "$ref" == refs/heads/* ]]; then
  branch_name=${ref#refs/heads/}
  echo "  > Branch Name: $branch_name"
elif [[ "$ref" == refs/tags/* ]]; then
  tag_name=${ref#refs/tags/}
  echo "  > Tag Name: $tag_name"
else
  echo "  Unknown ref type: $ref"
  exit 1
fi
