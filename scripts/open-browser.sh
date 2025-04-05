#!/bin/bash

# Default browser configuration
BRAVE_APP_NAME="Brave Browser"
DEFAULT_BROWSER="$BRAVE_APP_NAME"

# Function to check if Brave is installed
check_brave() {
    if [ "$(uname)" == "Darwin" ]; then
        # macOS
        if [ -d "/Applications/Brave Browser.app" ]; then
            return 0
        fi
    elif [ "$(expr substr $(uname -s) 1 5)" == "Linux" ]; then
        # Linux
        if command -v brave-browser &> /dev/null; then
            return 0
        fi
    elif [ "$(expr substr $(uname -s) 1 10)" == "MINGW32_NT" ] || [ "$(expr substr $(uname -s) 1 10)" == "MINGW64_NT" ]; then
        # Windows
        if [ -f "/c/Program Files/BraveSoftware/Brave-Browser/Application/brave.exe" ]; then
            return 0
        fi
    fi
    return 1
}

# Function to open URL in browser
open_url() {
    local url="$1"
    
    if [ "$(uname)" == "Darwin" ]; then
        # macOS
        open -a "$DEFAULT_BROWSER" "$url"
    elif [ "$(expr substr $(uname -s) 1 5)" == "Linux" ]; then
        # Linux
        xdg-open "$url"
    elif [ "$(expr substr $(uname -s) 1 10)" == "MINGW32_NT" ] || [ "$(expr substr $(uname -s) 1 10)" == "MINGW64_NT" ]; then
        # Windows
        start brave "$url"
    else
        echo "Unsupported operating system"
        exit 1
    fi
}

# Main execution
if [ "$#" -ne 1 ]; then
    echo "Usage: $0 <url_or_file>"
    exit 1
fi

if ! check_brave; then
    echo "Brave browser is not installed"
    exit 1
fi

open_url "$1"