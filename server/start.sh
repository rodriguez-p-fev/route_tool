#!/bin/bash  
# Start Bash script  
docker build -t route_tool .
docker run -itd --name route_tool -v tiles_cache:/tiles_cache route_tool
