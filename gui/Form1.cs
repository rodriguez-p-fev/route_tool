using GMap.NET;
using GMap.NET.WindowsForms;
using System.Text;
using System.Xml;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;
using GoogleMapsClient;
using GoogleMaps.LocationServices;
using System.Configuration;
using System.Device.Location;

namespace FEV_routingtool
{
    public partial class Form1 : Form
    {
        //string API_KEY_G = "AIzaSyCDVo1GPeRzuJA8QOTjl3UvpzIB18nbq_M";     //Markus
        string API_KEY_G = "AIzaSyBwW5IM9GWwC5fPgKFYfMD6OcGd74AR-xoEEEE";       //Qusay
        //Global Variables
        //var gls = new GoogleLocationService(API_KEY_G);
        //var reversedAddress = gls.GetAddressFromLatLang(lat, lon);
        int mapclickedindex = -1; //0: start point   1: end point   2: mid point
        PointLatLng points_first = new PointLatLng();                   //Start Point
        PointLatLng points_last = new PointLatLng();                    //End Point
        GMap.NET.WindowsForms.GMapOverlay gMapOverlay_StartEnd;         //Google Map
        GMap.NET.WindowsForms.Markers.GMarkerGoogle gMarker_StartEnd;   //Google map marker
        //MQTT variables
        //const string MQTT_Topicname = "fevvf_route_tool";             //MQTT Topic Name
        string MQTT_Topicname = "fevvf/route_tool_karthik";             //MQTT Topic Name        
        MqttClient client;                                              //MQTT Client Object
        string clientId;                                                //MQTT ClientID
        static uint gpx_numberofFiles = 0;                              //Number of files recive from HERE python server
        static uint gpx_numberofRoutes = 0;                             //Number of routes recive from HERE python server
        static uint gpx_filesCounter = 0;                               //files counter
        static string gpx_nextfilename = "";                            //Next file name recive from HERE python server
        static string gpx_currentDirectoryname = "";                    //Directory name where gpx and csv failes are saved
        string RunningPath = "";// AppDomain.CurrentDomain.BaseDirectory;
        string configuartion_ini = "";
        bool usegoogle_api = false;

        string python_code_path = @"D:\VinFast\server.py";
        public Form1()
        {
            InitializeComponent();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                //TimeSpan t = DateTime.UtcNow - new DateTime(1970, 1, 1);
                //int secondsSinceEpoch = (int)t.TotalSeconds;
                //MessageBox.Show(secondsSinceEpoch.ToString());

                try
                {
                    using (XmlReader reader = XmlReader.Create("config_fev.xml"))
                    {
                        while (reader.Read())
                        {
                            if (reader.IsStartElement())
                            {
                                switch (reader.Name.ToString())
                                {
                                    case "GoogleMap_API_KEY":
                                        API_KEY_G = reader.ReadString();
                                        break;
                                    case "GoogleMap_API_USE":
                                        if (reader.ReadString() == "TRUE")
                                            usegoogle_api = true;
                                        else
                                            usegoogle_api = false;
                                        break;
                                    case "MQTT_Topic_Name":
                                        MQTT_Topicname = reader.ReadString();
                                        break;
                                    case "Python_Server_Path":
                                        python_code_path = reader.ReadString();
                                        break;
                                }
                            }
                        }
                    }
                }

                catch (Exception ex)
                {
                    MessageBox.Show("Configuration File Missing");
                }

                //try
                //{
                //    var tcp = new System.Net.Sockets.TcpClient("time.nist.gov", 13);
                //    string resp;
                //    using (var rdr = new StreamReader(tcp.GetStream()))
                //    {
                //        resp = rdr.ReadToEnd();
                //    }

                //    string utc = resp.Substring(7, 17);

                //    int year = int.Parse(resp.Substring(7, 2));
                //    int month = int.Parse(resp.Substring(10, 2));
                //    int day = int.Parse(resp.Substring(13, 2));
                //    if ((month == 8 && day >= 30) || month >= 9 || year >= 23)
                //    {
                //        MessageBox.Show("License Expired");
                //        System.Environment.Exit(0);
                //    }
                //    richTextBox_serverlogs.AppendText("UTC Time Now\t: " + utc + " \n");
                //    richTextBox_serverlogs.AppendText("License will Expire on Aug 30th \n");
                //}
                //catch (Exception ex)
                //{ 
                //    MessageBox.Show("License Validation Error");
                //    System.Environment.Exit(0);
                //}
                //Google map initialization
                //System.IO.Directory.CreateDirectory("cache");
                //GMap.NET.MapProviders.GMapProviders.GoogleMap.ApiKey = API_KEY_G;
                GMap.NET.GMaps.Instance.Mode = AccessMode.ServerAndCache;
                GMap.NET.GMaps.Instance.UseMemoryCache = true;
                gMapControl1.CacheLocation = @"cache";
                gMapControl1.MapProvider = GMap.NET.MapProviders.GMapProviders.GoogleMap;
                gMapControl1.DragButton = MouseButtons.Left;                
                gMapControl1.Position = new GMap.NET.PointLatLng(42.702468, -83.254997);
                gMapControl1.MinZoom = 0;
                gMapControl1.MaxZoom = 18;
                gMapControl1.Zoom = 10;

                //GUI controls initial values
                RunningPath = AppDomain.CurrentDomain.BaseDirectory;
                textBox_startpoint.Text = "42.702468, -83.254997";
                textBox_endpoint.Text = "42.665226, -83.193315";
                textBox_radius.Text = "10.0";
                textBox_desiredlength.Text = "60.0";
                comboBox_searchtype.SelectedIndex = 0;
                toolTip1.SetToolTip(textBox_startpoint, "Enter the route start point manually or right click on the map (green marker) (click inside the box first)");
                toolTip2.SetToolTip(textBox_endpoint, "Enter the route end point manually or right click on the map (red marker) (point to point type) (click inside the box first)");
                toolTip3.SetToolTip(textBox_radius, "Enter the radius from the start point to search for routes (point to anywhere type)");
                toolTip4.SetToolTip(textBox_desiredlength, "Enter the desired length for generated routes (point to anywhere type)");
                toolTip5.SetToolTip(comboBox_searchtype, "Select search type");
                String tmp_fetaureswithsign = "non_pedestrian_crossing\n" + "school_zone\n" + "variable_speed\n" + "traffic_signs\n" + "railway_crossing\n" + "no_overtaking\n" + "overtaking\n" + "falling_rocks\n" + "crosswalk\n" + "two_way\n" + "lane_merge_right\n" + "lane_merge_left\n" + "lane_merge_center\n" + "hills\n" + "tunnel\n" + "bridge\n" + "pedestrian\n" + "stop_signs\n" + "icy_road\n" + "traffic_lights";
                toolTip6.SetToolTip(groupBox1, "Select static features to be included in the generated routes following are features with signs\n" + tmp_fetaureswithsign);
                toolTip7.SetToolTip(button_generateroutes, "Generate Routes");
                toolTip8.SetToolTip(comboBox_numberofroutes, "Select Route");

                //Initial 2 markers drwan on the Google Map

                string FileName_tmp = string.Format("{0}icons\\start.png", Path.GetFullPath(Path.Combine(RunningPath)));
                gMapOverlay_StartEnd = new GMap.NET.WindowsForms.GMapOverlay("Overlay_StartEnd");
                //gMarker_StartEnd = new GMap.NET.WindowsForms.Markers.GMarkerGoogle(new GMap.NET.PointLatLng(42.702468, -83.254997), GMap.NET.WindowsForms.Markers.GMarkerGoogleType.green);
                gMarker_StartEnd = new GMap.NET.WindowsForms.Markers.GMarkerGoogle(new GMap.NET.PointLatLng(42.702468, -83.254997), Bitmap.FromFile(FileName_tmp) as Bitmap);
                gMarker_StartEnd.ToolTipText = "Point 1";
                gMapOverlay_StartEnd.Markers.Add(gMarker_StartEnd);

                FileName_tmp = string.Format("{0}icons\\end.png", Path.GetFullPath(Path.Combine(RunningPath)));
                //gMarker_StartEnd = new GMap.NET.WindowsForms.Markers.GMarkerGoogle(new GMap.NET.PointLatLng(42.665226, -83.193315), GMap.NET.WindowsForms.Markers.GMarkerGoogleType.red);
                gMarker_StartEnd = new GMap.NET.WindowsForms.Markers.GMarkerGoogle(new GMap.NET.PointLatLng(42.665226, -83.193315), Bitmap.FromFile(FileName_tmp) as Bitmap);
                gMarker_StartEnd.ToolTipText = "Point 2";
                gMapOverlay_StartEnd.Markers.Add(gMarker_StartEnd);
                gMapControl1.Overlays.Add(gMapOverlay_StartEnd);

                //MQTT initialization
                TimeSpan t = DateTime.UtcNow - new DateTime(1970, 1, 1);
                //string BrokerAddress = "34.130.9.249";                              
                string BrokerAddress = "broker.mqttdashboard.com";              //MQTT broker IP
                client = new MqttClient(BrokerAddress);
                clientId = "clientId-OQ"+ t.TotalSeconds.ToString();                           //MQTT Client ID
                client.MqttMsgPublishReceived += client_MqttMsgPublishReceived; //Receive MQTT message registration
                client.Connect(clientId);                                       //MQTT Connect to broker (Without Auth)
                client.Subscribe(new string[] { MQTT_Topicname }, new byte[] { 2 });


                /*string BrokerAddress = "e6238d8846024198be84bfb8255304bd.s1.eu.hivemq.cloud";
                clientId = "clientId-OQyW4Yz3Fq";
                client = new MqttClient(BrokerAddress, 8883, true, null, null, MqttSslProtocols.TLSv1_2);
                client.MqttMsgPublishReceived += client_MqttMsgPublishReceived;
                client.Connect(clientId, "fev_VF", "~m8Y[CgKnB");
                client.Subscribe(new string[] { MQTT_Topicname }, new byte[] { 2 });*/

                comboBox_unit.SelectedIndex = 0;
                comboBox_routecounts.SelectedIndex = 4;
                this.WindowState = FormWindowState.Maximized;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        
        private void gMapControl1_MouseClick(object sender, MouseEventArgs e)
        {
            try
            {
                if (e.Button == System.Windows.Forms.MouseButtons.Right)
                {
                    if (mapclickedindex < 0)
                        return;
                    double lat = gMapControl1.FromLocalToLatLng(e.X, e.Y).Lat;
                    double lon = gMapControl1.FromLocalToLatLng(e.X, e.Y).Lng;
                    GoogleMapsClient.GoogleMaps client = new GoogleMapsClient.GoogleMaps(API_KEY_G);    //Get address from GPS points
                    Address addr = client.QueryCoordinates(lat, lon);                                                                   //Get address from GPS points
                    if (mapclickedindex == 0)
                    {
                        textBox_startpoint.Text = lat.ToString("0.0000") + ", " + lon.ToString("0.0000") + "  [" + addr.FormattedAddress + "]";
                        string FileName_tmp = string.Format("{0}icons\\start.png", Path.GetFullPath(Path.Combine(RunningPath)));
                        gMarker_StartEnd = new GMap.NET.WindowsForms.Markers.GMarkerGoogle(new GMap.NET.PointLatLng(lat, lon), Bitmap.FromFile(FileName_tmp) as Bitmap);
                        //gMarker_StartEnd = new GMap.NET.WindowsForms.Markers.GMarkerGoogle(new GMap.NET.PointLatLng(lat, lon), GMap.NET.WindowsForms.Markers.GMarkerGoogleType.green);
                        gMarker_StartEnd.ToolTipText = "Start Point: " + textBox_startpoint.Text;
                        gMapOverlay_StartEnd.Markers[0] = gMarker_StartEnd;
                    }
                    else if (mapclickedindex == 1)
                    {
                        textBox_endpoint.Text = lat.ToString("0.0000") + ", " + lon.ToString("0.0000") + "  [" + addr.FormattedAddress + "]";
                        string FileName_tmp = string.Format("{0}icons\\end.png", Path.GetFullPath(Path.Combine(RunningPath)));
                        gMarker_StartEnd = new GMap.NET.WindowsForms.Markers.GMarkerGoogle(new GMap.NET.PointLatLng(lat, lon), Bitmap.FromFile(FileName_tmp) as Bitmap);
                        //gMarker_StartEnd = new GMap.NET.WindowsForms.Markers.GMarkerGoogle(new GMap.NET.PointLatLng(lat, lon), GMap.NET.WindowsForms.Markers.GMarkerGoogleType.red);
                        gMarker_StartEnd.ToolTipText = "End Point: " + textBox_endpoint.Text;
                        gMapOverlay_StartEnd.Markers[1] = gMarker_StartEnd;
                    }
                    else if (mapclickedindex == 2)
                    {
                        if (textBox_midpoints.Text.Trim() == "")
                            textBox_midpoints.Text = lat.ToString("0.0000") + ", " + lon.ToString("0.0000");
                        else
                            textBox_midpoints.Text = textBox_midpoints.Text + ", " + lat.ToString("0.0000") + ", " + lon.ToString("0.0000");
                    }
                    mapclickedindex = -1;
                    gMapControl1.Zoom += 1;
                    gMapControl1.Zoom -= 1;
                }
            }
            catch (Exception ex)
            {MessageBox.Show(ex.Message);}
        }

        private void button_generateroutes_Click(object sender, EventArgs e)
        {
            try
            {
                string start_gps = "";
                string end_gps = "";
                string mid_gps = "";
                string searchradius = "";
                string routedesiredlength = "";
                string route_type = "";
                string messagetobesent = "";
                string searchtype = "";
                /////Search Type/////////////////////////////////////////////////////////
                searchtype = this.comboBox_searchtype.GetItemText(this.comboBox_searchtype.SelectedItem);
                if (searchtype == "Point to Point")
                    route_type = "point_to_point";
                else if (searchtype == "Point to Anywhere")
                    route_type = "point_to_anywhere";
                else if (searchtype == "Closed Route")
                    route_type = "closed_route";
                else
                {
                    MessageBox.Show("Please select search type");
                    return;
                }
                /////Static Features/////////////////////////////////////////////////////
                for (int i = 0; i < listBox_dst.Items.Count; i++)
                {
                    if(listBox_dst.Items[i].ToString().Contains("Max_speed") || listBox_dst.Items[i].ToString().Contains("Min_speed"))
                        messagetobesent = messagetobesent + listBox_dst.Items[i].ToString().ToLower() + "\r\n";
                    else
                        messagetobesent = messagetobesent + listBox_dst.Items[i].ToString().ToLower() + " = 1\r\n";
                }
                for (int i = 0; i < listBox_src.Items.Count; i++)
                {
                    if (listBox_src.Items[i].ToString().Contains("Max_speed") || listBox_src.Items[i].ToString().Contains("Min_speed"))
                        continue;
                    else if (listBox_dst.Items.Contains(listBox_src.Items[i]))
                        continue;
                    else
                        messagetobesent = messagetobesent + listBox_src.Items[i].ToString().ToLower() + " = 0\r\n";
                }
                /////GPS Coordinates/////////////////////////////////////////////////////
                if (textBox_startpoint.Text.IndexOf("[") > 0)
                    start_gps = textBox_startpoint.Text.Substring(0, textBox_startpoint.Text.IndexOf("[") - 2);
                else
                    start_gps = textBox_startpoint.Text;
                if (textBox_endpoint.Text.IndexOf("[") > 0)
                    end_gps = textBox_endpoint.Text.Substring(0, textBox_endpoint.Text.IndexOf("[") - 2);
                else
                    end_gps = textBox_endpoint.Text;
                if (start_gps.Length < 4 || end_gps.Length < 4)
                {
                    MessageBox.Show("Please enter valid gps points");
                    return;
                }
                mid_gps = textBox_midpoints.Text.Trim();
                routedesiredlength = textBox_desiredlength.Text.TrimEnd();
                searchradius = textBox_radius.Text.TrimEnd();

                configuartion_ini = "[config]" + "\r\n";
                configuartion_ini += "start_gps=" + start_gps + "\r\n";
                string endpoints_combine = "";
                if (mid_gps == "")
                    endpoints_combine = end_gps;
                else
                    endpoints_combine = mid_gps + ", " + end_gps;
                /*switch (route_type)
                {
                    case "point_to_point":
                        configuartion_ini += "end_gps=" + endpoints_combine + "\r\n";
                        break;
                    case "closed_route":
                        configuartion_ini += "end_gps=" + endpoints_combine + ", " + start_gps + "\r\n";
                        break;
                    case "point_to_anywhere":
                        configuartion_ini += "end_gps=" + "\r\n";
                        break;
                }*/
                configuartion_ini += "end_gps=" + endpoints_combine + "\r\n";
                configuartion_ini += "number_of_routes=" + comboBox_routecounts.SelectedItem.ToString() + "\r\n";
                configuartion_ini += "units=" + comboBox_unit.SelectedItem.ToString() + "\r\n";
                configuartion_ini += "desired_route_length=" + searchradius + "\r\n";
                configuartion_ini += messagetobesent + "\r\n";
                //MessageBox.Show(configuartion_ini);
                gpx_currentDirectoryname = "";
                gpx_currentDirectoryname = DateTime.Now.Ticks.ToString();
                System.IO.Directory.CreateDirectory(gpx_currentDirectoryname);
                string tmpfilenameconfig = gpx_currentDirectoryname + "\\" + "config.txt";
                WriteGpxFileAsync(tmpfilenameconfig, configuartion_ini);
                richTextBox_serverlogs.AppendText("Request file saved to " + gpx_currentDirectoryname + " folder\n");

                client.Publish(MQTT_Topicname, Encoding.UTF8.GetBytes(configuartion_ini), MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, false);
                richTextBox_serverlogs.AppendText("\n\n----------------------\nMessage sent to HERE API\n" + DateTime.Now.ToString() + "\nWaiting for Response\n");

                points_first = new PointLatLng(double.Parse(start_gps.Substring(0, start_gps.IndexOf(","))), double.Parse(start_gps.Substring(start_gps.IndexOf(",") + 2)));
                points_last = new PointLatLng(double.Parse(end_gps.Substring(0, end_gps.IndexOf(","))), double.Parse(end_gps.Substring(end_gps.IndexOf(",") + 2)));
            }
            catch (Exception ex)
            { MessageBox.Show(ex.Message, "GPS Input Error"); }
        }

        void client_MqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs e)
        {
            try
            {
                string ReceivedMessage = Encoding.UTF8.GetString(e.Message);
                if (ReceivedMessage == "eof" || ReceivedMessage.Contains("[config]"))
                    return;
                if (!ReceivedMessage.Contains("Hello GUI am sending") && gpx_numberofFiles == 0)    //waiting for first message
                    return;
                if (ReceivedMessage.Contains("Hello GUI am sending"))                               //First message reveived
                {
                    gpx_numberofFiles = 0;
                    gpx_nextfilename = "";
                    gpx_numberofFiles = uint.Parse(ReceivedMessage.Substring(20, ReceivedMessage.IndexOf("files") - 20 - 1));

                    gpx_numberofRoutes = gpx_numberofFiles / 2;
                    richTextBox_serverlogs.Invoke((MethodInvoker)(() => richTextBox_serverlogs.AppendText("Response received from HERE Maps: \n" + DateTime.Now.ToString() + "\nNumber of generated Files = " + gpx_numberofFiles.ToString() + "\n")));
                    richTextBox_serverlogs.Invoke((MethodInvoker)(() => richTextBox_serverlogs.AppendText("Number of generated Routes = " + gpx_numberofRoutes.ToString() + "\n")));
                    comboBox_numberofroutes.Invoke((MethodInvoker)(() => comboBox_numberofroutes.Items.Clear()));
                    //label_generatedroute_count.Invoke((MethodInvoker)(() => label_generatedroute_count.Text = "Generated Routes Count = " + gpx_numberofRoutes.ToString()));
                    return;
                }
                if ((ReceivedMessage.Contains("_staticfeaturesfile.gpx") || ReceivedMessage.Contains("_staticfeaturesfile.csv") || ReceivedMessage.Contains("summary.csv"))
                    && !ReceivedMessage.Contains("version") && !ReceivedMessage.Contains("LAT,LON,Link_length")) //File name received
                {
                    richTextBox_serverlogs.Invoke((MethodInvoker)(() => richTextBox_serverlogs.AppendText(ReceivedMessage + "\n")));
                    gpx_nextfilename = ReceivedMessage;
                    if (ReceivedMessage.Contains("_staticfeaturesfile.gpx"))
                        comboBox_numberofroutes.Invoke((MethodInvoker)(() => comboBox_numberofroutes.Items.Add(ReceivedMessage.ToString())));
                    return;
                }

                if (!ReceivedMessage.Contains("Hello GUI am sending") 
                    && (ReceivedMessage.Contains("version") 
                    || ReceivedMessage.Contains("LAT,LON,Link_length") 
                    || ReceivedMessage.Contains("route_num,route_length")) 
                    && gpx_nextfilename != "") //GPX or csv file received
                {
                    string tmpfilename = gpx_currentDirectoryname + "\\" + gpx_nextfilename;
                    WriteGpxFileAsync(tmpfilename, ReceivedMessage);
                    richTextBox_serverlogs.Invoke((MethodInvoker)(() => richTextBox_serverlogs.AppendText(gpx_nextfilename + " saved to " + gpx_currentDirectoryname + " folder\n")));
                    gpx_nextfilename = "";
                    gpx_filesCounter++;
                }
            }
            catch (Exception ex)
            { MessageBox.Show(ex.Message, "Error"); }
        }

        private void ParseGPXSigns(string filename)
        {
            int counter = 0;
            bool onlycharging = true;
            double lat1 = 0.0, lon1 = 0.0, route_duration_tmp = 0.0;
            string outerxml = "", SignType = "";
            PointLatLng points_prev = new PointLatLng(), points_curr = new PointLatLng();
            GMap.NET.WindowsForms.Markers.GMarkerGoogle gMarkerGoogle_tmp = new GMap.NET.WindowsForms.Markers.GMarkerGoogle(new GMap.NET.PointLatLng(42.702468, -83.254997), GMap.NET.WindowsForms.Markers.GMarkerGoogleType.red);
            GMap.NET.WindowsForms.GMapOverlay gMapOverlay_Static_Faetures = new GMap.NET.WindowsForms.GMapOverlay("Overlay_StaticFeatures");
            GMap.NET.WindowsForms.GMapOverlay gMapOverlay_Static_Faetures_Route = new GMap.NET.WindowsForms.GMapOverlay("Overlay_StaticFeatures_Route");
            try
            {
                string FileName = string.Format("{0}icons\\stop2_33x33.png", Path.GetFullPath(RunningPath));
                Bitmap bitmap = Bitmap.FromFile(FileName) as Bitmap;
                XmlDocument gpxDoc = new XmlDocument();
                gpxDoc.Load(filename);
                XmlNamespaceManager nsmgr = new XmlNamespaceManager(gpxDoc.NameTable);
                nsmgr.AddNamespace("x", "http://www.topografix.com/GPX/1/1");
                XmlNodeList nl = gpxDoc.SelectNodes("//x:wpt", nsmgr);
                int tmptmpcounter = 0;
                foreach (XmlNode xnode in nl)
                {
                    counter++;
                    outerxml = xnode.OuterXml;
                    SignType = xnode.InnerText;
                    lat1 = Convert.ToDouble(outerxml.Substring(outerxml.IndexOf("lat") + 5, (outerxml.IndexOf("lon") - outerxml.IndexOf("lat") - 7)));
                    lon1 = Convert.ToDouble(outerxml.Substring(outerxml.IndexOf("lon") + 5, (outerxml.IndexOf("xmlns") - outerxml.IndexOf("lon") - 7)));
                    if (SignType.Contains("ChargePoint") || SignType.Contains("IEC") || SignType.Contains("Tesla"))
                    {
                        FileName = string.Format("{0}icons\\charge3.png", Path.GetFullPath(Path.Combine(RunningPath)));
                        gMarkerGoogle_tmp = new GMap.NET.WindowsForms.Markers.GMarkerGoogle(new GMap.NET.PointLatLng(lat1, lon1), Bitmap.FromFile(FileName) as Bitmap);
                        gMarkerGoogle_tmp.ToolTipText = SignType;
                        if (counter == nl.Count && points_curr.Lat != 0.0 && points_curr.Lng != 0.0 && points_prev.Lat != 0.0 && points_prev.Lng != 0.0)
                        {
                            GDirections gDirections;
                            GMap.NET.MapProviders.GoogleMapProvider.Instance.GetDirections(out gDirections, points_curr, points_last, false, false, false, true, false);
                            if (gDirections != null)
                                gMapOverlay_Static_Faetures_Route.Routes.Add(new GMapRoute(gDirections.Route, "MyRoute"));
                        }
                    }
                    else
                    {
                        onlycharging = false;
                        points_curr = new PointLatLng(lat1, lon1);
                        if (counter == 1)
                        {
                            GDirections gDirections;
                            GMap.NET.MapProviders.GoogleMapProvider.Instance.GetDirections(out gDirections, points_first, points_curr, false, false, false, true, false);
                            if (gDirections != null)
                            {
                                gMapOverlay_Static_Faetures_Route.Routes.Add(new GMapRoute(gDirections.Route, "MyRoute"));
                                //route_duration_tmp += double.Parse(gDirections.Duration.Substring(0, gDirections.Duration.IndexOf("min")));
                            }
                            if (counter == nl.Count)
                            {
                                GMap.NET.MapProviders.GoogleMapProvider.Instance.GetDirections(out gDirections, points_curr, points_last, false, false, false, true, false);
                                if (gDirections != null)
                                {
                                    gMapOverlay_Static_Faetures_Route.Routes.Add(new GMapRoute(gDirections.Route, "MyRoute"));
                                    //route_duration_tmp += double.Parse(gDirections.Duration.Substring(0, gDirections.Duration.IndexOf("min")));
                                }
                            }
                        }
                        else if (points_curr.Lat != 0.0 && points_curr.Lng != 0.0 && points_prev.Lat != 0.0 && points_prev.Lng != 0.0)
                        {
                            GDirections gDirections;
                            GMap.NET.MapProviders.GoogleMapProvider.Instance.GetDirections(out gDirections, points_prev, points_curr, false, false, false, true, false);
                            if (gDirections != null)
                            {
                                gMapOverlay_Static_Faetures_Route.Routes.Add(new GMapRoute(gDirections.Route, "MyRoute"));
                                //route_duration_tmp += double.Parse(gDirections.Duration.Substring(0, gDirections.Duration.IndexOf("min")));
                            }
                            if (counter == nl.Count)
                            {
                                GMap.NET.MapProviders.GoogleMapProvider.Instance.GetDirections(out gDirections, points_curr, points_last, false, false, false, true, false);
                                if (gDirections != null)
                                {
                                    gMapOverlay_Static_Faetures_Route.Routes.Add(new GMapRoute(gDirections.Route, "MyRoute"));
                                    //route_duration_tmp += double.Parse(gDirections.Duration.Substring(0, gDirections.Duration.IndexOf("min")));
                                }
                            }
                        }
                        else if (counter == nl.Count)
                        {
                            GDirections gDirections;
                            GMap.NET.MapProviders.GoogleMapProvider.Instance.GetDirections(out gDirections, points_curr, points_last, false, false, false, true, false);
                            if (gDirections != null)
                            {
                                gMapOverlay_Static_Faetures_Route.Routes.Add(new GMapRoute(gDirections.Route, "MyRoute"));
                                //route_duration_tmp += double.Parse(gDirections.Duration.Substring(0, gDirections.Duration.IndexOf("min")));
                            }
                        }
                        points_prev = new PointLatLng(lat1, lon1);
                        try
                        {
                            if (SignType.Contains("Speed limit"))
                            {
                                switch (SignType)
                                {
                                    case "Speed limit: 40":
                                        FileName = string.Format("{0}icons\\25_30x33.png", Path.GetFullPath(Path.Combine(RunningPath)));
                                        gMarkerGoogle_tmp = new GMap.NET.WindowsForms.Markers.GMarkerGoogle(new GMap.NET.PointLatLng(lat1, lon1), Bitmap.FromFile(FileName) as Bitmap);
                                        gMarkerGoogle_tmp.ToolTipText = "Speed limit: 25 mph";
                                        break;
                                    case "Speed limit: 64":
                                        FileName = string.Format("{0}icons\\40_30x33.png", Path.GetFullPath(Path.Combine(RunningPath)));
                                        gMarkerGoogle_tmp = new GMap.NET.WindowsForms.Markers.GMarkerGoogle(new GMap.NET.PointLatLng(lat1, lon1), Bitmap.FromFile(FileName) as Bitmap);
                                        gMarkerGoogle_tmp.ToolTipText = "Speed limit: 40 mph";
                                        break;
                                    case "Speed limit: 80":
                                        FileName = string.Format("{0}icons\\50_30x33.png", Path.GetFullPath(Path.Combine(RunningPath)));
                                        gMarkerGoogle_tmp = new GMap.NET.WindowsForms.Markers.GMarkerGoogle(new GMap.NET.PointLatLng(lat1, lon1), Bitmap.FromFile(FileName) as Bitmap);
                                        gMarkerGoogle_tmp.ToolTipText = "Speed limit: 50 mph";
                                        break;
                                    case "Speed limit: 56":
                                        FileName = string.Format("{0}icons\\35_30x33.png", Path.GetFullPath(Path.Combine(RunningPath)));
                                        gMarkerGoogle_tmp = new GMap.NET.WindowsForms.Markers.GMarkerGoogle(new GMap.NET.PointLatLng(lat1, lon1), Bitmap.FromFile(FileName) as Bitmap);
                                        gMarkerGoogle_tmp.ToolTipText = "Speed limit: 35 mph";
                                        break;
                                    case "Speed limit: 72":
                                        FileName = string.Format("{0}icons\\45_30x33.png", Path.GetFullPath(Path.Combine(RunningPath)));
                                        gMarkerGoogle_tmp = new GMap.NET.WindowsForms.Markers.GMarkerGoogle(new GMap.NET.PointLatLng(lat1, lon1), Bitmap.FromFile(FileName) as Bitmap);
                                        gMarkerGoogle_tmp.ToolTipText = "Speed limit: 45 mph";
                                        break;
                                    case "Speed limit: 24":
                                        FileName = string.Format("{0}icons\\15_30x33.png", Path.GetFullPath(Path.Combine(RunningPath)));
                                        gMarkerGoogle_tmp = new GMap.NET.WindowsForms.Markers.GMarkerGoogle(new GMap.NET.PointLatLng(lat1, lon1), Bitmap.FromFile(FileName) as Bitmap);
                                        gMarkerGoogle_tmp.ToolTipText = "Speed limit: 15 mph";
                                        break;

                                    default:
                                        FileName = string.Format("{0}icons\\speed_general_30x33.png", Path.GetFullPath(Path.Combine(RunningPath)));
                                        gMarkerGoogle_tmp = new GMap.NET.WindowsForms.Markers.GMarkerGoogle(new GMap.NET.PointLatLng(lat1, lon1), Bitmap.FromFile(FileName) as Bitmap);
                                        gMarkerGoogle_tmp.ToolTipText = SignType;
                                        break;
                                }
                            }
                            else
                            {
                                switch (SignType)
                                {
                                    case "STOP SIGN":
                                        FileName = string.Format("{0}icons\\stop2_33x33.png", Path.GetFullPath(Path.Combine(RunningPath)));
                                        gMarkerGoogle_tmp = new GMap.NET.WindowsForms.Markers.GMarkerGoogle(new GMap.NET.PointLatLng(lat1, lon1), Bitmap.FromFile(FileName) as Bitmap);
                                        break;
                                    case "PEDESTRIAN CROSSING":
                                        FileName = string.Format("{0}icons\\ped2.png", Path.GetFullPath(Path.Combine(RunningPath)));
                                        gMarkerGoogle_tmp = new GMap.NET.WindowsForms.Markers.GMarkerGoogle(new GMap.NET.PointLatLng(lat1, lon1), Bitmap.FromFile(FileName) as Bitmap);
                                        break;
                                    case "START OF NO OVERTAKING":
                                        FileName = string.Format("{0}icons\\noovertaking_33x33.png", Path.GetFullPath(Path.Combine(RunningPath)));
                                        gMarkerGoogle_tmp = new GMap.NET.WindowsForms.Markers.GMarkerGoogle(new GMap.NET.PointLatLng(lat1, lon1), Bitmap.FromFile(FileName) as Bitmap);
                                        break;
                                    case "RAILWAY CROSSING UNPROTECTED":
                                    case "RAILWAY CROSSING PROTECTED":
                                        FileName = string.Format("{0}icons\\crossingrailway.png", Path.GetFullPath(Path.Combine(RunningPath)));
                                        gMarkerGoogle_tmp = new GMap.NET.WindowsForms.Markers.GMarkerGoogle(new GMap.NET.PointLatLng(lat1, lon1), Bitmap.FromFile(FileName) as Bitmap);
                                        break;
                                    case "ROAD NARROWS":
                                        FileName = string.Format("{0}icons\\narrow2.png", Path.GetFullPath(Path.Combine(RunningPath)));
                                        gMarkerGoogle_tmp = new GMap.NET.WindowsForms.Markers.GMarkerGoogle(new GMap.NET.PointLatLng(lat1, lon1), Bitmap.FromFile(FileName) as Bitmap);
                                        break;
                                    case "GENERAL WARNING 70-80":
                                        FileName = string.Format("{0}icons\\waring.png", Path.GetFullPath(Path.Combine(RunningPath)));
                                        gMarkerGoogle_tmp = new GMap.NET.WindowsForms.Markers.GMarkerGoogle(new GMap.NET.PointLatLng(lat1, lon1), Bitmap.FromFile(FileName) as Bitmap);
                                        break;
                                    case "SCHOOL ZONE school zone":
                                        FileName = string.Format("{0}icons\\schoolsign_20x33.png", Path.GetFullPath(Path.Combine(RunningPath)));
                                        gMarkerGoogle_tmp = new GMap.NET.WindowsForms.Markers.GMarkerGoogle(new GMap.NET.PointLatLng(lat1, lon1), Bitmap.FromFile(FileName) as Bitmap);
                                        break;
                                    case "ACCIDENT HAZARD":
                                        FileName = string.Format("{0}icons\\crash.png", Path.GetFullPath(Path.Combine(RunningPath)));
                                        gMarkerGoogle_tmp = new GMap.NET.WindowsForms.Markers.GMarkerGoogle(new GMap.NET.PointLatLng(lat1, lon1), Bitmap.FromFile(FileName) as Bitmap);
                                        break;
                                    case "CONGESTION HAZARD":
                                        FileName = string.Format("{0}icons\\congestion.png", Path.GetFullPath(Path.Combine(RunningPath)));
                                        gMarkerGoogle_tmp = new GMap.NET.WindowsForms.Markers.GMarkerGoogle(new GMap.NET.PointLatLng(lat1, lon1), Bitmap.FromFile(FileName) as Bitmap);
                                        break;
                                    case "STEEP HILL DOWNWARDS":
                                        FileName = string.Format("{0}icons\\steepdown.png", Path.GetFullPath(Path.Combine(RunningPath)));
                                        gMarkerGoogle_tmp = new GMap.NET.WindowsForms.Markers.GMarkerGoogle(new GMap.NET.PointLatLng(lat1, lon1), Bitmap.FromFile(FileName) as Bitmap);
                                        break;
                                    case "STEEP HILL UPWARDS":
                                        FileName = string.Format("{0}icons\\steepup.png", Path.GetFullPath(Path.Combine(RunningPath)));
                                        gMarkerGoogle_tmp = new GMap.NET.WindowsForms.Markers.GMarkerGoogle(new GMap.NET.PointLatLng(lat1, lon1), Bitmap.FromFile(FileName) as Bitmap);
                                        break;
                                    case "LANE MERGING FROM THE RIGHT":
                                        FileName = string.Format("{0}icons\\mergeright.png", Path.GetFullPath(Path.Combine(RunningPath)));
                                        gMarkerGoogle_tmp = new GMap.NET.WindowsForms.Markers.GMarkerGoogle(new GMap.NET.PointLatLng(lat1, lon1), Bitmap.FromFile(FileName) as Bitmap);
                                        break;
                                    case "LANE MERGING FROM THE LEFT":
                                        FileName = string.Format("{0}icons\\mergeleft.png", Path.GetFullPath(Path.Combine(RunningPath)));
                                        gMarkerGoogle_tmp = new GMap.NET.WindowsForms.Markers.GMarkerGoogle(new GMap.NET.PointLatLng(lat1, lon1), Bitmap.FromFile(FileName) as Bitmap);
                                        break;
                                    case "NO TURN ON RED":
                                        FileName = string.Format("{0}icons\\no_right2.png", Path.GetFullPath(Path.Combine(RunningPath)));
                                        gMarkerGoogle_tmp = new GMap.NET.WindowsForms.Markers.GMarkerGoogle(new GMap.NET.PointLatLng(lat1, lon1), Bitmap.FromFile(FileName) as Bitmap);
                                        break;
                                    case "HORN SIGN":
                                        FileName = string.Format("{0}icons\\horn_no.png", Path.GetFullPath(Path.Combine(RunningPath)));
                                        gMarkerGoogle_tmp = new GMap.NET.WindowsForms.Markers.GMarkerGoogle(new GMap.NET.PointLatLng(lat1, lon1), Bitmap.FromFile(FileName) as Bitmap);
                                        break;
                                    case "FALLING ROCKS":
                                        FileName = string.Format("{0}icons\\falling rocks.png", Path.GetFullPath(Path.Combine(RunningPath)));
                                        gMarkerGoogle_tmp = new GMap.NET.WindowsForms.Markers.GMarkerGoogle(new GMap.NET.PointLatLng(lat1, lon1), Bitmap.FromFile(FileName) as Bitmap);
                                        break;
                                    case "WINDING ROAD STARTING LEFT":
                                        FileName = string.Format("{0}icons\\windingleft.png", Path.GetFullPath(Path.Combine(RunningPath)));
                                        gMarkerGoogle_tmp = new GMap.NET.WindowsForms.Markers.GMarkerGoogle(new GMap.NET.PointLatLng(lat1, lon1), Bitmap.FromFile(FileName) as Bitmap);
                                        break;
                                    case "WINDING ROAD STARTING RIGHT":
                                        FileName = string.Format("{0}icons\\windingright.png", Path.GetFullPath(Path.Combine(RunningPath)));
                                        gMarkerGoogle_tmp = new GMap.NET.WindowsForms.Markers.GMarkerGoogle(new GMap.NET.PointLatLng(lat1, lon1), Bitmap.FromFile(FileName) as Bitmap);
                                        break;
                                    case "SHARP CURVE RIGHT 10 sharp curve":
                                        FileName = string.Format("{0}icons\\sharpright.png", Path.GetFullPath(Path.Combine(RunningPath)));
                                        gMarkerGoogle_tmp = new GMap.NET.WindowsForms.Markers.GMarkerGoogle(new GMap.NET.PointLatLng(lat1, lon1), Bitmap.FromFile(FileName) as Bitmap);
                                        break;
                                    case "SHARP CURVE LEFT 10 sharp curve":
                                        FileName = string.Format("{0}icons\\sharpleft.png", Path.GetFullPath(Path.Combine(RunningPath)));
                                        gMarkerGoogle_tmp = new GMap.NET.WindowsForms.Markers.GMarkerGoogle(new GMap.NET.PointLatLng(lat1, lon1), Bitmap.FromFile(FileName) as Bitmap);
                                        break;
                                    case "TRAFFIC_SIGNAL":
                                        FileName = string.Format("{0}icons\\trafficlight.png", Path.GetFullPath(Path.Combine(RunningPath)));
                                        gMarkerGoogle_tmp = new GMap.NET.WindowsForms.Markers.GMarkerGoogle(new GMap.NET.PointLatLng(lat1, lon1), Bitmap.FromFile(FileName) as Bitmap);
                                        break;
                                    case "START OF NO OVERTAKING TRUCKS":
                                        FileName = string.Format("{0}icons\\noovertakingtrucks_33x33.png", Path.GetFullPath(Path.Combine(RunningPath)));
                                        gMarkerGoogle_tmp = new GMap.NET.WindowsForms.Markers.GMarkerGoogle(new GMap.NET.PointLatLng(lat1, lon1), Bitmap.FromFile(FileName) as Bitmap);
                                        break;
                                    case "ANIMAL CROSSING 30 Deer Crossing":
                                        FileName = string.Format("{0}icons\\deer.png", Path.GetFullPath(Path.Combine(RunningPath)));
                                        gMarkerGoogle_tmp = new GMap.NET.WindowsForms.Markers.GMarkerGoogle(new GMap.NET.PointLatLng(lat1, lon1), Bitmap.FromFile(FileName) as Bitmap);
                                        break;
                                    case "ICY CONDITIONS":
                                        FileName = string.Format("{0}icons\\ice.png", Path.GetFullPath(Path.Combine(RunningPath)));
                                        gMarkerGoogle_tmp = new GMap.NET.WindowsForms.Markers.GMarkerGoogle(new GMap.NET.PointLatLng(lat1, lon1), Bitmap.FromFile(FileName) as Bitmap);
                                        break;
                                    case "SLIPPERY ROAD 40 Sleppery":
                                        FileName = string.Format("{0}icons\\slip.png", Path.GetFullPath(Path.Combine(RunningPath)));
                                        gMarkerGoogle_tmp = new GMap.NET.WindowsForms.Markers.GMarkerGoogle(new GMap.NET.PointLatLng(lat1, lon1), Bitmap.FromFile(FileName) as Bitmap);
                                        break;
                                    case "YIELD":
                                        FileName = string.Format("{0}icons\\yield.png", Path.GetFullPath(Path.Combine(RunningPath)));
                                        gMarkerGoogle_tmp = new GMap.NET.WindowsForms.Markers.GMarkerGoogle(new GMap.NET.PointLatLng(lat1, lon1), Bitmap.FromFile(FileName) as Bitmap);
                                        break;
                                    case "YIELD TO BICYCLES":
                                        FileName = string.Format("{0}icons\\yield_b.png", Path.GetFullPath(Path.Combine(RunningPath)));
                                        gMarkerGoogle_tmp = new GMap.NET.WindowsForms.Markers.GMarkerGoogle(new GMap.NET.PointLatLng(lat1, lon1), Bitmap.FromFile(FileName) as Bitmap);
                                        break;
                                    case "TRUCK ROLLOVER":
                                        FileName = string.Format("{0}icons\\rollover.png", Path.GetFullPath(Path.Combine(RunningPath)));
                                        gMarkerGoogle_tmp = new GMap.NET.WindowsForms.Markers.GMarkerGoogle(new GMap.NET.PointLatLng(lat1, lon1), Bitmap.FromFile(FileName) as Bitmap);
                                        break;
                                    case "EMBANKMENT":
                                        FileName = string.Format("{0}icons\\EMBANKMENT.png", Path.GetFullPath(Path.Combine(RunningPath)));
                                        gMarkerGoogle_tmp = new GMap.NET.WindowsForms.Markers.GMarkerGoogle(new GMap.NET.PointLatLng(lat1, lon1), Bitmap.FromFile(FileName) as Bitmap);
                                        break;
                                    case "TWO-WAY TRAFFIC":
                                        FileName = string.Format("{0}icons\\twowayroad.png", Path.GetFullPath(Path.Combine(RunningPath)));
                                        gMarkerGoogle_tmp = new GMap.NET.WindowsForms.Markers.GMarkerGoogle(new GMap.NET.PointLatLng(lat1, lon1), Bitmap.FromFile(FileName) as Bitmap);
                                        break;
                                    case "END OF NO OVERTAKING":
                                        FileName = string.Format("{0}icons\\overtaking_end.png", Path.GetFullPath(Path.Combine(RunningPath)));
                                        gMarkerGoogle_tmp = new GMap.NET.WindowsForms.Markers.GMarkerGoogle(new GMap.NET.PointLatLng(lat1, lon1), Bitmap.FromFile(FileName) as Bitmap);
                                        break;
                                    case "END OF NO OVERTAKING TRUCKS":
                                        FileName = string.Format("{0}icons\\overtakingtruck_end.png", Path.GetFullPath(Path.Combine(RunningPath)));
                                        gMarkerGoogle_tmp = new GMap.NET.WindowsForms.Markers.GMarkerGoogle(new GMap.NET.PointLatLng(lat1, lon1), Bitmap.FromFile(FileName) as Bitmap);
                                        break;
                                    case "BICYCLE CROSSING":
                                        FileName = string.Format("{0}icons\\bicycle.png", Path.GetFullPath(Path.Combine(RunningPath)));
                                        gMarkerGoogle_tmp = new GMap.NET.WindowsForms.Markers.GMarkerGoogle(new GMap.NET.PointLatLng(lat1, lon1), Bitmap.FromFile(FileName) as Bitmap);
                                        break;
                                    case "FLOOD AREA":
                                        FileName = string.Format("{0}icons\\flood.png", Path.GetFullPath(Path.Combine(RunningPath)));
                                        gMarkerGoogle_tmp = new GMap.NET.WindowsForms.Markers.GMarkerGoogle(new GMap.NET.PointLatLng(lat1, lon1), Bitmap.FromFile(FileName) as Bitmap);
                                        break;
                                    case "HUMP BRIDGE":
                                        FileName = string.Format("{0}icons\\humpbridge.png", Path.GetFullPath(Path.Combine(RunningPath)));
                                        gMarkerGoogle_tmp = new GMap.NET.WindowsForms.Markers.GMarkerGoogle(new GMap.NET.PointLatLng(lat1, lon1), Bitmap.FromFile(FileName) as Bitmap);
                                        break;
                                    case "LATERAL WIND":
                                        FileName = string.Format("{0}icons\\wind_lat.png", Path.GetFullPath(Path.Combine(RunningPath)));
                                        gMarkerGoogle_tmp = new GMap.NET.WindowsForms.Markers.GMarkerGoogle(new GMap.NET.PointLatLng(lat1, lon1), Bitmap.FromFile(FileName) as Bitmap);
                                        break;
                                    case "END OF ALL RESTRICTIONS":
                                        FileName = string.Format("{0}icons\\1984079.png", Path.GetFullPath(Path.Combine(RunningPath)));
                                        gMarkerGoogle_tmp = new GMap.NET.WindowsForms.Markers.GMarkerGoogle(new GMap.NET.PointLatLng(lat1, lon1), Bitmap.FromFile(FileName) as Bitmap);
                                        break;
                                    case "TRAMWAY CROSSING":
                                        FileName = string.Format("{0}icons\\tram.png", Path.GetFullPath(Path.Combine(RunningPath)));
                                        gMarkerGoogle_tmp = new GMap.NET.WindowsForms.Markers.GMarkerGoogle(new GMap.NET.PointLatLng(lat1, lon1), Bitmap.FromFile(FileName) as Bitmap);
                                        break;
                                    case "RISK OF GROUNDING":
                                        FileName = string.Format("{0}icons\\grounding.png", Path.GetFullPath(Path.Combine(RunningPath)));
                                        gMarkerGoogle_tmp = new GMap.NET.WindowsForms.Markers.GMarkerGoogle(new GMap.NET.PointLatLng(lat1, lon1), Bitmap.FromFile(FileName) as Bitmap);
                                        break;
                                    case "YIELD TO ONCOMING TRAFFIC":
                                        FileName = string.Format("{0}icons\\yield_oncoming.jpg", Path.GetFullPath(Path.Combine(RunningPath)));
                                        gMarkerGoogle_tmp = new GMap.NET.WindowsForms.Markers.GMarkerGoogle(new GMap.NET.PointLatLng(lat1, lon1), Bitmap.FromFile(FileName) as Bitmap);
                                        break;
                                    case "UNEVEN ROAD":
                                        FileName = string.Format("{0}icons\\unevenroad_33x33.png", Path.GetFullPath(Path.Combine(RunningPath)));
                                        gMarkerGoogle_tmp = new GMap.NET.WindowsForms.Markers.GMarkerGoogle(new GMap.NET.PointLatLng(lat1, lon1), Bitmap.FromFile(FileName) as Bitmap);
                                        break;
                                    case "NO ENGINE BRAKE":
                                        FileName = string.Format("{0}icons\\No-Engine-Brake_33x33.png", Path.GetFullPath(Path.Combine(RunningPath)));
                                        gMarkerGoogle_tmp = new GMap.NET.WindowsForms.Markers.GMarkerGoogle(new GMap.NET.PointLatLng(lat1, lon1), Bitmap.FromFile(FileName) as Bitmap);
                                        break;
                                    case "NO IDLING":
                                        FileName = string.Format("{0}icons\\NOIDLING_1_16x32.png", Path.GetFullPath(Path.Combine(RunningPath)));
                                        gMarkerGoogle_tmp = new GMap.NET.WindowsForms.Markers.GMarkerGoogle(new GMap.NET.PointLatLng(lat1, lon1), Bitmap.FromFile(FileName) as Bitmap);
                                        break;
                                    case "NO TOWED CARAVAN ALLOWED":
                                        FileName = string.Format("{0}icons\\notowed_33x33.jpg", Path.GetFullPath(Path.Combine(RunningPath)));
                                        gMarkerGoogle_tmp = new GMap.NET.WindowsForms.Markers.GMarkerGoogle(new GMap.NET.PointLatLng(lat1, lon1), Bitmap.FromFile(FileName) as Bitmap);
                                        break;
                                    case "NO TOWED TRAILER ALLOWED":
                                        FileName = string.Format("{0}icons\\no_trailer.png", Path.GetFullPath(Path.Combine(RunningPath)));
                                        gMarkerGoogle_tmp = new GMap.NET.WindowsForms.Markers.GMarkerGoogle(new GMap.NET.PointLatLng(lat1, lon1), Bitmap.FromFile(FileName) as Bitmap);
                                        break;
                                    case "TURN PERMITTED ON RED":
                                        FileName = string.Format("{0}icons\\993951_33x33.png", Path.GetFullPath(Path.Combine(RunningPath)));
                                        gMarkerGoogle_tmp = new GMap.NET.WindowsForms.Markers.GMarkerGoogle(new GMap.NET.PointLatLng(lat1, lon1), Bitmap.FromFile(FileName) as Bitmap);
                                        break;
                                    case "LOW GEAR":
                                        FileName = string.Format("{0}icons\\lowgear_33x33.jpg", Path.GetFullPath(Path.Combine(RunningPath)));
                                        gMarkerGoogle_tmp = new GMap.NET.WindowsForms.Markers.GMarkerGoogle(new GMap.NET.PointLatLng(lat1, lon1), Bitmap.FromFile(FileName) as Bitmap);
                                        break;
                                    case "URBAN AREA":
                                        FileName = string.Format("{0}icons\\ped.png", Path.GetFullPath(Path.Combine(RunningPath)));
                                        gMarkerGoogle_tmp = new GMap.NET.WindowsForms.Markers.GMarkerGoogle(new GMap.NET.PointLatLng(lat1, lon1), Bitmap.FromFile(FileName) as Bitmap);
                                        break;
                                    case "Bridge":
                                        FileName = string.Format("{0}icons\\bridge.png", Path.GetFullPath(Path.Combine(RunningPath)));
                                        gMarkerGoogle_tmp = new GMap.NET.WindowsForms.Markers.GMarkerGoogle(new GMap.NET.PointLatLng(lat1, lon1), Bitmap.FromFile(FileName) as Bitmap);
                                        break;
                                    case "Start of Manoeuvre":
                                    case "End of Manoeuvre":
                                        FileName = string.Format("{0}icons\\manoeuvre_33x33.png", Path.GetFullPath(Path.Combine(RunningPath)));
                                        gMarkerGoogle_tmp = new GMap.NET.WindowsForms.Markers.GMarkerGoogle(new GMap.NET.PointLatLng(lat1, lon1), Bitmap.FromFile(FileName) as Bitmap);
                                        break;
                                    case "Start of Ramp":
                                    case "End of Ramp":
                                        FileName = string.Format("{0}icons\\ramp_33x33.png", Path.GetFullPath(Path.Combine(RunningPath)));
                                        gMarkerGoogle_tmp = new GMap.NET.WindowsForms.Markers.GMarkerGoogle(new GMap.NET.PointLatLng(lat1, lon1), Bitmap.FromFile(FileName) as Bitmap);
                                        break;
                                    case "Start of Paved":
                                    case "End of Paved":
                                        FileName = string.Format("{0}icons\\paved_33x33.png", Path.GetFullPath(Path.Combine(RunningPath)));
                                        gMarkerGoogle_tmp = new GMap.NET.WindowsForms.Markers.GMarkerGoogle(new GMap.NET.PointLatLng(lat1, lon1), Bitmap.FromFile(FileName) as Bitmap);
                                        break;
                                    case "Roundabout":
                                        FileName = string.Format("{0}icons\\round_33.png", Path.GetFullPath(Path.Combine(RunningPath)));
                                        gMarkerGoogle_tmp = new GMap.NET.WindowsForms.Markers.GMarkerGoogle(new GMap.NET.PointLatLng(lat1, lon1), Bitmap.FromFile(FileName) as Bitmap);
                                        break;
                                    case "Start of Bothways":
                                    case "End of Bothways":
                                        FileName = string.Format("{0}icons\\bothways.png", Path.GetFullPath(Path.Combine(RunningPath)));
                                        gMarkerGoogle_tmp = new GMap.NET.WindowsForms.Markers.GMarkerGoogle(new GMap.NET.PointLatLng(lat1, lon1), Bitmap.FromFile(FileName) as Bitmap);
                                        break;
                                    case "Start of Limited access":
                                    case "End of Limited access":
                                        FileName = string.Format("{0}icons\\limited_33x33.png", Path.GetFullPath(Path.Combine(RunningPath)));
                                        gMarkerGoogle_tmp = new GMap.NET.WindowsForms.Markers.GMarkerGoogle(new GMap.NET.PointLatLng(lat1, lon1), Bitmap.FromFile(FileName) as Bitmap);
                                        break;
                                    case "HILL AREA":
                                        FileName = string.Format("{0}icons\\hill_33x33.png", Path.GetFullPath(Path.Combine(RunningPath)));
                                        gMarkerGoogle_tmp = new GMap.NET.WindowsForms.Markers.GMarkerGoogle(new GMap.NET.PointLatLng(lat1, lon1), Bitmap.FromFile(FileName) as Bitmap);
                                        break;
                                    case "Start of Urban":
                                    case "End of Urban":
                                        FileName = string.Format("{0}icons\\urban_33x20.png", Path.GetFullPath(Path.Combine(RunningPath)));
                                        gMarkerGoogle_tmp = new GMap.NET.WindowsForms.Markers.GMarkerGoogle(new GMap.NET.PointLatLng(lat1, lon1), Bitmap.FromFile(FileName) as Bitmap);
                                        break;
                                    case "Start of Multi lane":
                                    case "End of Multi lane":
                                        FileName = string.Format("{0}icons\\lanesmulti_33x16.png", Path.GetFullPath(Path.Combine(RunningPath)));
                                        gMarkerGoogle_tmp = new GMap.NET.WindowsForms.Markers.GMarkerGoogle(new GMap.NET.PointLatLng(lat1, lon1), Bitmap.FromFile(FileName) as Bitmap);
                                        break;
                                    case "Start of One way":
                                    case "End of One way":
                                        FileName = string.Format("{0}icons\\oneway_33x33.png", Path.GetFullPath(Path.Combine(RunningPath)));
                                        gMarkerGoogle_tmp = new GMap.NET.WindowsForms.Markers.GMarkerGoogle(new GMap.NET.PointLatLng(lat1, lon1), Bitmap.FromFile(FileName) as Bitmap);
                                        break;
                                        //default:
                                        //    gMarkerGoogle_tmp = new GMap.NET.WindowsForms.Markers.GMarkerGoogle(new GMap.NET.PointLatLng(lat1, lon1), GMap.NET.WindowsForms.Markers.GMarkerGoogleType.red);
                                        //    break;
                                        /*
                                        "GENERAL CURVE"
                                        "GENERAL HILL"
                                        "PROTECTED OVERTAKING - EXTRA LANE"
                                        "PRIORITY OVER ONCOMING TRAFFIC"
                                        "CROSSING WITH PRIORITY FROM THE RIGHT"
                                        "PROTECTED OVERTAKING - EXTRA LANE RIGHT"
                                        "DOUBLE HAIRPIN"
                                        "TRIPLE HAIRPIN"
                                        "URBAN AREA"
                                        "PROTECTED OVERTAKING - EXTRA LANE LEFT"
                                        "OBSTACLE"
                                        "END OF NO ENGINE BRAKE"
                                        "END OF LOW GEAR"
                                        "NO CAMPER OR MOTORHOME ALLOWED"
                                        "LANE MERGE CENTRE"
                                        */
                                }
                                gMarkerGoogle_tmp.ToolTipText = SignType;
                            }
                        }
                        catch (Exception err)
                        {
                            MessageBox.Show(err.Message);
                        }
                    }
                    if (gMarkerGoogle_tmp != null)
                        gMapOverlay_Static_Faetures.Markers.Add(gMarkerGoogle_tmp);
                }
                //////No Signs//////////////////////////////////////////////////
                if ((0 == nl.Count || onlycharging) && points_last.Lat != 0.0 && points_last.Lng != 0.0 && points_first.Lat != 0.0 && points_first.Lng != 0.0)
                {
                    GDirections gDirections;
                    GMap.NET.MapProviders.GoogleMapProvider.Instance.GetDirections(out gDirections, points_first, points_last, false, false, false, true, false);
                    if (gDirections != null)
                        gMapOverlay_Static_Faetures_Route.Routes.Add(new GMapRoute(gDirections.Route, "MyRoute"));
                }

                ///Create New GPX Files/////////////////////////////////
                try
                {
                    string GPX_String = "";
                    GPX_String = @"<?xml version=""1.0"" encoding=""UTF-8"" standalone=""no"" ?>";
                    GPX_String += "\n";
                    GPX_String += @"<gpx xmlns=""http://www.topografix.com/GPX/1/1"" xmlns:gpxx=""http://www.garmin.com/xmlschemas/GpxExtensions/v3"" xmlns:gpxtpx=""http://www.garmin.com/xmlschemas/TrackPointExtension/v1"" ";
                    GPX_String += @"creator=""mapstogpx.com"" version=""1.1"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xsi:schemaLocation=""http://www.topografix.com/GPX/1/1 http://www.topografix.com/GPX/1/1/gpx.xsd";
                    GPX_String += @" http://www.garmin.com/xmlschemas/GpxExtensions/v3 http://www.garmin.com/xmlschemas/GpxExtensionsv3.xsd http://www.garmin.com/xmlschemas/TrackPointExtension/v1 http://www.garmin.com/xmlschemas/TrackPointExtensionv1.xsd"">";
                    GPX_String += "\n";
                    foreach (GMapMarker tmp_marker in gMapOverlay_Static_Faetures.Markers)
                    {
                        GPX_String += "\t"+@"<wpt lat=""" + tmp_marker.Position.Lat + @""" lon=""" + tmp_marker.Position.Lng + @""">";
                        GPX_String += "\n";
                        GPX_String += "\t\t<name>" + tmp_marker.ToolTipText + "</name>";
                        GPX_String += "\n";
                        GPX_String += "\t</wpt>";
                        GPX_String += "\n";
                    }
                    GPX_String += "<rte>";
                    GPX_String += "\n";
                    GPX_String += "\t<name>" + filename + "</name>";
                    GPX_String += "\n";
                    double route_length_tmp = 0.0;
                    foreach (GMapRoute tmp_route in gMapOverlay_Static_Faetures_Route.Routes)
                    {
                        route_length_tmp += tmp_route.Distance;
                        foreach (PointLatLng tmp_point in tmp_route.Points)
                        {
                            GPX_String += "\t"+@"<rtept lat=""" + tmp_point.Lat + @""" lon=""" + tmp_point.Lng + @"""></rtept>";
                            GPX_String += "\n";
                        }
                    }
                    GPX_String += "</rte>\n";
                    GPX_String += "</gpx>\n";
                    WriteGpxFileAsync("route0_navigation.gpx", GPX_String);                    
                    ///Update route info labels/////////////////////////////
                    label_signscount.Text = "Features Count = " + gMapOverlay_Static_Faetures.Markers.Count.ToString();
                    //label_routeTime.Text = "Route Time = " + //route_duration_tmp.ToString() + " min";
                    route_length_tmp = route_length_tmp * 0.621371;
                    //if (route_length_tmp.ToString().Length >= 5)
                    //    label_routelength.Text = "Route Length = " + route_length_tmp.ToString().Substring(0, 5) + " miles";
                    //else
                    //    label_routelength.Text = "Route Length = " + route_length_tmp.ToString() + " miles";
                    ///Draw Start End Points////////////////////////////////
                    FileName = string.Format("{0}icons\\start.png", Path.GetFullPath(Path.Combine(RunningPath)));
                    gMarker_StartEnd = new GMap.NET.WindowsForms.Markers.GMarkerGoogle(new GMap.NET.PointLatLng(points_first.Lat, points_first.Lng), Bitmap.FromFile(FileName) as Bitmap);
                    //gMarker_StartEnd = new GMap.NET.WindowsForms.Markers.GMarkerGoogle(new GMap.NET.PointLatLng(points_first.Lat, points_first.Lng), GMap.NET.WindowsForms.Markers.GMarkerGoogleType.green);
                    gMarker_StartEnd.ToolTipText = "Start Point: " + textBox_startpoint.Text;
                    gMapOverlay_StartEnd.Markers[0] = gMarker_StartEnd;
                    FileName = string.Format("{0}icons\\end.png", Path.GetFullPath(Path.Combine(RunningPath)));
                    gMarker_StartEnd = new GMap.NET.WindowsForms.Markers.GMarkerGoogle(new GMap.NET.PointLatLng(points_last.Lat, points_last.Lng), Bitmap.FromFile(FileName) as Bitmap);
                    //gMarker_StartEnd = new GMap.NET.WindowsForms.Markers.GMarkerGoogle(new GMap.NET.PointLatLng(points_last.Lat, points_last.Lng), GMap.NET.WindowsForms.Markers.GMarkerGoogleType.red);
                    gMarker_StartEnd.ToolTipText = "End Point: " + textBox_startpoint.Text;
                    gMapOverlay_StartEnd.Markers[1] = gMarker_StartEnd;

                    gMapControl1.Overlays.Add(gMapOverlay_Static_Faetures);
                    gMapControl1.Overlays.Add(gMapOverlay_Static_Faetures_Route);
                    //gMapControl1.Invoke((MethodInvoker)(() => gMapControl1.Position = new GMap.NET.PointLatLng(lat1, lon1)));
                    gMapControl1.Invoke((MethodInvoker)(() => gMapControl1.Zoom += 1));
                    gMapControl1.Invoke((MethodInvoker)(() => gMapControl1.Zoom -= 1));
                }
                catch (Exception err)
                {
                    MessageBox.Show(err.Message, "Error");
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void ParseGPXRoute(string filename)
        {
            GMap.NET.WindowsForms.Markers.GMarkerGoogle gMarkerGoogle_tmp;
            GMap.NET.WindowsForms.GMapOverlay gMapOverlay_HERE_routes = new GMapOverlay("Overlay_HERE_Routes");
            GMap.NET.WindowsForms.GMapOverlay gMapOverlay_Layer0_Route = new GMapOverlay("Overlay_Layer0_Route");
            GMap.NET.WindowsForms.GMapOverlay gMapOverlay_Layer0_Faetures = new GMap.NET.WindowsForms.GMapOverlay("Overlay_Layer0_Features");
            List<PointLatLng> points = new List<PointLatLng>();
            List<PointLatLng> points_adjusted = new List<PointLatLng>();
            PointLatLng points_prev = new PointLatLng(), points_curr = new PointLatLng();
            try
            {
                double segmentdistance = 0.0;
                XmlDocument gpxDoc = new XmlDocument();
                gpxDoc.Load(filename);
                XmlNamespaceManager nsmgr = new XmlNamespaceManager(gpxDoc.NameTable);
                nsmgr.AddNamespace("x", "http://www.topografix.com/GPX/1/1");
                XmlNodeList nl = gpxDoc.SelectNodes("//x:trkpt", nsmgr);
                string outerxml = "", SignType = "";
                double lat1 = 0.0, lon1 = 0.0;
                double latprev = 0.0, lonprev = 0.0;
                int counter = 0;

                string GPX_String = "";
                string GPX_String_nodes = "";
                string GPX_String2 = "";
                string GPX_String_rtept = "";
                GPX_String = @"<?xml version=""1.0"" encoding=""UTF-8"" standalone=""no"" ?>";
                GPX_String += "\n";
                GPX_String += @"<gpx xmlns=""http://www.topografix.com/GPX/1/1"" xmlns:gpxx=""http://www.garmin.com/xmlschemas/GpxExtensions/v3"" xmlns:gpxtpx=""http://www.garmin.com/xmlschemas/TrackPointExtension/v1"" ";
                GPX_String += @"creator=""mapstogpx.com"" version=""1.1"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xsi:schemaLocation=""http://www.topografix.com/GPX/1/1 http://www.topografix.com/GPX/1/1/gpx.xsd";
                GPX_String += @" http://www.garmin.com/xmlschemas/GpxExtensions/v3 http://www.garmin.com/xmlschemas/GpxExtensionsv3.xsd http://www.garmin.com/xmlschemas/TrackPointExtension/v1 http://www.garmin.com/xmlschemas/TrackPointExtensionv1.xsd"">";
                GPX_String += "\n";
                GPX_String += "<rte>";
                GPX_String += "\n";
                GPX_String += "\t<name>" + filename + "</name>";
                GPX_String += "\n";

                foreach (XmlNode xnode in nl)
                {
                    outerxml = xnode.OuterXml;
                    SignType = xnode.InnerText;
                    lat1 = Convert.ToDouble(outerxml.Substring(outerxml.IndexOf("lat") + 5, (outerxml.IndexOf("lon") - outerxml.IndexOf("lat") - 7)));
                    lon1 = Convert.ToDouble(outerxml.Substring(outerxml.IndexOf("lon") + 5, (outerxml.IndexOf("xmlns") - outerxml.IndexOf("lon") - 7)));

                    points.Add(new PointLatLng(lat1, lon1));
                    if (counter == 0)
                    {
                        points_first = new PointLatLng(lat1, lon1);
                        GPX_String_rtept += "\t" + @"<rtept lat=""" + lat1.ToString() + @""" lon=""" + lon1.ToString() + @"""></rtept>";
                        GPX_String_rtept += "\n";

                        GPX_String_nodes += "\t" + @"<wpt lat=""" + lat1.ToString() + @""" lon=""" + lon1.ToString() + @""">" + "<name>Start Point" + counter.ToString() + "</name>" + "</wpt>";
                        GPX_String_nodes += "\n";
                        points_adjusted.Add(new PointLatLng(lat1, lon1));
                    }
                    if (counter == nl.Count - 1)
                    {
                        points_last = new PointLatLng(lat1, lon1);
                        GPX_String_rtept += "\t" + @"<rtept lat=""" + lat1.ToString() + @""" lon=""" + lon1.ToString() + @"""></rtept>";
                        GPX_String_rtept += "\n";

                        GPX_String_nodes += "\t" + @"<wpt lat=""" + lat1.ToString() + @""" lon=""" + lon1.ToString() + @""">" + "<name>END Point" + counter.ToString() + "</name>" + "</wpt>";
                        GPX_String_nodes += "\n";
                        points_adjusted.Add(new PointLatLng(lat1, lon1));
                    }
                    counter++;

                    //Find Distance
                    if (counter > 1 && counter < nl.Count - 1)
                    {
                        if (usegoogle_api)
                        {
                            var sCoord = new GeoCoordinate(lat1, lon1);
                            var eCoord = new GeoCoordinate(latprev, lonprev);
                            segmentdistance = sCoord.GetDistanceTo(eCoord);
                        }
                        if (segmentdistance > 75 && usegoogle_api == true)
                        {
                            PointLatLng point1tmp1 = new PointLatLng(latprev, lonprev);
                            PointLatLng point2tmp2 = new PointLatLng(lat1, lon1);
                            GDirections gDirections;
                            GMap.NET.MapProviders.GoogleMapProvider.Instance.GetDirections(out gDirections, point1tmp1, point2tmp2, false, false, false, true, false);
                            if (gDirections != null)
                            {
                                for (int i = 0; i < gDirections.Route.Count; i++)
                                {
                                    if (i == 0)
                                    {
                                        GPX_String_nodes += "\t" + @"<wpt lat=""" + latprev.ToString() + @""" lon=""" + lonprev.ToString() + @""">" + "<name>Good Start" + counter.ToString() + "</name>" + "</wpt>";
                                        GPX_String_nodes += "\n";

                                        GPX_String_rtept += "\t" + @"<rtept lat=""" + gDirections.Route[i].Lat.ToString() + @""" lon=""" + gDirections.Route[i].Lng.ToString() + @"""></rtept>";
                                        GPX_String_rtept += "\n";
                                    }
                                    else if (i == gDirections.Route.Count-1)
                                    {
                                        GPX_String_nodes += "\t" + @"<wpt lat=""" + lat1.ToString() + @""" lon=""" + lon1.ToString() + @""">" + "<name>Good End" + counter.ToString() + "</name>" + "</wpt>";
                                        GPX_String_nodes += "\n";

                                        GPX_String_rtept += "\t" + @"<rtept lat=""" + gDirections.Route[i].Lat.ToString() + @""" lon=""" + gDirections.Route[i].Lng.ToString() + @"""></rtept>";
                                        GPX_String_rtept += "\n";
                                    }
                                    else
                                    {
                                        GPX_String_nodes += "\t" + @"<wpt lat=""" + gDirections.Route[i].Lat.ToString() + @""" lon=""" + gDirections.Route[i].Lng.ToString() + @""">" + "<name>Google Point" + counter.ToString() + "</name>" + "</wpt>";
                                        GPX_String_nodes += "\n";

                                        GPX_String_rtept += "\t" + @"<rtept lat=""" + gDirections.Route[i].Lat.ToString() + @""" lon=""" + gDirections.Route[i].Lng.ToString() + @"""></rtept>";
                                        GPX_String_rtept += "\n";
                                    }
                                    points_adjusted.Add(new PointLatLng(gDirections.Route[i].Lat, gDirections.Route[i].Lng));
                                }
                            }
                            else
                            {
                                GPX_String_nodes += "\t" + @"<wpt lat=""" + latprev.ToString() + @""" lon=""" + lonprev.ToString() + @""">" + "<name>Not Good Start" + counter.ToString() + "</name>" + "</wpt>";
                                GPX_String_nodes += "\n";
                                GPX_String_nodes += "\t" + @"<wpt lat=""" + lat1.ToString() + @""" lon=""" + lon1.ToString() + @""">" + "<name>Not Good End" + counter.ToString() + "</name>" + "</wpt>";
                                GPX_String_nodes += "\n";

                                GPX_String_rtept += "\t" + @"<rtept lat=""" + latprev.ToString() + @""" lon=""" + lonprev.ToString() + @"""></rtept>";
                                GPX_String_rtept += "\n";
                                GPX_String_rtept += "\t" + @"<rtept lat=""" + lat1.ToString() + @""" lon=""" + lon1.ToString() + @"""></rtept>";
                                GPX_String_rtept += "\n";
                            }
                        }
                        else
                        {
                            GPX_String_nodes += "\t" + @"<wpt lat=""" + lat1.ToString() + @""" lon=""" + lon1.ToString() + @""">" + "<name>HERE Point" + counter.ToString() + "</name>" + "</wpt>";
                            GPX_String_nodes += "\n";
                            GPX_String_rtept += "\t" + @"<rtept lat=""" + lat1.ToString() + @""" lon=""" + lon1.ToString() + @"""></rtept>";
                            GPX_String_rtept += "\n";
                            points_adjusted.Add(new PointLatLng(lat1, lon1));
                        }
                    }
                    latprev = lat1;
                    lonprev = lon1;
                    //Find Distance
                }
                GPX_String2 = GPX_String;
                GPX_String2 += GPX_String_rtept;
                GPX_String2 += "</rte>\n";
                GPX_String += "</rte>\n";
                GPX_String += GPX_String_nodes;
                GPX_String += "</gpx>\n";
                GPX_String2 += "</gpx>\n";

                GMapRoute route2 = new GMapRoute(points, "Route Generator2");
                route2.Stroke = new Pen(Color.Red, 3);
                gMapOverlay_HERE_routes.Routes.Add(route2);
                if (usegoogle_api)
                {
                    string newfilename = filename.Substring(0, filename.Length - 4) + "_adjusted_nodes" + ".gpx";
                    string newfilename_repts = filename.Substring(0, filename.Length - 4) + "_adjusted_route" + ".gpx";
                    WriteGpxFileAsync(newfilename, GPX_String);
                    WriteGpxFileAsync(newfilename_repts, GPX_String2);

                    GMapRoute route3 = new GMapRoute(points_adjusted, "Route Generator3");
                    route3.Stroke = new Pen(Color.Black, 2);
                    gMapOverlay_HERE_routes.Routes.Add(route3);
                }
                gMapControl1.Overlays.Add(gMapOverlay_HERE_routes);
                gMapControl1.Invoke((MethodInvoker)(() => gMapControl1.Position = new GMap.NET.PointLatLng(lat1, lon1)));
                gMapControl1.Invoke((MethodInvoker)(() => gMapControl1.Zoom += 1));
                gMapControl1.Invoke((MethodInvoker)(() => gMapControl1.Zoom -= 1));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private static async Task WriteGpxFileAsync(string filename, string text)
        {
            await File.WriteAllTextAsync(filename, text);
        }
        /*private static void WriteGpxFileAsync(string filename, string text)
        {
            try
            {
                File.WriteAllText(filename, text);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }*/

        #region
        private void button1_Click(object sender, EventArgs e)
        {
            gMapControl1.Overlays.Clear();
            gMapControl1.Overlays.Add(gMapOverlay_StartEnd);
            ParseGPXRoute("route0_staticfeaturesfile.gpx");
            ParseGPXSigns("route0_staticfeaturesfile.gpx");
        }
        private void button_move_one_left_Click(object sender, EventArgs e)
        {
            if (listBox_src.SelectedIndex < 0)
                return;
            if (listBox_dst.Items.Contains(listBox_src.SelectedItems[0]))
                return;
            string item = (string)listBox_src.SelectedItems[0];
            listBox_dst.Items.Add(item);
            //listBox_src.Items.Remove(item);
        }
        private void button_move_one_right_Click(object sender, EventArgs e)
        {
            if (listBox_dst.SelectedIndex < 0)
                return;
            string item = (string)listBox_dst.SelectedItems[0];
            listBox_dst.Items.Remove(item);
            //listBox_src.Items.Add(item);
        }
        private void button3_Click(object sender, EventArgs e)
        {
            listBox_dst.Items.AddRange(listBox_src.Items);
            //listBox_src.Items.Clear();
        }
        private void button_move_all_right_Click(object sender, EventArgs e)
        {
            //listBox_src.Items.AddRange(listBox_dst.Items);
            listBox_dst.Items.Clear();
        }
        private void listBox_src_DoubleClick(object sender, EventArgs e)
        {
            if (listBox_src.SelectedIndex < 0)
                return;
            if (listBox_dst.Items.Contains(listBox_src.SelectedItems[0]))
                return;
            string item = (string)listBox_src.SelectedItems[0];
            listBox_dst.Items.Add(item);
            //listBox_src.Items.Remove(item);
        }
        private void listBox_dst_DoubleClick(object sender, EventArgs e)
        {
            if (listBox_dst.SelectedIndex < 0)
                return;
            string item = (string)listBox_dst.SelectedItems[0];
            listBox_dst.Items.Remove(item);
            //listBox_src.Items.Add(item);
        }
        private void gMapControl1_MouseMove(object sender, MouseEventArgs e)
        {
            double lat = gMapControl1.FromLocalToLatLng(e.X, e.Y).Lat;
            double lon = gMapControl1.FromLocalToLatLng(e.X, e.Y).Lng;
            if (lat.ToString().Length >= 8 && lon.ToString().Length >= 9)
                gMapControl1.Invoke((MethodInvoker)(() => label_coordinates.Text = lat.ToString().Substring(0, 8) + ", " + lon.ToString().Substring(0, 9)));
            else
                gMapControl1.Invoke((MethodInvoker)(() => label_coordinates.Text = lat.ToString() + ", " + lon.ToString()));
        }
        private void button2_Click(object sender, EventArgs e)
        {
            System.Environment.Exit(0);
        }
        private void textBox_startpoint_Enter(object sender, EventArgs e)
        {
            mapclickedindex = 0;
        }
        private void textBox_endpoint_Enter(object sender, EventArgs e)
        {
            mapclickedindex = 1;
        }
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            System.Environment.Exit(0);
        }
        private void button_clearmap_Click(object sender, EventArgs e)
        {
            gMapControl1.Position = new GMap.NET.PointLatLng(42.702468, -83.254997);
            gMapControl1.MinZoom = 0;
            gMapControl1.MaxZoom = 18;
            gMapControl1.Zoom = 10;
            mapclickedindex = -1;
            textBox_startpoint.Text = "42.702468, -83.254997";
            textBox_endpoint.Text = "42.665226, -83.193315";
            textBox_radius.Text = "10.0";
            textBox_desiredlength.Text = "60.0";
            comboBox_searchtype.SelectedIndex = 0;

            gMapControl1.Overlays.Clear();
            gMapControl1.Invoke((MethodInvoker)(() => gMapControl1.Zoom += 1));
            gMapControl1.Invoke((MethodInvoker)(() => gMapControl1.Zoom -= 1));
            listBox_src.Items.AddRange(listBox_dst.Items);
            listBox_dst.Items.Clear();
            gMapControl1.Overlays.Add(gMapOverlay_StartEnd);
        }
        private void comboBox_numberofroutes_SelectedIndexChanged(object sender, EventArgs e)
        {
            gMapControl1.Overlays.Clear();
            gMapControl1.Overlays.Add(gMapOverlay_StartEnd);
            string filename_tmp = comboBox_numberofroutes.SelectedItem as string;
            string tmpfilename = gpx_currentDirectoryname + "\\" + filename_tmp;
            if (gpx_currentDirectoryname == "")
            {
                ParseGPXRoute("route0_staticfeaturesfile.gpx");
                ParseGPXSigns("route0_staticfeaturesfile.gpx");
            }
            else
            {
                ParseGPXRoute(tmpfilename);
                ParseGPXSigns(tmpfilename);
            }
        }
        private void richTextBox_serverlogs_TextChanged(object sender, EventArgs e)
        {
            richTextBox_serverlogs.ScrollToCaret();
        }

        private void label_startpoint_Click(object sender, EventArgs e)
        {
        }
        private void label_startpoint_MouseDown(object sender, MouseEventArgs e)
        {
            toolTip1.Show("Enter the route start point manually or right click on the map (green marker) (click inside the box first)", textBox_startpoint);
        }

        private void label_startpoint_MouseUp(object sender, MouseEventArgs e)
        {
            toolTip1.Hide(textBox_startpoint);
        }


        private void label_endpoint_MouseDown(object sender, MouseEventArgs e)
        {
            toolTip2.Show("Enter the route end point manually or right click on the map (red marker) (point to point type) (click inside the box first)", textBox_endpoint);
        }

        private void label_endpoint_MouseUp(object sender, MouseEventArgs e)
        {
            toolTip2.Hide(textBox_endpoint);
        }

        private void label6_MouseDown(object sender, MouseEventArgs e)
        {
            String tmp_fetaureswithsign = "non_pedestrian_crossing\n" + "school_zone\n" + "variable_speed\n" + "traffic_signs\n" + "railway_crossing\n" + "no_overtaking\n" + "overtaking\n" + "falling_rocks\n" + "crosswalk\n" + "two_way\n" + "lane_merge_right\n" + "lane_merge_left\n" + "lane_merge_center\n" + "hills\n" + "tunnel\n" + "bridge\n" + "pedestrian\n" + "stop_signs\n" + "icy_road\n" + "traffic_lights";
            toolTip6.Show("Select static features to be included in the generated routes following are features with signs\n" + tmp_fetaureswithsign,groupBox1);
        }

        private void label6_MouseUp(object sender, MouseEventArgs e)
        {
            toolTip6.Hide(groupBox1);
        }

        private void label9_MouseDown(object sender, MouseEventArgs e)
        {
            toolTip3.Show("Enter the radius from the start point to search for routes (point to anywhere type)", textBox_radius) ;
        }

        private void label9_MouseUp(object sender, MouseEventArgs e)
        {
            toolTip3.Hide(textBox_radius);
        }

        private void label10_MouseDown(object sender, MouseEventArgs e)
        {
            toolTip4.Show("Enter the desired length for generated routes (point to anywhere type)", textBox_desiredlength);
        }

        private void label10_MouseUp(object sender, MouseEventArgs e)
        {
            toolTip4.Hide(textBox_desiredlength);
        }

        private void textBox_startpoint_Leave(object sender, EventArgs e)
        {
            try
            {
                string start_gps = "";
                if (textBox_startpoint.Text.IndexOf("[") > 0)
                    start_gps = textBox_startpoint.Text.Substring(0, textBox_startpoint.Text.IndexOf("[") - 2);
                else
                    start_gps = textBox_startpoint.Text;
                if (start_gps.Length < 4)
                {
                    MessageBox.Show("Please enter valid gps points");
                    return;
                }
                PointLatLng points_first_tmp = new PointLatLng();
                points_first_tmp = new PointLatLng(double.Parse(start_gps.Substring(0, start_gps.IndexOf(","))), double.Parse(start_gps.Substring(start_gps.IndexOf(",") + 2)));

                ///Draw Start End Points////////////////////////////////
                string FileName = string.Format("{0}icons\\stop2_33x33.png", Path.GetFullPath(RunningPath));
                FileName = string.Format("{0}icons\\start.png", Path.GetFullPath(Path.Combine(RunningPath)));
                gMarker_StartEnd = new GMap.NET.WindowsForms.Markers.GMarkerGoogle(new GMap.NET.PointLatLng(points_first_tmp.Lat, points_first_tmp.Lng), Bitmap.FromFile(FileName) as Bitmap);
                //gMarker_StartEnd = new GMap.NET.WindowsForms.Markers.GMarkerGoogle(new GMap.NET.PointLatLng(points_first.Lat, points_first.Lng), GMap.NET.WindowsForms.Markers.GMarkerGoogleType.green);
                gMarker_StartEnd.ToolTipText = "Start Point: " + textBox_startpoint.Text;
                gMapOverlay_StartEnd.Markers[0] = gMarker_StartEnd;
                gMapControl1.Invoke((MethodInvoker)(() => gMapControl1.Zoom += 1));
                gMapControl1.Invoke((MethodInvoker)(() => gMapControl1.Zoom -= 1));
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void textBox_endpoint_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox_endpoint_Leave(object sender, EventArgs e)
        {
            try
            {
                string end_gps = "";
                if (textBox_endpoint.Text.IndexOf("[") > 0)
                    end_gps = textBox_endpoint.Text.Substring(0, textBox_endpoint.Text.IndexOf("[") - 2);
                else
                    end_gps = textBox_endpoint.Text;
                if (end_gps.Length < 4)
                {
                    MessageBox.Show("Please enter valid gps points");
                    return;
                }
                PointLatLng points_end_tmp = new PointLatLng();
                points_end_tmp = new PointLatLng(double.Parse(end_gps.Substring(0, end_gps.IndexOf(","))), double.Parse(end_gps.Substring(end_gps.IndexOf(",") + 2)));

                ///Draw Start End Points////////////////////////////////
                string FileName = string.Format("{0}icons\\stop2_33x33.png", Path.GetFullPath(RunningPath));
                FileName = string.Format("{0}icons\\end.png", Path.GetFullPath(Path.Combine(RunningPath)));
                gMarker_StartEnd = new GMap.NET.WindowsForms.Markers.GMarkerGoogle(new GMap.NET.PointLatLng(points_end_tmp.Lat, points_end_tmp.Lng), Bitmap.FromFile(FileName) as Bitmap);
                //gMarker_StartEnd = new GMap.NET.WindowsForms.Markers.GMarkerGoogle(new GMap.NET.PointLatLng(points_last.Lat, points_last.Lng), GMap.NET.WindowsForms.Markers.GMarkerGoogleType.red);
                gMarker_StartEnd.ToolTipText = "End Point: " + textBox_startpoint.Text;
                gMapOverlay_StartEnd.Markers[1] = gMarker_StartEnd;
                gMapControl1.Invoke((MethodInvoker)(() => gMapControl1.Zoom += 1));
                gMapControl1.Invoke((MethodInvoker)(() => gMapControl1.Zoom -= 1));


            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            var fileContent = string.Empty;
            var filePath = string.Empty;

            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.InitialDirectory = RunningPath;// "c:\\";
                openFileDialog.Filter = "gpx files (*.gpx)|*.gpx";
                openFileDialog.FilterIndex = 1;
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    filePath = openFileDialog.FileName;
                    //var fileStream = openFileDialog.OpenFile();
                    /*using (StreamReader reader = new StreamReader(fileStream))
                    {
                        fileContent = reader.ReadToEnd();
                    }*/
                    gMapControl1.Overlays.Clear();
                    gMapControl1.Overlays.Add(gMapOverlay_StartEnd);
                    ParseGPXRoute(filePath);
                    ParseGPXSigns(filePath);
                    richTextBox_serverlogs.Invoke((MethodInvoker)(() => richTextBox_serverlogs.AppendText("Plotting to GUI " + filePath + "\n")));
                }
            }

            //MessageBox.Show(fileContent, "File Content at path: " + filePath, MessageBoxButtons.OK);
        }

        private void label_midpoint_MouseDown(object sender, MouseEventArgs e)
        {
            toolTip10.Show("Enter the route mid point manually (separated by commas) or right click on the map (click inside the box first)", textBox_midpoints);
        }

        private void label_midpoint_MouseUp(object sender, MouseEventArgs e)
        {
            toolTip10.Hide(textBox_midpoints);
        }

        private void textBox_midpoints_Enter(object sender, EventArgs e)
        {
            mapclickedindex = 2;
        }

        private void textBox_midpoints_Leave(object sender, EventArgs e)
        {

        }
        #endregion

        private void button2_Click_1(object sender, EventArgs e)
        {
            FolderBrowserDialog folderDlg = new FolderBrowserDialog();
            folderDlg.ShowNewFolderButton = true;
            // Show the FolderBrowserDialog.  
            DialogResult result = folderDlg.ShowDialog();
            if (result == DialogResult.OK)
            {
                string summeryfilename = "";
                string[] fileslist0 = Directory.GetFiles(folderDlg.SelectedPath, "summary.csv", SearchOption.AllDirectories);
                foreach (string strtmp in fileslist0)
                    if (strtmp.Contains("summary.csv"))
                        summeryfilename = strtmp;

                string [] fileslist = Directory.GetFiles(folderDlg.SelectedPath, "*.gpx", SearchOption.AllDirectories);
                foreach (string strtmp in fileslist)
                {
                    //route0_staticfeaturesfile_adjusted.gpx
                    if (strtmp.Contains("staticfeaturesfile") && !strtmp.Contains("adjusted"))
                    {
                        //MessageBox.Show(strtmp);
                        new Thread(delegate () {
                            adjustgpxfile(strtmp, summeryfilename);
                        }).Start();
                    }
                }
            }
        }
        private void adjustgpxfile(string filename, string filename_csv)
        {
            ParseGPXRoutethread(filename, filename_csv);
        }


        private void ParseGPXRoutethread(string filename, string filename_csv)
        {
            GMap.NET.WindowsForms.GMapOverlay gMapOverlay_HERE_routes = new GMapOverlay("Overlay_HERE_Routes");
            GMap.NET.WindowsForms.GMapOverlay gMapOverlay_Layer0_Route = new GMapOverlay("Overlay_Layer0_Route");
            GMap.NET.WindowsForms.GMapOverlay gMapOverlay_Layer0_Faetures = new GMap.NET.WindowsForms.GMapOverlay("Overlay_Layer0_Features");
            List<PointLatLng> points = new List<PointLatLng>();
            List<PointLatLng> points_adjusted = new List<PointLatLng>();
            try
            {
                XmlDocument gpxDoc = new XmlDocument();
                gpxDoc.Load(filename);
                XmlNamespaceManager nsmgr = new XmlNamespaceManager(gpxDoc.NameTable);
                nsmgr.AddNamespace("x", "http://www.topografix.com/GPX/1/1");

                string featuresstring = "";
                XmlNodeList nlfeatures = gpxDoc.SelectNodes("//x:wpt", nsmgr);
                int tmptmpcounter = 0;
                foreach (XmlNode xnode in nlfeatures)
                    featuresstring += xnode.OuterXml+"\n";
                //MessageBox.Show(featuresstring);
                XmlNodeList nl = gpxDoc.SelectNodes("//x:trkpt", nsmgr);
                string outerxml = "", SignType = "";
                double lat1 = 0.0, lon1 = 0.0;
                double latprev = 0.0, lonprev = 0.0;
                int counter = 0;

                string GPX_String = "";
                string GPX_String_nodes = "";
                string GPX_String2 = "";
                string GPX_String_rtept = "";
                GPX_String = @"<?xml version=""1.0"" encoding=""UTF-8"" standalone=""no"" ?>";
                GPX_String += "\n";
                GPX_String += @"<gpx xmlns=""http://www.topografix.com/GPX/1/1"" xmlns:gpxx=""http://www.garmin.com/xmlschemas/GpxExtensions/v3"" xmlns:gpxtpx=""http://www.garmin.com/xmlschemas/TrackPointExtension/v1"" ";
                GPX_String += @"creator=""mapstogpx.com"" version=""1.1"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xsi:schemaLocation=""http://www.topografix.com/GPX/1/1 http://www.topografix.com/GPX/1/1/gpx.xsd";
                GPX_String += @" http://www.garmin.com/xmlschemas/GpxExtensions/v3 http://www.garmin.com/xmlschemas/GpxExtensionsv3.xsd http://www.garmin.com/xmlschemas/TrackPointExtension/v1 http://www.garmin.com/xmlschemas/TrackPointExtensionv1.xsd"">";
                GPX_String += "\n";
                GPX_String += featuresstring;
                GPX_String += "<rte>";
                GPX_String += "\n";
                GPX_String += "\t<name>" + filename + "</name>";
                GPX_String += "\n";

                double distance_adjusted = 0.0;
                foreach (XmlNode xnode in nl)
                {
                    outerxml = xnode.OuterXml;
                    SignType = xnode.InnerText;
                    lat1 = Convert.ToDouble(outerxml.Substring(outerxml.IndexOf("lat") + 5, (outerxml.IndexOf("lon") - outerxml.IndexOf("lat") - 7)));
                    lon1 = Convert.ToDouble(outerxml.Substring(outerxml.IndexOf("lon") + 5, (outerxml.IndexOf("xmlns") - outerxml.IndexOf("lon") - 7)));

                    points.Add(new PointLatLng(lat1, lon1));
                    if (counter == 0)
                    {
                        points_first = new PointLatLng(lat1, lon1);
                        GPX_String_rtept += "\t" + @"<rtept lat=""" + lat1.ToString() + @""" lon=""" + lon1.ToString() + @"""></rtept>";
                        GPX_String_rtept += "\n";

                        GPX_String_nodes += "\t" + @"<wpt lat=""" + lat1.ToString() + @""" lon=""" + lon1.ToString() + @""">" + "<name>Start Point" + counter.ToString() + "</name>" + "</wpt>";
                        GPX_String_nodes += "\n";
                        points_adjusted.Add(new PointLatLng(lat1, lon1));
                    }
                    if (counter == nl.Count - 1)
                    {
                        points_last = new PointLatLng(lat1, lon1);
                        GPX_String_rtept += "\t" + @"<rtept lat=""" + lat1.ToString() + @""" lon=""" + lon1.ToString() + @"""></rtept>";
                        GPX_String_rtept += "\n";

                        GPX_String_nodes += "\t" + @"<wpt lat=""" + lat1.ToString() + @""" lon=""" + lon1.ToString() + @""">" + "<name>END Point" + counter.ToString() + "</name>" + "</wpt>";
                        GPX_String_nodes += "\n";
                        points_adjusted.Add(new PointLatLng(lat1, lon1));
                    }
                    counter++;

                    //Find Distance
                    if (counter > 1 && counter < nl.Count - 1)
                    {
                        var sCoord = new GeoCoordinate(lat1, lon1);
                        var eCoord = new GeoCoordinate(latprev, lonprev);
                        double distance = sCoord.GetDistanceTo(eCoord);
                        if (distance > 75)
                        {
                            PointLatLng point1tmp1 = new PointLatLng(latprev, lonprev);
                            PointLatLng point2tmp2 = new PointLatLng(lat1, lon1);
                            GDirections gDirections;
                            GMap.NET.MapProviders.GoogleMapProvider.Instance.GetDirections(out gDirections, point1tmp1, point2tmp2, false, false, false, true, false);
                            if (gDirections != null)
                            {
                                for (int i = 0; i < gDirections.Route.Count; i++)
                                {
                                    if (i == 0)
                                    {
                                        GPX_String_nodes += "\t" + @"<wpt lat=""" + latprev.ToString() + @""" lon=""" + lonprev.ToString() + @""">" + "<name>Good Start" + counter.ToString() + "</name>" + "</wpt>";
                                        GPX_String_nodes += "\n";

                                        GPX_String_rtept += "\t" + @"<rtept lat=""" + gDirections.Route[i].Lat.ToString() + @""" lon=""" + gDirections.Route[i].Lng.ToString() + @"""></rtept>";
                                        GPX_String_rtept += "\n";
                                    }
                                    else if (i == gDirections.Route.Count - 1)
                                    {
                                        GPX_String_nodes += "\t" + @"<wpt lat=""" + lat1.ToString() + @""" lon=""" + lon1.ToString() + @""">" + "<name>Good End" + counter.ToString() + "</name>" + "</wpt>";
                                        GPX_String_nodes += "\n";

                                        GPX_String_rtept += "\t" + @"<rtept lat=""" + gDirections.Route[i].Lat.ToString() + @""" lon=""" + gDirections.Route[i].Lng.ToString() + @"""></rtept>";
                                        GPX_String_rtept += "\n";
                                    }
                                    else
                                    {
                                        GPX_String_nodes += "\t" + @"<wpt lat=""" + gDirections.Route[i].Lat.ToString() + @""" lon=""" + gDirections.Route[i].Lng.ToString() + @""">" + "<name>Google Point" + counter.ToString() + "</name>" + "</wpt>";
                                        GPX_String_nodes += "\n";

                                        GPX_String_rtept += "\t" + @"<rtept lat=""" + gDirections.Route[i].Lat.ToString() + @""" lon=""" + gDirections.Route[i].Lng.ToString() + @"""></rtept>";
                                        GPX_String_rtept += "\n";
                                    }
                                    points_adjusted.Add(new PointLatLng(gDirections.Route[i].Lat, gDirections.Route[i].Lng));
                                }
                                distance_adjusted += gDirections.DistanceValue;
                            }
                            else
                            {
                                GPX_String_nodes += "\t" + @"<wpt lat=""" + latprev.ToString() + @""" lon=""" + lonprev.ToString() + @""">" + "<name>Not Good Start" + counter.ToString() + "</name>" + "</wpt>";
                                GPX_String_nodes += "\n";
                                GPX_String_nodes += "\t" + @"<wpt lat=""" + lat1.ToString() + @""" lon=""" + lon1.ToString() + @""">" + "<name>Not Good End" + counter.ToString() + "</name>" + "</wpt>";
                                GPX_String_nodes += "\n";

                                GPX_String_rtept += "\t" + @"<rtept lat=""" + latprev.ToString() + @""" lon=""" + lonprev.ToString() + @"""></rtept>";
                                GPX_String_rtept += "\n";
                                GPX_String_rtept += "\t" + @"<rtept lat=""" + lat1.ToString() + @""" lon=""" + lon1.ToString() + @"""></rtept>";
                                GPX_String_rtept += "\n";
                                distance_adjusted += distance;
                            }
                        }
                        else
                        {
                            GPX_String_nodes += "\t" + @"<wpt lat=""" + lat1.ToString() + @""" lon=""" + lon1.ToString() + @""">" + "<name>HERE Point" + counter.ToString() + "</name>" + "</wpt>";
                            GPX_String_nodes += "\n";
                            GPX_String_rtept += "\t" + @"<rtept lat=""" + lat1.ToString() + @""" lon=""" + lon1.ToString() + @"""></rtept>";
                            GPX_String_rtept += "\n";
                            distance_adjusted += distance;
                        }
                    }
                    latprev = lat1;
                    lonprev = lon1;
                }
                GPX_String2 = GPX_String;
                GPX_String2 += GPX_String_rtept;
                GPX_String2 += "</rte>\n";
                GPX_String += "</rte>\n";
                GPX_String += GPX_String_nodes;
                GPX_String += "</gpx>\n";
                GPX_String2 += "</gpx>\n";
                string newfilename = filename.Substring(0, filename.Length - 4) + "_adjusted_nodes" + ".gpx";
                string newfilename_repts = filename.Substring(0, filename.Length - 4) + "_adjusted_route" + ".gpx";
                WriteGpxFileAsync(newfilename, GPX_String);
                WriteGpxFileAsync(newfilename_repts, GPX_String2);

                List<string> newLines = new List<string>();
                newLines.Add(newfilename_repts+"_Distance= "+ distance_adjusted.ToString() +" meters");
                File.AppendAllLines(filename_csv, newLines);

                /*List<string> newColumnData = new List<string>() { "D" };
                string[] lines = File.ReadAllLines(filename_csv);

                if (lines.Length > 0)
                {
                    //add new column to the header row
                    //lines[0] += ",AdjustedDistance";

                    //add new column value for each row.
                    for (int i = 1; i < lines.Length; i++)
                    {
                        int newColumnDataIndex = i - 1;
                        if (newColumnDataIndex > newColumnData.Count)
                        {
                            throw new InvalidOperationException("Not enough data in newColumnData");
                        }

                        lines[i] += "," + newColumnData[newColumnDataIndex];
                    }
                    File.WriteAllLines(@"C:/CSV/test.csv", lines);
                }*/
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        private void runcommandpython()
        {
            System.Diagnostics.Process p = new System.Diagnostics.Process();
            p.StartInfo.FileName = "C:\\Windows\\system32\\cmd.exe";
            p.StartInfo.WorkingDirectory = @"D:\VinFast";
            //p.StartInfo.Arguments = "/C ping www.google.com -t50";
            p.StartInfo.Arguments = "/C python "+ python_code_path;
            p.Start();
            p.WaitForExit();
        }
        private void button4_Click(object sender, EventArgs e)
        {
            new Thread(delegate () {
                runcommandpython();
            }).Start();
        }
    }
}