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

TEXT_WHITE="\033[38;5;15m"
TEXT_RED="\033[38;5;196m"
TEXT_BLUE="\033[38;5;30m"
TEXT_GREEN="\033[38;5;46m"
TEXT_LIGHT_GRAY="\033[38;5;245m"
TEXT_DARK_RED="\033[38;5;124m"
TEXT_DARK_BLUE="\033[38;5;20m"
TEXT_DARK_GREEN="\033[38;5;28m"

BACKGROUND_LIGHT_GRAY="\033[48;5;245m"
BACKGROUND_DARK_RED="\033[48;5;52m"
BACKGROUND_DARK_BLUE="\033[48;5;19m"
BACKGROUND_DARK_GREEN="\033[48;5;22m"

RESET="\033[0m"

is_error=0
# Handle different events
case "$event_name" in
  push)
    output="${TEXT_LIGHT_GRAY}● Detail${RESET}"
    output+="\n${TEXT_LIGHT_GRAY}  - Event : ${TEXT_DARK_GREEN}Push${RESET}"
    output+="\n${TEXT_LIGHT_GRAY}  - Ref: $ref${RESET}"
    if [[ "$ref" == refs/heads/* ]]; then
      branch_name=${ref#refs/heads/}
      output+="\n${TEXT_LIGHT_GRAY}  - Branch: ${TEXT_DARK_BLUE}$branch_name${RESET}"
      summary="${BACKGROUND_DARK_GREEN}${TEXT_GREEN}Push${TEXT_WHITE} Branch : ${TEXT_BLUE}$branch_name${RESET}\n"
    elif [[ "$ref" == refs/tags/* ]]; then
      tag_name=${ref#refs/tags/}
      output+="\n${TEXT_LIGHT_GRAY}  - Branch: ${TEXT_DARK_BLUE}$tag_name${RESET}"
      summary="${BACKGROUND_DARK_GREEN}${TEXT_GREEN}Push Tag : $tag_name${RESET}\n"
    else
      output+="\n${TEXT_LIGHT_GRAY}  - ${TEXT_DARK_RED}Unknown${RESET}"
      summary="${BACKGROUND_DARK_RED}${TEXT_RED}Push Unknown${RESET}\n"
      is_error=1
    fi
    ;;
  pull_request)
    summary="${TEXT_GREEN}Pull Request"
    output="${TEXT_LIGHT_GRAY}● Detail${RESET}"
    output+="${TEXT_LIGHT_GRAY}\n  - Event : ${TEXT_DARK_GREEN}Pull Request${RESET}\n"
    if [[ "$head_ref" == refs/heads/* ]]; then
      output+="${TEXT_LIGHT_GRAY}\n  - Head Ref : ${TEXT_DARK_RED}$head_ref${RESET}"
      summary+="$\n  ${TEXT_RED}Invalid Head Ref ($head_ref)"
      is_error=1
    else
      output+="${TEXT_LIGHT_GRAY}\n  - Head Ref : $head_ref${RESET}"
    fi
    if [[ "$base_ref" == refs/heads/* ]]; then
      output+="${TEXT_LIGHT_GRAY}\n  - Base Ref : ${TEXT_DARK_RED}$base_ref${RESET}"
      summary+="\n  ${TEXT_RED}Invalid Base Ref ($base_ref)"
      is_error=1
    else
      output+="$\n  - Base Ref : $base_ref${RESET}"
    fi
    
    if [[ "$is_error" == "0" ]]; then
      head_branch_name=${head_ref#refs/heads/}
      base_branch_name=${base_ref#refs/heads/}
      output+="${TEXT_LIGHT_GRAY}\n  - Head Branch : $head_branch_name${RESET}"
      output+="${TEXT_LIGHT_GRAY}\n  - Base Branch : $base_branch_name${RESET}"
      output+="${TEXT_LIGHT_GRAY}\n  $head_branch_name -> $base_branch_name${RESET}"
      summary="${BACKGROUND_DARK_GRAY}${summary}${TEXT_WHITE} : $head_branch_name -> $base_branch_name${RESET}"
    else
      summary="${BACKGROUND_DARK_RED}${summary}${RESET}"
    fi
    summary+="\n"
    ;;
  *)
    output=" ${TEXT_LIGHT_GRAY}Event : ${TEXT_DARK_RED}$event_name${RESET}\n"
    output+="${TEXT_DARK_RED}Unknown Event Type${RESET}"
    summary="${BACKGROUND_DARK_RED}${TEXT_RED}Unknown Event Type ($base_ref) ${RESET}"
    is_error=1
    ;;
esac


result="$summary$output"

if [ -z "$result_file" ]; then
  echo -e "$result"
else
  echo -e "$result" > "$result_file"
fi