using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace MCDaemon
{
    //[Serializable]
    [XmlInclude(typeof(xmlServer))]
    public class XMLConfig
    {
        //hostname
        //port
        //ServerDirectory


        [XmlArray("Servers")]
        public List<xmlServer> ServerList = new List<xmlServer>();
        [XmlElement]
        public string Host;
        [XmlElement]
        public ushort Port;

        public XMLConfig()
        {

        }

        public XMLConfig(List<xmlServer> sl, string host="localhost", ushort port=80 )
        {
            ServerList = sl;
            Host = host;
            Port = port;
            VerifyValues();
        }

        public void VerifyValues()
        {
            if (Host == null || Host == "")
            {
                Host = "localhost";
            }

            if (Port < 0 || Port > ushort.MaxValue)
            {
                Port = 0; //dynamic allocate an available port
            }

        }



    }

    [XmlType("Server")]
    public class xmlServer
    {
        [XmlElement("Name")]
        public String Name;
        [XmlElement("FilePath")]
        public String FilePath;
        [XmlElement("AllowInput")]
        public bool AllowInput;

        public xmlServer()
        {
            Name = "Err";
            FilePath = "Err";
            AllowInput = false;
        }

        public xmlServer(String name, String filepath, bool input = false)
        {
            Name = name;
            FilePath = filepath;
            AllowInput = input;
        }
    }

    public static class XmlHandler
    {
        public static void Serialize(XMLConfig config)
        {
            Type[] contains = { typeof(xmlServer) };
            XmlSerializer x = new XmlSerializer(typeof(XMLConfig),contains);
            // TextWriter writer = new StreamWriter("config.xml");
            //x.Serialize(writer, config);
            using (StreamWriter streamWriter = System.IO.File.CreateText("config.xml"))
            {
                x.Serialize(streamWriter, config);
            }

        }

        public static XMLConfig Deserialize()
        {
            XMLConfig config;
            if (!File.Exists("config.xml"))
            {
                return new XMLConfig();
            }
            Type[] contains = { typeof(xmlServer) };
            XmlSerializer x = new XmlSerializer(typeof(XMLConfig), contains);
            using (StreamReader streamreader = System.IO.File.OpenText("config.xml"))
            {
                try
                {
                    config = (XMLConfig)x.Deserialize(streamreader);
                    config.VerifyValues();
                }
                catch (Exception)
                {
                    Console.WriteLine("Error deserializing XMLconfig from config.xml");
                    config = new XMLConfig();
                    throw;
                }

            }

            return config;
        }
    }
}
