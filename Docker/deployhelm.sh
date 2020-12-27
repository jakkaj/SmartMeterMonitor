#!/bin/bash

helm template -f ./Helm/SmartSystemPods/values.yaml ./Helm/SmartSystemPods | kubectl apply -f -

