﻿using System;
using System.Linq;
using System.Threading;
using Hylasoft.Opc.Common;
using Hylasoft.Opc.Ua;
using System.Configuration;
using Opc.Ua.Client;
using Opc.Ua;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Text;
using Hylasoft.Opc.Da;

namespace OSHMI_OPC_Client
{
    class Program
    {
        public struct OPC_entry
        {
            public string opc_path;
            public string oshmi_tag;
            public string opc_type; 
        }

        static void SendUdp(byte[] data)
        {
            using (UdpClient c = new UdpClient())
                c.Send(data, data.Length, "127.0.0.1", 9100);
        }

        static void ProcessUa(String URI, OPC_entry[] entries, int readperiod)
        {
            do
            {
                try
                {
                    var options = new UaClientOptions
                    {
                        SessionTimeout = 60000,
                        SessionName = "OSHMI client (h-opc)"
                    };

                    using (var client = new UaClient(new Uri(URI), options))
                    {
                        client.Connect();
                        Console.WriteLine("Connect UA " + URI);
                        
                        foreach (OPC_entry entry in entries)
                        {
                            string tag;
                            if (entry.oshmi_tag == "")
                                tag = entry.opc_path;
                            else
                                tag = entry.oshmi_tag;

                            string stype;
                            if (entry.opc_type == "")
                                // this function fails for opc DA returning only "System.Int16", so plese define the type manually
                                stype = client.GetDataType(entry.opc_path).ToString().ToLower();
                            else
                                stype = entry.opc_type.ToLower();

                            switch (stype)
                            {
                                case "bool":
                                case "boolean":
                                case "system.boolean":
                                    {
                                        client.Monitor<Boolean>(entry.opc_path, (readEvent, unsubscribe) =>
                                        {
                                            bool bval = readEvent.Value;
                                            string sval = bval.ToString().ToLower();
                                            Console.WriteLine("EVENT " + URI + " " + entry.opc_path + " " + tag + " " + sval);
                                            SendUdp(Encoding.ASCII.GetBytes("{\"" + tag + "\" : " + sval + "}"));
                                        });
                                    }
                                    break;
                                case "float":
                                case "single":
                                case "system.single":
                                    {
                                        client.Monitor<Single>(entry.opc_path, (readEvent, unsubscribe) =>
                                        {
                                            Console.WriteLine("EVENT " + URI + " " + entry.opc_path + " " + tag + " " + readEvent.Value);
                                            SendUdp(Encoding.ASCII.GetBytes("{\"" + tag + "\" : " + readEvent.Value + "}"));
                                        });
                                    }
                                    break;
                                case "double":
                                case "system.double":
                                    {
                                        client.Monitor<Double>(entry.opc_path, (readEvent, unsubscribe) =>
                                        {
                                            Console.WriteLine("EVENT " + URI + " " + entry.opc_path + " " + tag + " " + readEvent.Value);
                                            SendUdp(Encoding.ASCII.GetBytes("{\"" + tag + "\" : " + readEvent.Value + "}"));
                                        });
                                    }
                                    break;
                                case "system.decimal":
                                    {
                                        client.Monitor<Decimal>(entry.opc_path, (readEvent, unsubscribe) =>
                                        {
                                            Decimal dval = readEvent.Value;
                                            string sval = dval.ToString().ToLower();
                                            Console.WriteLine("EVENT " + URI + " " + entry.opc_path + " " + tag + " " + sval);
                                            SendUdp(Encoding.ASCII.GetBytes("{\"" + tag + "\" : " + sval + "}"));
                                        });
                                    }
                                    break;
                                case "byte":
                                case "system.byte":
                                    {
                                        client.Monitor<Byte>(entry.opc_path, (readEvent, unsubscribe) =>
                                        {
                                            Console.WriteLine("EVENT " + URI + " " + entry.opc_path + " " + tag + " " + readEvent.Value);
                                            SendUdp(Encoding.ASCII.GetBytes("{\"" + tag + "\" : " + readEvent.Value + "}"));
                                        });
                                    }
                                    break;
                                case "sbyte":
                                case "system.sbyte":
                                    {
                                        client.Monitor<SByte>(entry.opc_path, (readEvent, unsubscribe) =>
                                        {
                                            Console.WriteLine("EVENT " + URI + " " + entry.opc_path + " " + tag + " " + readEvent.Value);
                                            SendUdp(Encoding.ASCII.GetBytes("{\"" + tag + "\" : " + readEvent.Value + "}"));
                                        });
                                    }
                                    break;
                                case "int16":
                                case "system.int16":
                                    {
                                        client.Monitor<Int16>(entry.opc_path, (readEvent, unsubscribe) =>
                                        {
                                            Console.WriteLine("EVENT " + URI + " " + entry.opc_path + " " + tag + " " + readEvent.Value);
                                            SendUdp(Encoding.ASCII.GetBytes("{\"" + tag + "\" : " + readEvent.Value + "}"));
                                        });
                                    }
                                    break;
                                case "uint16":
                                case "system.uint16":
                                    {
                                        client.Monitor<UInt16>(entry.opc_path, (readEvent, unsubscribe) =>
                                        {
                                            Console.WriteLine("EVENT " + URI + " " + entry.opc_path + " " + tag + " " + readEvent.Value);
                                            SendUdp(Encoding.ASCII.GetBytes("{\"" + tag + "\" : " + readEvent.Value + "}"));
                                        });
                                    }
                                    break;
                                case "integer":
                                case "int32":
                                case "system.int32":
                                    {
                                        client.Monitor<Int32>(entry.opc_path, (readEvent, unsubscribe) =>
                                        {
                                            Console.WriteLine("EVENT " + URI + " " + entry.opc_path + " " + tag + " " + readEvent.Value);
                                            SendUdp(Encoding.ASCII.GetBytes("{\"" + tag + "\" : " + readEvent.Value + "}"));
                                        });
                                    }
                                    break;
                                case "uint32":
                                case "system.uint32":
                                    {
                                        client.Monitor<UInt32>(entry.opc_path, (readEvent, unsubscribe) =>
                                        {
                                            Console.WriteLine("EVENT " + URI + " " + entry.opc_path + " " + tag + " " + readEvent.Value);
                                            SendUdp(Encoding.ASCII.GetBytes("{\"" + tag + "\" : " + readEvent.Value + "}"));
                                        });
                                    }
                                    break;
                                case "int64":
                                case "system.int64":
                                    {
                                        client.Monitor<Int64>(entry.opc_path, (readEvent, unsubscribe) =>
                                        {
                                            Console.WriteLine("EVENT " + URI + " " + entry.opc_path + " " + tag + " " + readEvent.Value);
                                            SendUdp(Encoding.ASCII.GetBytes("{\"" + tag + "\" : " + readEvent.Value + "}"));
                                        });
                                    }
                                    break;
                                case "uint64":
                                case "system.uint64":
                                    {
                                        client.Monitor<UInt64>(entry.opc_path, (readEvent, unsubscribe) =>
                                        {
                                            Console.WriteLine("EVENT " + URI + " " + entry.opc_path + " " + tag + " " + readEvent.Value);
                                            SendUdp(Encoding.ASCII.GetBytes("{\"" + tag + "\" : " + readEvent.Value + "}"));
                                        });
                                    }
                                    break;
                                case "time":
                                case "date":
                                case "datetime":
                                case "system.datetime":
                                    {
                                        client.Monitor<DateTime>(entry.opc_path, (readEvent, unsubscribe) =>
                                        {
                                            DateTime dt = readEvent.Value;
                                            string sval = dt.Ticks.ToString();
                                            Console.WriteLine("EVENT " + URI + " " + entry.opc_path + " " + tag + " " + sval);
                                            SendUdp(Encoding.ASCII.GetBytes("{\"" + tag + "\" : " + sval + "}"));
                                        });
                                    }
                                    break;
                            }
                        }
                        
                        do
                        {
                            foreach (OPC_entry entry in entries)
                            {
                                string tag;
                                if (entry.oshmi_tag == "")
                                    tag = entry.opc_path;
                                else
                                    tag = entry.oshmi_tag;

                                string stype;
                                if (entry.opc_type == "")
                                    // this function fails for opc DA returning only "System.Int16", so plese define the type manually
                                    stype = client.GetDataType(entry.opc_path).ToString().ToLower();
                                else
                                    stype = entry.opc_type.ToLower();

                                double val = 0;
                                string sval = "";
                                bool fail = false;

                                switch (stype)
                                {
                                    case "bool":
                                    case "boolean":
                                    case "system.boolean":
                                        {
                                            var task = client.ReadAsync<Boolean>(entry.opc_path);
                                            task.Wait();
                                            bool bval = task.Result.Value;
                                            sval = bval.ToString().ToLower();
                                        }
                                        break;
                                    case "float":
                                    case "single":
                                    case "system.single":
                                        {
                                            var task = client.ReadAsync<Single>(entry.opc_path);
                                            task.Wait();
                                            val = task.Result.Value;
                                            sval = val.ToString();
                                        }
                                        break;
                                    case "double":
                                    case "system.double":
                                        {
                                            var task = client.ReadAsync<double>(entry.opc_path);
                                            task.Wait();
                                            val = task.Result.Value;
                                            sval = val.ToString();
                                        }
                                        break;
                                    case "system.decimal":
                                        {
                                            var task = client.ReadAsync<Decimal>(entry.opc_path);
                                            task.Wait();
                                            Decimal dval = task.Result.Value;
                                            sval = dval.ToString();
                                        }
                                        break;
                                    case "byte":
                                    case "system.byte":
                                        {
                                            var task = client.ReadAsync<Byte>(entry.opc_path);
                                            task.Wait();
                                            val = task.Result.Value;
                                            sval = val.ToString();
                                        }
                                        break;
                                    case "sbyte":
                                    case "system.sbyte":
                                        {
                                            var task = client.ReadAsync<SByte>(entry.opc_path);
                                            task.Wait();
                                            val = task.Result.Value;
                                            sval = val.ToString();
                                        }
                                        break;
                                    case "int16":
                                    case "system.int16":
                                        {
                                            var task = client.ReadAsync<Int16>(entry.opc_path);
                                            task.Wait();
                                            val = task.Result.Value;
                                            sval = val.ToString();
                                        }
                                        break;
                                    case "uint16":
                                    case "system.uint16":
                                        {
                                            var task = client.ReadAsync<UInt16>(entry.opc_path);
                                            task.Wait();
                                            val = task.Result.Value;
                                            sval = val.ToString();
                                        }
                                        break;
                                    case "integer":
                                    case "int32":
                                    case "system.int32":
                                        {
                                            var task = client.ReadAsync<Int32>(entry.opc_path);
                                            task.Wait();
                                            val = task.Result.Value;
                                            sval = val.ToString();
                                        }
                                        break;
                                    case "uint32":
                                    case "system.uint32":
                                        {
                                            var task = client.ReadAsync<UInt32>(entry.opc_path);
                                            task.Wait();
                                            val = task.Result.Value;
                                            sval = val.ToString();
                                        }
                                        break;
                                    case "int64":
                                    case "system.int64":
                                        {
                                            var task = client.ReadAsync<Int64>(entry.opc_path);
                                            task.Wait();
                                            val = task.Result.Value;
                                            sval = val.ToString();
                                        }
                                        break;
                                    case "uint64":
                                    case "system.uint64":
                                        {
                                            var task = client.ReadAsync<UInt64>(entry.opc_path);
                                            task.Wait();
                                            val = task.Result.Value;
                                            sval = val.ToString();
                                        }
                                        break;
                                    case "time":
                                    case "date":
                                    case "datetime":
                                    case "system.datetime":
                                        {
                                            var task = client.ReadAsync<DateTime>(entry.opc_path);
                                            task.Wait();
                                            DateTime dt = task.Result.Value;
                                            val = dt.Ticks;
                                            sval = val.ToString();
                                        }
                                        break;
                                    default:
                                        Console.WriteLine("READ UNSUPPORTED TYPE: " + URI + " " + entry.opc_path + " " + stype);
                                        fail = true;
                                        break;
                                }

                                if (!fail)
                                {
                                    SendUdp(Encoding.ASCII.GetBytes("{\"" + tag + "\" : " + sval + "}"));
                                    Console.WriteLine("READ  " + URI + " " + entry.opc_path + " " + tag + " " + sval);
                                }
                            }

                            System.Threading.Thread.Sleep(readperiod * 1000);
                        } while (true);
                    }
                }
                catch (Exception e)
                {
                    // EXCEPTION HANDLER
                    Console.WriteLine("Exception UA " + URI);
                    Console.WriteLine(e);
                    System.Threading.Thread.Sleep(15000);
                }
            } while (true);
        }

        static void ProcessDa(String URI, OPC_entry[] entries, int readperiod)
        {
            do
            {
                try
                {
                    using (var client = new DaClient(new Uri(URI)))
                    {
                        client.Connect();
                        Console.WriteLine("Connect DA " + URI);
                        
                        foreach (OPC_entry entry in entries)
                        {
                            string tag;
                            if (entry.oshmi_tag == "")
                                tag = entry.opc_path;
                            else
                                tag = entry.oshmi_tag;

                            string stype;
                            if (entry.opc_type == "")
                                // this function fails for opc DA returning only "System.Int16", so plese define the type manually
                                stype = client.GetDataType(entry.opc_path).ToString().ToLower();
                            else
                                stype = entry.opc_type.ToLower();

                            switch (stype)
                            {
                                case "vt_bool":
                                case "bool":
                                case "system.boolean":
                                    {
                                        client.Monitor<Boolean>(entry.opc_path, (readEvent, unsubscribe) =>
                                        {
                                            bool bval = readEvent.Value;
                                            string sval = bval.ToString().ToLower();
                                            SendUdp(Encoding.ASCII.GetBytes("{\"" + tag + "\" : " + sval + "}"));
                                            Console.WriteLine("EVENT " + URI + " " + entry.opc_path + " " + tag + " " + sval );
                                        });
                                    }
                                    break;
                                case "vt_r4":
                                case "system.double":
                                    {
                                        client.Monitor<Single>(entry.opc_path, (readEvent, unsubscribe) =>
                                        {
                                            SendUdp(Encoding.ASCII.GetBytes("{\"" + tag + "\" : " + readEvent.Value + "}"));
                                            Console.WriteLine("EVENT " + URI + " " + entry.opc_path + " " + tag + " " + readEvent.Value);
                                        });
                                    }
                                    break;
                                case "vt_r8":
                                case "float":
                                case "System.Double":
                                    {
                                        client.Monitor<double>(entry.opc_path, (readEvent, unsubscribe) =>
                                        {
                                            SendUdp(Encoding.ASCII.GetBytes("{\"" + tag + "\" : " + readEvent.Value + "}"));
                                            Console.WriteLine("EVENT " + URI + " " + entry.opc_path + " " + tag + " " + readEvent.Value);
                                        });
                                    }
                                    break;
                                case "vt_i1":
                                case "byte":
                                case "system.byte":
                                    {
                                        client.Monitor<Byte>(entry.opc_path, (readEvent, unsubscribe) =>
                                        {
                                            SendUdp(Encoding.ASCII.GetBytes("{\"" + tag + "\" : " + readEvent.Value + "}"));
                                            Console.WriteLine("EVENT " + URI + " " + entry.opc_path + " " + tag + " " + readEvent.Value);
                                        });
                                    }
                                    break;
                                case "vt_i2":
                                case "int16":
                                case "system.int16":
                                    {
                                        client.Monitor<Int16>(entry.opc_path, (readEvent, unsubscribe) =>
                                        {
                                            SendUdp(Encoding.ASCII.GetBytes("{\"" + tag + "\" : " + readEvent.Value + "}"));
                                            Console.WriteLine("EVENT " + URI + " " + entry.opc_path + " " + tag + " " + readEvent.Value);
                                        });
                                    }
                                    break;
                                case "state":
                                case "vt_i4":
                                case "int32":
                                case "integer":
                                case "system.int32":
                                    {
                                        client.Monitor<Int32>(entry.opc_path, (readEvent, unsubscribe) =>
                                        {
                                            SendUdp(Encoding.ASCII.GetBytes("{\"" + tag + "\" : " + readEvent.Value + "}"));
                                            Console.WriteLine("EVENT " + URI + " " + entry.opc_path + " " + tag + " " + readEvent.Value);
                                        });
                                    }
                                    break;
                                case "vt_date":
                                case "date":
                                case "datetime":
                                case "system.datetime":
                                    {
                                        client.Monitor<DateTime>(entry.opc_path, (readEvent, unsubscribe) =>
                                        {
                                            DateTime dt = readEvent.Value;
                                            string sval = dt.Ticks.ToString();
                                            SendUdp(Encoding.ASCII.GetBytes("{\"" + tag + "\" : " + sval + "}"));
                                            Console.WriteLine("EVENT " + URI + " " + entry.opc_path + " " + tag + " " + sval);
                                        });
                                    }
                                    break;
                            }
                        }
                        
                        do
                        {
                            foreach (OPC_entry entry in entries)
                            {
                                string tag;
                                if (entry.oshmi_tag == "")
                                    tag = entry.opc_path;
                                else
                                    tag = entry.oshmi_tag;

                                string stype;
                                if (entry.opc_type == "")
                                    // this function fails for opc DA returning only "System.Int16", so plese define the type manually
                                    stype = client.GetDataType(entry.opc_path).ToString().ToLower();
                                else
                                    stype = entry.opc_type.ToLower();

                                double val = 0;                                
                                string sval = "";
                                bool fail = false;

                                switch (stype)
                                {
                                    case "vt_bool":
                                    case "bool":
                                    case "system.boolean":
                                        {
                                            var task = client.ReadAsync<Boolean>(entry.opc_path);
                                            task.Wait();
                                            bool bval = task.Result.Value;
                                            sval = bval.ToString().ToLower();
                                        }
                                        break;
                                    case "vt_r4":
                                    case "system.single":
                                        {
                                            var task = client.ReadAsync<Single>(entry.opc_path);
                                            task.Wait();
                                            val = task.Result.Value;
                                            sval = val.ToString();
                                        }
                                        break;
                                    case "vt_r8":
                                    case "float":
                                    case "system.double":
                                        {
                                            var task = client.ReadAsync<double>(entry.opc_path);
                                            task.Wait();
                                            val = task.Result.Value;
                                            sval = val.ToString();
                                        }
                                        break;
                                    case "vt_i1":
                                    case "byte":
                                    case "system.byte":
                                        {
                                            var task = client.ReadAsync<Byte>(entry.opc_path);
                                            task.Wait();
                                            val = task.Result.Value;
                                            sval = val.ToString();
                                        }
                                        break;
                                    case "vt_i2":
                                    case "int16":
                                    case "system.int16":
                                        {
                                            var task = client.ReadAsync<Int16>(entry.opc_path);
                                            task.Wait();
                                            val = task.Result.Value;
                                            sval = val.ToString();
                                        }
                                        break;
                                    case "state":
                                    case "vt_i4":
                                    case "int32":
                                    case "integer":
                                    case "system.int32":
                                        {
                                            var task = client.ReadAsync<Int32>(entry.opc_path);
                                            task.Wait();
                                            val = task.Result.Value;
                                            sval = val.ToString();
                                        }
                                        break;
                                    case "vt_date":
                                    case "date":
                                    case "datetime":
                                    case "system.datetime":
                                        {
                                            var task = client.ReadAsync<DateTime>(entry.opc_path);
                                            task.Wait();
                                            DateTime dt = task.Result.Value;
                                            val = dt.Ticks;
                                            sval = val.ToString();
                                        }
                                        break;
                                    default:
                                        Console.WriteLine("READ UNSUPPORTED TYPE: " + URI + " " + entry.opc_path + " " + stype);
                                        fail = true;
                                        break;
                                }

                                if (!fail)
                                {
                                    SendUdp(Encoding.ASCII.GetBytes("{\"" + tag + "\" : " + sval + "}"));
                                    Console.WriteLine("READ  " + URI + " " + entry.opc_path + " " + tag  + " " + sval);
                                }
                            }

                            System.Threading.Thread.Sleep(readperiod * 1000);
                        } while (true);
                    }
                }
                catch (Exception e)
                {
                    // EXCEPTION HANDLER
                    Console.WriteLine("Exception DA " + URI);
                    Console.WriteLine(e);
                    System.Threading.Thread.Sleep(15000);
                }
            } while (true);
        }

        static void Main(string[] args)
        {
            OPC_entry[] entriesUA1 =
            new OPC_entry[]{
                //new OPC_entry() { opc_path = "ns=2;s=Demo.Dynamic.Scalar.Double", oshmi_tag = "", opc_type = "" },
                //new OPC_entry() { opc_path = "ns=2;s=Demo.BoilerDemo.Boiler1.FillLevelSensor.FillLevel", oshmi_tag = "", opc_type = "" },
                //new OPC_entry() { opc_path = "ns=2;s=Demo.Dynamic.Scalar.Boolean", oshmi_tag = "", opc_type = "" },
                new OPC_entry() { opc_path = "ns=2;s=Demo.Dynamic.Scalar.Byte", oshmi_tag = "", opc_type = "" },
                //new OPC_entry() { opc_path = "ns=2;s=Demo.Dynamic.Scalar.SByte", oshmi_tag = "", opc_type = "" },
                new OPC_entry() { opc_path = "ns=2;s=Demo.Dynamic.Scalar.Int16", oshmi_tag = "", opc_type = "" }
            };
            String URIUa1 = "opc.tcp://opcuaserver.com:48010";
            Thread tUa = new Thread(() => ProcessUa(URIUa1, entriesUA1, 15));
            tUa.Start();

            OPC_entry[] entriesUA2 =
            new OPC_entry[]{
                new OPC_entry() { opc_path = "ns=5;s=Random1", oshmi_tag = "Random1", opc_type = "" },
                new OPC_entry() { opc_path = "ns=5;s=Sinusoid1", oshmi_tag = "Sinusoid1", opc_type = "" },
                new OPC_entry() { opc_path = "ns=3;s=BooleanDataItem", oshmi_tag = "BooleanDataItem", opc_type = "" },
                new OPC_entry() { opc_path = "ns=3;s=Int16DataItem", oshmi_tag = "Int16DataItem", opc_type = "" },
                new OPC_entry() { opc_path = "ns=3;s=Int32DataItem", oshmi_tag = "Int32DataItem", opc_type = "" },
                new OPC_entry() { opc_path = "ns=3;s=Int64DataItem", oshmi_tag = "Int64DataItem", opc_type = "" },
                new OPC_entry() { opc_path = "ns=3;s=UInt16DataItem", oshmi_tag = "UInt16DataItem", opc_type = "" },
                new OPC_entry() { opc_path = "ns=3;s=UInt32DataItem", oshmi_tag = "UInt32DataItem", opc_type = "" },
                new OPC_entry() { opc_path = "ns=3;s=UInt64DataItem", oshmi_tag = "UInt64DataItem", opc_type = "" },
                // new OPC_entry() { opc_path = "ns=3;s=StringDataItem", oshmi_tag = "", opc_type = "" },
                new OPC_entry() { opc_path = "ns=3;s=ByteDataItem", oshmi_tag = "ByteDataItem", opc_type = "" },
                new OPC_entry() { opc_path = "ns=3;s=SByteDataItem", oshmi_tag = "SByteDataItem", opc_type = "" },
                new OPC_entry() { opc_path = "ns=3;s=DoubleDataItem", oshmi_tag = "DoubleDataItem", opc_type = "" },
                new OPC_entry() { opc_path = "ns=3;s=DateTimeDataItem", oshmi_tag = "DateTimeDataItem", opc_type = "" },
                new OPC_entry() { opc_path = "ns=5;s=Counter1", oshmi_tag = "Counter1", opc_type = "" }
            };
            String URIUa2 = "opc.tcp://LAPTOP-GSJBD1FP:53530/OPCUA/SimulationServer";
            Thread tUa2 = new Thread(() => ProcessUa(URIUa2, entriesUA2, 15));
            tUa2.Start();

 
            OPC_entry[] entriesDA =
            new OPC_entry[]{
                new OPC_entry() { opc_path = "Random.PsFloat1", oshmi_tag = "Random.PsFloat1" , opc_type = "float" },
                new OPC_entry() { opc_path = "Random.PsInteger1", oshmi_tag = "Random.PsInteger1" , opc_type = "vt_i4" },
                new OPC_entry() { opc_path = "Random.PsBool1", oshmi_tag = "Random.PsBool1" , opc_type = "bool" },
                new OPC_entry() { opc_path = "Random.PsState1", oshmi_tag = "Random.PsState1" , opc_type = "state" },
                new OPC_entry() { opc_path = "Random.PsDateTime1", oshmi_tag = "Random.PsDateTime1" , opc_type = "datetime" }
            };
            String URIda = "opcda://localhost/Prosys.OPC.Simulation";
            Thread tDa = new Thread(() => ProcessDa(URIda, entriesDA, 5));
            tDa.Start();

        }
    }
}