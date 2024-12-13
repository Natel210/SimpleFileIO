#!/bin/bash

ref="${1:-unknown_ref}"

if [[ "$ref" == refs/heads/* ]]; then
  branch_name=${ref#refs/heads/}
  echo "::group::  - Info (Push - branch : $branch_name)"
  echo "    > Ref: $ref"
  echo "    > Branch Name: $branch_name"
  echo "::endgroup::"
  echo "::info::Push Branch: $branch_name"

elif [[ "$ref" == refs/tags/* ]]; then
  tag_name=${ref#refs/tags/}
  echo "::group::  - Info (Push - tag : $tag_name)"
  echo "    > Ref: $ref"
  echo "    > Tag Name: $tag_name"
  echo "::endgroup::"
  echo "::info::Push Tag: $tag_name"

else
  echo "::group::  - Info (Push - Unknown)"
  echo "    > Ref: $ref"
  echo "::endgroup::"
  echo "::error::Unknown push reference: $ref"
  exit 1

fi
