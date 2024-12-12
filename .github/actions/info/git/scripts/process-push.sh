#!/bin/bash

ref="${1:-unknown_ref}"

if [[ "$ref" == refs/heads/* ]]; then
  branch_name=${ref#refs/heads/}
  echo "::group::Push Info (branch : $branch_name)"
  echo "  > Ref: $ref"
  echo "  > Branch Name: $branch_name"
  echo "::endgroup::"
  
elif [[ "$ref" == refs/tags/* ]]; then
  tag_name=${ref#refs/tags/}
  echo "::group::Push Info (tag : $tag_name)"
  echo "  > Ref: $ref"
  echo "  > Tag Name: $tag_name"
  echo "::endgroup::"
else
  echo "::group::Push Info (Unknown)"
  echo "  > Ref: $ref"
  echo "::endgroup::"
  exit 1
fi
