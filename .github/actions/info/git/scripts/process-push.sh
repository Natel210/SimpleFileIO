#!/bin/bash


ref="${1:-unknown_ref}"

if [[ "$ref" == refs/heads/* ]]; then
  branch_name=${ref#refs/heads/}
  echo -e "::group::Info (branch : \033[34m$branch_name\033[0m)"
  echo "> Ref: $ref"
  echo -e "> Branch Name: \033[34m$branch_name\033[0m"
  echo "::endgroup::"
  
elif [[ "$ref" == refs/tags/* ]]; then
  tag_name=${ref#refs/tags/}
  echo -e "::group::Info (tag : \033[34m$tag_name\033[0m)"
  echo "> Ref: $ref"
  echo -e "> Tag Name: \033[34m$tag_name\033[0m"
  echo "::endgroup::"
else
  echo -e "::group::Info (\033[31mUnknown\033[0m)"
  echo "> Ref: $ref"
  echo "::endgroup::"
  exit 1
fi
