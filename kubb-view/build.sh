#!/usr/bin/env sh

# Building project
npm run build

# Clean Static folder in kAPI
rm -rf ../kubb-api/Static/*

# Copy files in Kapi Static folder
cp dist/index.html ../kubb-api/Static/
cp dist/assets/* ../kubb-api/Static/