#!/bin/bash

ref="${1:-unknown_ref}"

if [[ "$ref" == refs/heads/* ]]; then
  branch_name=${ref#refs/heads/}
  echo "::group::Push Info (branch : \033[1m\033[32m$branch_name\033[0m)"
  echo "  > Ref: $ref"
  echo "  > Branch Name: $branch_name"
  echo "::endgroup::"
  
elif [[ "$ref" == refs/tags/* ]]; then
  tag_name=${ref#refs/tags/}
  echo "::group::Push Info (tag : \033[1m\033[32m$tag_name\033[0m)"
  echo "  > Ref: $ref"
  echo "  > Tag Name: $tag_name"
  echo "::endgroup::"
else
  echo "::group::Push Info (\033[1m\033[31mUnknown\033[0m)"
  echo "  > Ref: $ref"
  echo "::endgroup::"
  exit 1
fi
