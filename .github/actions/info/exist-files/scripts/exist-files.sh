#!/bin/bash

# Check if at least one argument is provided
if [ "$#" -lt 1 ]; then
  echo "::error::No arguments provided. \n Usage: $0 <file1> [file2] [file3] ..."
  exit 1
fi

is_miss_files=0

for file in "$@"; do
  if [ -f "$file" ]; then
  else
    is_miss_files=1
  fi
done

if [ $is_miss_files != 0 ]; then
  echo -e "::warning::One or more files are missing."
else
  echo -e "::notice::\033[32mAll files are present.\033[0m"
fi

echo -e "::group::Check Files List"
for file in "$@"; do
  if [ -f "$file" ]; then
    echo "> $file : Exist"
  else
    echo -e "> $file : \033[31mNot Exist\033[0m"
  fi
done
echo "::endgroup::"