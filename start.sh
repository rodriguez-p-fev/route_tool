#!/bin/bash  
# Start Bash script  
sudo docker build -t route_tool .
sudo docker run -it --name route_tool -v tiles_cache:/tiles_cache route_tool
