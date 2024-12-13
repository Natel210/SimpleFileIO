#!/bin/bash

if [ "$#" -lt 1 ]; then
  echo "::error::No arguments. \n Usage: $0 <file1> [file2] [file3] ..."
  exit 1
fi

is_miss_files=0

for file in "$@"; do
  if [ ! -f "$file" ]; then
    is_miss_files=1
  fi
done

if [ $is_miss_files != 0 ]; then
  echo -e "::warning::One or more files are missing."
else
  echo -e "::notice::\[\033[0;32m\]All files Exist.\[\033[0m\]"
fi

echo -e "::group::Files List"
for file in "$@"; do
  if [ -f "$file" ]; then
    echo -e "> $file : \[\033[0;34m\]Exist\[\033[0m\]"
  else
    echo -e "> $file : \[\033[0;31m\]Not Exist\[\033[0m\]"
  fi
done
echo "::endgroup::"