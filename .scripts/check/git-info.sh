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
result_file="$4"

is_error=0
# Handle different events
case "$event_name" in
  push)
    output="● Detail"
    output+="  - Event : \033[32mPush\033[0m\n"
    output+="  - Ref: $ref\n"
    if [[ "$ref" == refs/heads/* ]]; then
      branch_name=${ref#refs/heads/}
      output+="  - Branch: \033[34m$branch_name\033[0m\n"
      summary="\033[32mPush\033[0m Branch : \033[34m$branch_name\033[0m\n"
    elif [[ "$ref" == refs/tags/* ]]; then
      tag_name=${ref#refs/tags/}
      output+="  - Branch: \033[34m$tag_name\033[0m\n"
      summary="\033[32mPush\033[0m Tag : \033[34m$tag_name\033[0m\n"
    else
      output+="  - \033[31mUnknown\033[0m\n"
      summary="\033[32mPush\033[0m \033[31mUnknown\033[0m\n"
      is_error=1
    fi
    ;;
  pull_request)
    summary="\033[32mPull Request\033[0m"
    output="● Detail"
    output+="  - Event : \033[32mmPull Request\033[0m\n"
    if [[ "$head_ref" == refs/heads/* ]]; then
      output+="  - Head Ref : \033[31m$head_ref\033[0m"
      summary+="\n  \033[31mInvalid Head Ref ($head_ref) \033[0m"
      is_error=1
    else
      output+="  - Head Ref : $head_ref"
    fi
    if [[ "$base_ref" == refs/heads/* ]]; then
      output+="  - Base Ref : \033[31m$base_ref\033[0m"
      summary+="\n  \033[31mInvalid Base Ref ($base_ref) \033[0m"
      is_error=1
    else
      output+="  - Base Ref : $base_ref"
    fi
    
    if [[ "$is_error" == "0" ]]; then
      head_branch_name=${head_ref#refs/heads/}
      base_branch_name=${base_ref#refs/heads/}
      output+="  - Head Branch : $head_branch_name"
      output+="  - Base Branch : $base_branch_name"
      output+="  $head_branch_name -> $base_branch_name"
      summary+=" : $head_branch_name -> $base_branch_name"
    fi
    summary+="\n"
    ;;
  *)
    output="Event : \033[31m$event_name\033[0m\n"
    output+="\033[31mUnknown event type\033[0m"
    is_error=1
    ;;
esac


result="$summary$output"

if [ -z "$result_file" ]; then
  echo -e "$result"
else
  echo -e "$result" > "$result_file"
fi