#!/usr/bin/env bash

set -eo pipefail
SCRIPT_DIR=$(cd "$( dirname "${BASH_SOURCE[0]}" )" && pwd)

# Install .Net
. "./tools/Install-DotNet.sh" $SCRIPT_DIR

# Restore workloads
echo "Restoring all workloads"
"$DOTNET_EXE" workload restore macos -v:quiet
PROJECTS=$(find ./src/ -type f \( -name "*Mac.csproj" -o -iname "*Windows.csproj" -o -name "*iOS.csproj" -o -name "*Android.csproj" -o -name "*Linux.csproj" \) -print )
for PROJECT_FILE in $PROJECTS
do
    echo "Restoring workload for $PROJECT_FILE..."
    "$DOTNET_EXE" workload restore -v:quiet --project $PROJECT_FILE
done
echo "Done."
echo "---------------------------------------"

# Restore NuGet solution dependencies
echo "Restoring all dependencies"
SOLUTIONS=$(find ./src/ -iname "*.sln" -print)
for SOLUTION_FILE in $SOLUTIONS
do
    echo "Restoring packages for $SOLUTION_FILE..."
    "$DOTNET_EXE" restore -v:quiet $SOLUTION_FILE
done
echo "Done."
echo "---------------------------------------"
