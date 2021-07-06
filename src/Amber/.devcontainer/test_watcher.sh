#!/bin/bash

dogo() {
       #wire ./internal/serviceLocator
       echo "###########################################"
       echo "Running tests in $1"
       go fmt
       go generate
       gotest -v $1
       echo "###########################################"
}



inotifywait --exclude "[^g].$|[^o]$" -m -r -e close_write ./ |
    while read path action file; do
           dogo $path
    done
