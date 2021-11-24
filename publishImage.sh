#!/usr/bin/env bash
step=2
while getopts u:n:f: flag
do
    case "${flag}" in
        u) userAccount=${OPTARG};;
        n) imageName=${OPTARG};;
        f) dockerFilePath=${OPTARG};;
        *)
    esac
done

echo "Step 1/$step: Build docker image for $imageName 🔨🔨🔨..."
docker build -f "$dockerFilePath" -t "$userAccount"/"$imageName" .
if [ $? -ne 0 ]; then
    echo "Fail!"
    exit 1;
fi

echo "Step 2/$step: Push to docker hub 🚀🚀🚀..."
docker push "$userAccount"/"$imageName"

echo "DONE ✅✅✅"