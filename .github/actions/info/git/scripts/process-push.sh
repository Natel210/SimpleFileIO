#!/bin/bash

BOLD="\033[1m"
YELLOW="\033[33m"
RESET="\033[0m"

ref="${1:-unknown_ref}"

if [[ "$ref" == refs/heads/* ]]; then
  branch_name=${ref#refs/heads/}
  echo "::group::Push Info (branch : ${BOLD}${YELLOW}$branch_name${RESET})"
  echo "  > Ref: $ref"
  echo "  > Branch Name: $branch_name"
  echo "::endgroup::"
  
elif [[ "$ref" == refs/tags/* ]]; then
  tag_name=${ref#refs/tags/}
  echo "::group::Push Info (tag : ${BOLD}${YELLOW}$tag_name${RESET})"
  echo "  > Ref: $ref"
  echo "  > Tag Name: $tag_name"
  echo "::endgroup::"
else
  echo "::group::Push Info (${BOLD}${YELLOW}Unknown${RESET})"
  echo "  > Ref: $ref"
  echo "::endgroup::"
  exit 1
fi
