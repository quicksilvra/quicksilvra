#!/bin/bash

# Perform a fetch with prune to remove deleted remote branches
git fetch --prune

# Find all local branches that are tracking a remote branch that is now gone
branches_to_delete=$(git branch -vv | awk '/: gone]/{print $1}')

# Force-delete orphaned local branches
for branch in $branches_to_delete; do
    # Skip the currently checked-out branch (HEAD)
    if [[ "$branch" != "$(git branch --show-current)" ]]; then
        echo "Deleting local branch: $branch"
        git branch -D "$branch"
    else
        echo "Skipping current branch: $branch"
    fi
done