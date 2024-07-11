import numpy as np
import pandas as pd
import requests 
import time
import os
from Config import cfg
from Tools import removeGPXFiles, Haversine, getRandomLocation
from HERErequest import  getTiles, getChargingStationsList, createAttrLists, updateAttrList, getLinksFromTile
from HEREgraph2 import HEREgraph, graphFromTileList, graphFromDict
from Route import Route, getSigns
from resources import feature_dict
import tracemalloc


session = requests.Session()
#UPDATED_CODE_01062022

N_ROUTES = cfg.get('routes_number')
ROUTES_LIMIT = 15
if(N_ROUTES > ROUTES_LIMIT):
    N_ROUTES = ROUTES_LIMIT
s_tiles = getTiles(cfg.get('gps_locations'),13, 13)
#chargingStations = getChargingStationsList(s_tiles, session)
chargingStations = []
def createCSVFile():
    features_file_name = f"./temp/summary.csv"
    head = ",".join([str(item) for item in feature_dict])
    head = head +',functional_class_1_time'+',functional_class_2_time'+',functional_class_3_time'+',functional_class_4_time'+',functional_class_5_time'
    features_file = open(features_file_name, "w")
    features_file.write("route_length,route_estimated_time(hrs),"+head+"\n")
    features_file.close()

if __name__ == '__main__':
    tracemalloc.start()
    start_time = time.time()
    
    removeGPXFiles("./temp/")
    removeGPXFiles("./gpx/")
    createCSVFile()
    session = requests.Session()


    start_time_01 = time.time()
    tiles = getTiles(cfg.get('gps_locations'),9, 13)
    end_time_01 = time.time()
    tiles_time = end_time_01 - start_time_01
    print("Get tiles time: ",tiles_time/3600)

    start_time_02 = time.time()
    if not session: session = requests.Session()
    attr_list = createAttrLists()
    
    #g = graphFromTileList(tiles, cfg, session) 
    ng = HEREgraph()
    for tile in tiles:
        links, aux_attr_list = getLinksFromTile(tile, cfg, session)
        attr_list = updateAttrList(attr_list, aux_attr_list)
        new_graph = graphFromDict(links)
        ng.updateGraph(new_graph)

    ng.saveEdgesToNumpy()
    ng.saveNodesToNumpy()
    end_time_02 = time.time()
    graph_time = end_time_02 - start_time_02
    print("Graph time: ",graph_time/3600.0)
    print('Roundabout: ',len(attr_list['roundabout']))
    print('Manoeuvre: ',len(attr_list['manoeuvre']))
    print('Ramps: ',len(attr_list['ramp']))
    print('Tunnels: ',len(attr_list['tunnel']))
    print('Bridges: ',len(attr_list['bridge']))
    print('traffic_lights: ',len(attr_list['traffic_lights']))
    print('traffic_signs: ',len(attr_list['traffic_signs']))
    
    start_time_03 = time.time()
    start_node, _ = ng.findNodeFromCoord(cfg.get('start_location'))

    if(cfg.get('route_type') == 'point_to_anywhere'):
        end_loc = getRandomLocation(cfg.get('start_location'), cfg.get('desired_route_length'))
        end_node, _ = ng.findNodeFromCoord(end_loc)
    else:
        end_node, _ = ng.findNodeFromCoord(cfg.get('end_location'))
    mid_nodes = []
    for loc in cfg.get('mid_locations'):
        mid_n, _ = ng.findNodeFromCoord(loc)
        mid_nodes.append(mid_n)

    routes_list = list()
    
    for i in range(min(cfg.get('query_features')['boolean_features']['manoeuvre'],len(attr_list['manoeuvre']))):
        loc = (int(attr_list['manoeuvre'][i]['LAT'].split(',')[0])*0.00001,int(attr_list['manoeuvre'][i]['LON'].split(',')[0])*0.00001)
        mid_n, _ = ng.findNodeFromCoord(loc)
        mid_nodes.append(mid_n)
    for i in range(min(cfg.get('query_features')['boolean_features']['roundabout'],len(attr_list['roundabout']))):
        loc = (int(attr_list['roundabout'][i]['LAT'].split(',')[0])*0.00001,int(attr_list['roundabout'][i]['LON'].split(',')[0])*0.00001)
        mid_n, _ = ng.findNodeFromCoord(loc)
        mid_nodes.append(mid_n)
    for i in range(min(cfg.get('query_features')['boolean_features']['ramp'],len(attr_list['ramp']))):
        loc = (int(attr_list['ramp'][i]['LAT'].split(',')[0])*0.00001,int(attr_list['ramp'][i]['LON'].split(',')[0])*0.00001)
        mid_n, _ = ng.findNodeFromCoord(loc)
        mid_nodes.append(mid_n)
    for i in range(min(cfg.get('query_features')['boolean_features']['traffic_sign'],len(attr_list['traffic_signs']))):
        loc = (int(attr_list['traffic_signs'][i]['LAT'].split(',')[0])*0.00001,int(attr_list['traffic_signs'][i]['LON'].split(',')[0])*0.00001)
        mid_n, _ = ng.findNodeFromCoord(loc)
        mid_nodes.append(mid_n)
    for i in range(min(cfg.get('query_features')['boolean_features']['tunnel'],len(attr_list['tunnel']))):
        loc = (int(attr_list['tunnel'][i]['LAT'].split(',')[0])*0.00001,int(attr_list['tunnel'][i]['LON'].split(',')[0])*0.00001)
        mid_n, _ = ng.findNodeFromCoord(loc)
        mid_nodes.append(mid_n)
    for i in range(min(cfg.get('query_features')['boolean_features']['bridge'],len(attr_list['bridge']))):
        loc = (int(attr_list['bridge'][i]['LAT'].split(',')[0])*0.00001,int(attr_list['bridge'][i]['LON'].split(',')[0])*0.00001)
        mid_n, _ = ng.findNodeFromCoord(loc)
        mid_nodes.append(mid_n)

    dtype = [('route', int), ('points', int)]
    values = []
    i = 0
    while(i < N_ROUTES):
        route_bool = False
        route = Route(cfg.get('desired_route_length_km'),chargingStations, int(cfg.get('visit_charge_station')))
        while(route_bool == False):
            #try:
            route.setRoute(ng, start_node, end_node, mid_nodes)
            route_bool = True
            #except:
            #    print("except route")
            #    end_loc = getRandomLocation(cfg.get('start_location'), cfg.get('desired_route_length'))
            #    end_node, _ = g.findNodeFromCoord(end_loc)
        
        route.setGPXFile(ng, i, "./temp", cfg)       
        route.setCSVFeatures(ng, i, units=cfg.get('units'))
        rank_points = route.getRankPoints()
        values.append((i,rank_points))
        i+=1

    routes_ranking_points = np.array(values, dtype=dtype)
    routes_ranking_points = np.sort(routes_ranking_points, order='points')  
    summary_file = open(f'./temp/summary.csv', "r")
    summary =  open(f'./gpx/summary.csv', "w")
    lines = summary_file.readlines()
    lines_array = []
    for line in lines:
        lines_array.append(line)
    summary_file.close()
    summary.write('route_num,'+lines_array[0])
    for i in range(N_ROUTES):
        n = routes_ranking_points[len(routes_ranking_points)-i-1][0]
        summary.write(str(i)+','+lines_array[n+1])
        csv_file = open(f'./temp/route{n}_staticfeaturesfile.csv', "r")
        f =  open(f'./gpx/route{i}_staticfeaturesfile.csv', "w")
        f.write(csv_file.read())
        f.close()
        csv_file.close()

        gpx_file = open(f'./temp/route{n}_staticfeaturesfile.gpx', "r")
        f =  open(f'./gpx/route{i}_staticfeaturesfile.gpx', "w")
        f.write(gpx_file.read())
        f.close()
        gpx_file.close()
    summary.close()

    


    end_time_03 = time.time()
  
    end_time = time.time()
    total_time = end_time - start_time
    
    
    route_time = end_time_03 - start_time_03
    tracemalloc.stop()

