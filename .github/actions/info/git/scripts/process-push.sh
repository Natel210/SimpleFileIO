#!/bin/bash

BOLD="\033[1m"
YELLOW="\033[33m"
RESET="\033[0m"

ref="${1:-unknown_ref}"

if [[ "$ref" == refs/heads/* ]]; then
  branch_name=${ref#refs/heads/}
  echo "> Push to branch : \033[1m\033[36m$branch_name\033[0m"
  echo "::group::"
  echo "  > Ref: $ref"
  echo "  > Branch Name: $branch_name"
  echo "::endgroup::"

  echo "> Push to tag : \033[1m\033[32m$tag_name\033[0m"
  echo "::group::"
  echo "  > Ref: $ref"
  echo "  > Tag Name: $tag_name"
  echo "::endgroup::"

    echo "> \033[1m\033[31m Unknown : $ref\033[0m"
  
elif [[ "$ref" == refs/tags/* ]]; then
  tag_name=${ref#refs/tags/}
  echo "> Push to tag : \033[1m\033[36m$tag_name\033[0m"
  echo "::group::"
  echo "  > Ref: $ref"
  echo "  > Tag Name: $tag_name"
  echo "::endgroup::"
else
  echo "> \033[1m\033[31m Unknown : $ref\033[0m"
  exit 1
fi
