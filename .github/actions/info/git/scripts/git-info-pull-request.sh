#!/bin/bash

head_ref="${1:-unknown_head_ref}"
base_ref="${2:-unknown_base_ref}"

if [[ "$head_ref" == refs/heads/* ]]; then
  echo -e "::error::\033[32mPull Request\033[0m - \033[31mInvalid Source\033[0m"
  echo "::group::Info"
  echo -e "> Source Ref : \033[31m$head_ref\033[0m"
  echo "> Target Ref : $base_ref"
  echo "::endgroup::"
  exit 1
fi

if [[ "$base_ref" == refs/heads/* ]]; then
  echo -e "::error::\033[32mPull Request\033[0m - \033[31mInvalid Target\033[0m"
  echo -e "::group::Info"
  echo "> Source Ref : $head_ref"
  echo -e "> Target Ref : \033[31m$base_ref\033[0m"
  echo "::endgroup::"
  exit 1
fi

source_branch_name=${head_ref#refs/heads/}
target_branch_name=${base_ref#refs/heads/}
echo "::notice::\033[32mPull Request\033[0m - $source_branch_name -> $target_branch_name"
echo "::group::Info (\033[32mPull Request\033[0m - $source_branch_name -> $target_branch_name)"
echo "> Source Branch: $source_branch_name"
echo "> Target Branch: $target_branch_name"
echo "> Source Ref : $head_ref"
echo "> Target Ref : $base_ref"
echo "::endgroup::"