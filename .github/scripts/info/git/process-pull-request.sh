#!/bin/bash

head_ref="${1:-unknown_head_ref}"
base_ref="${2:-unknown_base_ref}"

echo "Pull Request Event"
echo "Source Branch: $head_ref"
echo "Target Branch: $base_ref"
