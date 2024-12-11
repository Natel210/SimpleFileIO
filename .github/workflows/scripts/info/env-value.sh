#!/bin/bash

event_name="${1:-unknown_event}"
ref="${2:-unknown_ref}"
head_ref="${3:-unknown_head_ref}"
base_ref="${4:-unknown_base_ref}"
solution_file="${5:-unknown_solution_file}"
dll_file="${6:-unknown_dll_file}"
tester_file="${7:-unknown_tester_file}"


echo "Setting environment variables for Git Info Script."
echo "EVNET: $event_name"
echo "REF: $ref"
echo "HEAD_REF: $head_ref"
echo "BASE_REF: $base_ref"
echo "SOLUTION_FILE: $solution_file"
echo "DLL_FILE: $dll_file"
echo "TESTER_FILE: $tester_file"