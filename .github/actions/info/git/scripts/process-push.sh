#!/bin/bash

# ANSI Color Codes
BOLD="\033[1m"
CYAN="\033[36m"
GREEN="\033[32m"
RED="\033[31m"
RESET="\033[0m"

ref="${1:-unknown_ref}"

if [[ "$ref" == refs/heads/* ]]; then
  branch_name=${ref#refs/heads/}
  echo -e "> Push to branch : ${BOLD}${CYAN}$branch_name${RESET}"
  echo "::group::"
  echo "  > Ref: $ref"
  echo "  > Branch Name: $branch_name"
  echo "::endgroup::"

  echo -e "> Push to tag : ${BOLD}${GREEN}$tag_name${RESET}"
  echo -e "::group::detail ${BOLD}${GREEN}$tag_name${RESET}"
  echo "  > Ref: $ref"
  echo "  > Tag Name: $tag_name"
  echo "::endgroup::"

    echo -e "> ${BOLD}${RED}Unknown : $ref${RESET}"
  
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
