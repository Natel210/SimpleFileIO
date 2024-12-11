#!/bin/bash

# Check if the correct number of arguments is provided
if [ "$#" -ne 3 ]; then
  echo "Error: Invalid number of arguments."
  echo "Usage: $0 <solution_file> <dll_file> <tester_file>"
  exit 1
fi

# Read paths from command-line arguments
solution_file="$1"
dll_file="$2"
tester_file="$3"

# Initialize flags
error_flag=0

echo "Check Files"

# Check solution file
if [ -f "$solution_file" ]; then
  echo "> Solution : $solution_file"
else
  echo "Error: $solution_file not found"
  error_flag=1
fi

# Check DLL file
if [ -f "$dll_file" ]; then
  echo "> DLL : $dll_file"
else
  echo "Error: $dll_file not found"
  error_flag=1
fi

# Check Tester project file
if [ -f "$tester_file" ]; then
  echo "> Tester : $tester_file"
else
  echo "Error: $tester_file not found"
  error_flag=1
fi

echo ""
echo "Result"

# Exit with error if any required file is missing
if [ $error_flag -eq 1 ]; then
  echo "> One or more required files are missing."
  exit 1
else
  echo "> All required files are present."
fi