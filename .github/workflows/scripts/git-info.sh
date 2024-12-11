if [[ "${{ github.event_name }}" == "push" ]]; then
    echo "Event: Push"

    # Push Event: Extract Branch or Tag Information
    ref=${{ github.ref }}
    if [[ "$ref" == refs/heads/* ]]; then
        branch_name=${ref#refs/heads/}
        tag_name="N/A"
    elif [[ "$ref" == refs/tags/* ]]; then
        branch_name="N/A"
        tag_name=${ref#refs/tags/}
    else
        echo "Unknown ref type for push: $ref"
        exit 1
    fi

    # Commit Information
    commit_hash=$(git rev-parse HEAD)
    commit_message=$(git log -1 --pretty=format:%s)
    commit_author=$(git log -1 --pretty=format:'%an <%ae>')

    # Output
    echo "Branch Name      : $branch_name"
    echo "Tag Name         : $tag_name"
    echo "Commit Hash      : $commit_hash"
    echo "Commit Message   : $commit_message"
    echo "Author           : $commit_author"

elif [[ "${{ github.event_name }}" == "pull_request" ]]; then
  echo "Event: Pull Request"
          
  # Pull Request Event: Extract Source and Target Branch Information
  source_branch=${{ github.head_ref }}
  target_branch=${{ github.base_ref }}
          
  # Commit Information
  commit_hash=$(git rev-parse HEAD)
  commit_message=$(git log -1 --pretty=format:%s)
  commit_author=$(git log -1 --pretty=format:'%an <%ae>')
          
  # Output
  echo "Source Branch Name : $source_branch"
  echo "Target Branch Name : $target_branch"
  echo "Commit Hash        : $commit_hash"
  echo "Commit Message     : $commit_message"
  echo "Author             : $commit_author"

else
  echo "Unknown event type: ${{ github.event_name }}"
  exit 1
  
fi