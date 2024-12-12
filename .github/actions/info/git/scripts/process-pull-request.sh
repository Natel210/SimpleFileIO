#!/bin/bash

head_ref="${1:-unknown_head_ref}"
base_ref="${2:-unknown_base_ref}"

if [[ "$head_ref" == refs/heads/* ]]; then
  echo "::group::Push Info (Invalid Source)"
  echo "> Source Ref : $head_ref"
  echo "> Target Ref : $base_ref"
  echo "::endgroup::"
  exit 1
fi

if [[ "$base_ref" == refs/heads/* ]]; then
  echo "::group::Push Info (Invalid Target)"
  echo "> Source Ref : $head_ref"
  echo "> Target Ref : $base_ref"
  echo "::endgroup::"
  exit 1
fi

source_branch_name=${head_ref#refs/heads/}
target_branch_name=${base_ref#refs/heads/}

echo "::group::Push Info ($source_branch_name -> $target_branch_name)"
echo "> Source Branch: $source_branch_name"
echo "> Target Branch: $target_branch_name"
echo "> Source Ref : $head_ref"
echo "> Target Ref : $base_ref"
echo "::endgroup::"