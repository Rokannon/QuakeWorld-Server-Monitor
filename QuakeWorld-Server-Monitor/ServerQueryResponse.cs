using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace QuakeWorld_Server_Monitor
{
    public enum ServerQueryStatus
    {
        UNKNOWN,
        UP,
        ERROR,
        TIMEOUT
    }

    public class ServerQueryResponse
    {
        public ServerQueryStatus serverStatus;
        public string serverName;
        public string serverHostname;
        public int numPlayers;
        public int maxPlayers;
        public string serverError;

        public ServerQueryResponse()
        {
            Clear();
        }

        public void Parse(XElement xml)
        {
            serverHostname = xml.Element("server").Element("hostname").Value;
            switch (xml.Element("server").Attribute("status").Value)
            {
                case "UP":
                    serverStatus = ServerQueryStatus.UP;
                    break;

                case "ERROR":
                    serverStatus = ServerQueryStatus.ERROR;
                    break;

                case "TIMEOUT":
                    serverStatus = ServerQueryStatus.TIMEOUT;
                    break;
            }
            if (serverStatus == ServerQueryStatus.UP)
            {
                serverName = xml.Element("server").Element("name").Value;
                numPlayers = int.Parse(xml.Element("server").Element("numplayers").Value);
                maxPlayers = int.Parse(xml.Element("server").Element("maxplayers").Value);
            }
            else if (serverStatus == ServerQueryStatus.ERROR)
                serverError = xml.Element("server").Element("error").Value;
        }

        public void Clear()
        {
            serverHostname = null;
            serverStatus = ServerQueryStatus.UNKNOWN;
            serverName = null;
            numPlayers = 0;
            maxPlayers = 0;
            serverError = null;
        }
    }
}
