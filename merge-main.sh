#!/bin/bash

CURRENT_BRANCH=$(git rev-parse --abbrev-ref HEAD)
echo "Current branch: $CURRENT_BRANCH"

git checkout main || { echo "Error: can't checkout main"; exit 1; }

echo "Pulling main..."
git pull || { echo "Error: impossible to pull main"; exit 1; }

git checkout "$CURRENT_BRANCH" || { echo "Error: impossible checkout $CURRENT_BRANCH"; exit 1; }

echo "Pulling $CURRENT_BRANCH..."
git pull || { echo "Error: impossible to pull $CURRENT_BRANCH"; exit 1; }

echo "Merging main in $CURRENT_BRANCH..."
git merge main || { echo "Error: impossible to merge main in $CURRENT_BRANCH"; exit 1; }

echo "Operation completed!"
