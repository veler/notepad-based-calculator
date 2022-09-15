#!/usr/bin/env bash

set -eo pipefail
SCRIPT_DIR=$(cd "$( dirname "${BASH_SOURCE[0]}" )" && pwd)

# Install .Net
. "./tools/Install-DotNet.sh" $SCRIPT_DIR

# Restore NuGet solution dependencies
echo "Restoring all dependencies"
SOLUTIONS=$(find . -iname "*.sln" -print)
for SOLUTION_FILE in $SOLUTIONS
do
    echo "Restoring packages for $SOLUTION_FILE..."
    "$DOTNET_EXE" restore -v:quiet $SOLUTION_FILE
done

echo "Done."
echo "---------------------------------------"
