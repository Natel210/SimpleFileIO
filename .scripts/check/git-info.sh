#!/bin/bash


if [ "$#" -lt 1 ]; then
  echo "::error::No arguments. \n Usage: $0 '<Git Event Name>'"
  exit 1
fi

# Read arguments
event_name="${1}"
ref="${2:-unknown_ref}"
head_ref="${3:-unknown_head_ref}"
base_ref="${4:-unknown_base_ref}"
result_file="$5"

TEXT_BLACK="\033[38;5;0m"
TEXT_RED="\033[38;5;196m"
TEXT_GREEN="\033[38;5;46m"
TEXT_BLUE="\033[38;5;51m"
BACKGROUND_DARK_GRAY="\033[48;5;235m"
BACKGROUND_DARK_RED="\033[48;5;52m"
RESET="\033[0m"

is_error=0
# Handle different events
case "$event_name" in
  push)
    output="● Detail"
    output+="\n  - Event : ${TEXT_GREEN}Push${RESET}"
    output+="\n  - Ref: $ref"
    if [[ "$ref" == refs/heads/* ]]; then
      branch_name=${ref#refs/heads/}
      output+="\n  - Branch: ${TEXT_BLUE}$branch_name${RESET}"
      summary="${BACKGROUND_DARK_GRAY}${TEXT_BLACK}Push Branch : $branch_name${RESET}\n"
    elif [[ "$ref" == refs/tags/* ]]; then
      tag_name=${ref#refs/tags/}
      output+="\n  - Branch: ${TEXT_BLUE}$tag_name${RESET}"
      summary="${BACKGROUND_DARK_GRAY}${TEXT_BLACK}Push Tag : $tag_name${RESET}\n"
    else
      output+="\n  - ${TEXT_RED}Unknown${RESET}"
      summary="${BACKGROUND_DARK_RED}${TEXT_BLACK}Push Unknown${RESET}\n"
      is_error=1
    fi
    ;;
  pull_request)
    summary="${TEXT_GREEN}Pull Request${RESET}"
    output="● Detail"
    output+="\n  - Event : ${TEXT_GREEN}Pull Request${RESET}\n"
    if [[ "$head_ref" == refs/heads/* ]]; then
      output+="\n  - Head Ref : ${TEXT_RED}$head_ref${RESET}"
      summary+="\n  ${BACKGROUND_DARK_RED}${TEXT_BLACK}Invalid Head Ref ($head_ref) ${RESET}"
      is_error=1
    else
      output+="\n  - Head Ref : $head_ref"
    fi
    if [[ "$base_ref" == refs/heads/* ]]; then
      output+="\n  - Base Ref : ${TEXT_RED}$base_ref${RESET}"
      summary+="\n  ${BACKGROUND_DARK_RED}${TEXT_BLACK}Invalid Base Ref ($base_ref) ${RESET}"
      is_error=1
    else
      output+="\n  - Base Ref : $base_ref"
    fi
    
    if [[ "$is_error" == "0" ]]; then
      head_branch_name=${head_ref#refs/heads/}
      base_branch_name=${base_ref#refs/heads/}
      output+="\n  - Head Branch : $head_branch_name"
      output+="\n  - Base Branch : $base_branch_name"
      output+="\n  $head_branch_name -> $base_branch_name"
      summary+=" : $head_branch_name -> $base_branch_name"
    fi
    summary+="\n"
    ;;
  *)
    output="Event : ${TEXT_RED}$event_name${RESET}\n"
    output+="${TEXT_RED}Unknown Event Type${RESET}"
    summary="${BACKGROUND_DARK_RED}${TEXT_BLACK}Unknown Event Type ($base_ref) ${RESET}"
    is_error=1
    ;;
esac


result="$summary$output"

if [ -z "$result_file" ]; then
  echo -e "$result"
else
  echo -e "$result" > "$result_file"
fi