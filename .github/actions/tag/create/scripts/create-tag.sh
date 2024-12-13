#!/bin/bash

# Header 표시
echo -e "\033[36m● Create Tag ---------------------------\033[0m"

# 이전 태그 리스트 출력
echo "::group::Before All Tags"
echo "All Tags:"
git tag | sort -V
highest_tag_before=$(git tag | grep -E '^v[0-9]+\.[0-9]+\.[0-9]+$' | sort -V | tail -n 1 || echo "v0.0.0")
echo "Highest Tag (Before): $highest_tag_before"
echo "::endgroup::"

# Fetch remote tags to ensure we have the latest state
echo "Fetching the latest tags..."
git fetch --tags

# 모든 태그를 가져와 가장 높은 버전을 찾음
highest_tag=$(git tag | grep -E '^v[0-9]+\.[0-9]+\.[0-9]+$' | sort -V | tail -n 1 || echo "v0.0.0")
echo "Highest Tag Found: $highest_tag"

# 새로운 태그를 계산
if [[ "$highest_tag" =~ ^v([0-9]+)\.([0-9]+)\.([0-9]+)$ ]]; then
  major=${BASH_REMATCH[1]}
  minor=${BASH_REMATCH[2]}
  patch=${BASH_REMATCH[3]}
  # 패치 버전을 증가시킴
  new_patch=$((patch + 1))
  new_tag="v$major.$minor.$new_patch"
else
  # 기본값 설정
  new_tag="v0.0.1"
fi

echo "New Tag: $new_tag"

# 새 태그를 생성하고 원격 저장소에 푸시
if git tag "$new_tag"; then
  if git push origin "$new_tag"; then
    echo "New tag $new_tag created and pushed successfully."
  else
    echo "Error: Failed to push the new tag $new_tag to the remote repository." >&2
    exit 1
  fi
else
  echo "Error: Failed to create the new tag $new_tag locally." >&2
  exit 1
fi

# 이후 태그 리스트 출력
echo "::group::After All Tags"
echo "All Tags:"
git tag | sort -V
highest_tag_after=$(git tag | grep -E '^v[0-9]+\.[0-9]+\.[0-9]+$' | sort -V | tail -n 1 || echo "v0.0.0")
echo "Highest Tag (After): $highest_tag_after"
echo "::endgroup::"

# Footer 표시
echo -e "\033[36m----------------------------------------\033[0m"
