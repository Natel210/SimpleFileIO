#!/bin/bash




# Read arguments
event_name="${1}"
ref="${2:-unknown_ref}"
head_ref="${3:-unknown_head_ref}"
base_ref="${4:-unknown_base_ref}"
result_file="$5"

source ./.scripts/colors.sh

if [ "$#" -lt 1 ]; then
  echo "${background_dark_red}${text_red}No arguments.\n${background_dark_red}${text_red} Usage: $0 '<Git Event Name>'${reset}"
  exit 1
fi


is_error=0
# Handle different events
case "$event_name" in
  push)
    output="${text_light_gray}● Detail${reset}"
    output+="\n${text_light_gray}  - Event : ${text_dark_green}Push${reset}"
    output+="\n${text_light_gray}  - Ref: $ref${reset}"
    if [[ "$ref" == refs/heads/* ]]; then
      branch_name=${ref#refs/heads/}
      output+="\n${text_light_gray}  - Branch: ${text_dark_blue}$branch_name${reset}"
      summary="${background_dark_green}${text_green}Push Branch : $branch_name${reset}\n"
    elif [[ "$ref" == refs/tags/* ]]; then
      tag_name=${ref#refs/tags/}
      output+="\n${text_light_gray}  - Branch: ${text_dark_blue}$tag_name${reset}"
      summary="${background_dark_green}${text_green}Push Tag : $tag_name${reset}\n"
    else
      output+="\n${text_light_gray}  - ${text_dark_red}Unknown${reset}"
      summary="${background_dark_red}${text_red}Push Unknown${reset}\n"
      is_error=1
    fi
    ;;
  pull_request)
    summary="${text_green}Pull Request"
    output="${text_light_gray}● Detail${reset}"
    output+="${text_light_gray}\n  - Event : ${text_dark_green}Pull Request${reset}\n"
    if [[ "$head_ref" == refs/heads/* ]]; then
      output+="${text_light_gray}\n  - Head Ref : ${text_dark_red}$head_ref${reset}"
      summary+="$\n  ${text_red}Invalid Head Ref ($head_ref)"
      is_error=1
    else
      output+="${text_light_gray}\n  - Head Ref : $head_ref${reset}"
    fi
    if [[ "$base_ref" == refs/heads/* ]]; then
      output+="${text_light_gray}\n  - Base Ref : ${text_dark_red}$base_ref${reset}"
      summary+="\n  ${text_red}Invalid Base Ref ($base_ref)"
      is_error=1
    else
      output+="$\n  - Base Ref : $base_ref${reset}"
    fi
    
    if [[ "$is_error" == "0" ]]; then
      head_branch_name=${head_ref#refs/heads/}
      base_branch_name=${base_ref#refs/heads/}
      output+="${text_light_gray}\n  - Head Branch : $head_branch_name${reset}"
      output+="${text_light_gray}\n  - Base Branch : $base_branch_name${reset}"
      output+="${text_light_gray}\n  $head_branch_name -> $base_branch_name${reset}"
      summary="${background_dark_green}${summary}${text_green} : $head_branch_name -> $base_branch_name${reset}"
    else
      summary="${background_dark_red}${summary}${reset}"
    fi
    summary+="\n"
    ;;
  *)
    output=" ${text_light_gray}Event : ${text_dark_red}$event_name${reset}\n"
    output+="${text_dark_red}Unknown Event Type${reset}"
    summary="${background_dark_red}${text_red}Unknown Event Type ($base_ref) ${reset}"
    is_error=1
    ;;
esac


result="$summary$output"

if [ -z "$result_file" ]; then
  echo -e "$result"
else
  echo -e "$result" > "$result_file"
fi

if [ $is_error != 0]; then
  exit 1
fi