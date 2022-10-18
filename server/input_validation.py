import os
from os.path import isfile
from configparser import ConfigParser

config_ini_file_name = 'config.ini'
input_file_name = 'request_input.txt'
default_dict = {
    'route_type': 'point_to_point', 
    'start_gps': '', 
    'end_gps': '',
    'number_of_routes':'1',
    'units': 'km',
    'desired_route_length': '10.0', 
    'visit_charge_station': '0', 
    'highway': '0', 
    'avoid_highway': '0', 
    'urban': '0', 
    'oneway': '0', 
    'both_ways': '0', 
    'limited_access': '0', 
    'not_paved': '0', 
    'ramp': '0', 
    'manoeuvre': '0', 
    'roundabout': '0',
    'one_lane': '0', 
    'multiple_lanes': '0', 
    'speed_130km_80mph':'0',
    'speed_101kph_to_130kph_65_to_80mph':'0',
    'speed_91kph_to_100kph_55mph_to_64mph':'0',
    'speed_71kph_to_90kph_41mph_to_54mph':'0',
    'speed_51kph_to_70kph_31mph_to_40mph':'0',
    'speed_31kph_to_50kph_21mph_to_30mph':'0',
    'speed_11kph_to_30kph_6mph_to_20mph':'0',
    'speed_11kph_6mph':'0',
    'traffic_lights': '0', 
    'railway_crossing_signs': '0', 
    'no_overtaking_signs': '0', 
    'traffic_signs': '0', 
    'stop_signs': '0', 
    'school_zone_signs': '0', 
    'icy_road_signs': '0', 
    'crosswalk_signs': '0', 
    'animal_crossing_signs': '0', 
    'lane_merge_right_signs': '0', 
    'lane_merge_left_signs': '0', 
    'falling_rocks_signs': '0',
    'hills_signs': '0', 
    'tunnel': '0', 
    'bridge': '0',
    'road_good':'0',
    'road_fair':'0',
    'road_poor':'0',
    'speed_bumps':'0',
    'toll_station':'0',
    'lane_marker_long_dashed' : '0',
    'lane_marker_short_dashed' : '0',
    'lane_marker_double_dashed' : '0',
    'lane_marker_double_solid' : '0',
    'lane_marker_single_solid' : '0',
    'lane_marker_inner_solid_outter_dashed' : '0',
    'lane_marker_inner_dashed_outter_solid' : '0',
    'lane_marker_no_divider' : '0',
    'lane_marker_physical_divider' : '0',  
    'hov_lane':'0',
    'reversible_lane':'0',
    'express_lane':'0',
    'slow_lane':'0',
    'auxiliary_lane':'0',
    'shoulder_lane':'0',
    'passing_lane':'0',
    'turn_lane':'0',
    'parking_lane':'0',
    'center_turn_lane':'0',
    'parking_lot':'0'
    }

if not isfile(os.path.join(os.getcwd(), input_file_name)):
    print("input file doesn't exist")
else:
    with open(input_file_name,'r') as input_file:
        input = input_file.readlines()
    for i in range(1,len(input)):
        if(input[i] != "\n"):
            print(input[i])
            line = input[i].split("=")
            var = line[0].replace(" ", "").lower()
            val = line[1].replace(" ", "").replace("\n","")
            default_dict[var] = val
    input_file.close()

f = open(config_ini_file_name, "w")
f.write("[config]\n")
for k in default_dict.keys():
    print(k,default_dict[k])
    f.write(k+"="+default_dict[k]+"\n")
f.close()