:; set -eo pipefail
:; SCRIPT_DIR=$(cd "$( dirname "${BASH_SOURCE[0]}" )" && pwd)
:; ${SCRIPT_DIR}/init.sh "$@"
:; exit $?

@ECHO OFF
powershell -ExecutionPolicy ByPass -NoProfile -File "%~dp0init.ps1" %*
