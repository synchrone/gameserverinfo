using System;
using System.Collections.Generic;
using System.Text;
using System.Net;

namespace GameServerInfo
{
    class Samp : GameServerInfo.Protocol
    {
        byte[] iQuery = new byte[10];
        int port;

        public Samp(string _host, int _port)
        {
            base._protocol = GameProtocol.Samp;
           /* string[] ipParts = new string[4];

            try
            {
                IPAddress.Parse(_host);
                ipParts = _host.ToString().Split(".");
            }
            catch (FormatException) {
                ipParts = Dns.GetHostEntry(_host).AddressList[0].ToString().Split(".");
            }
            Array.Copy(Encoding.Default.GetBytes("SAMP"), 0, iQuery, 0, 4);
            Array.Copy(Encoding.Default.GetBytes(ipParts), 0, iQuery, 4, 4);
            iQuery[8] = (byte)(_port & 0xFF);
            iQuery[9] = (byte)(_port >> 8 & 0xFF);
            iQuery[10] = Encoding.Default.GetBytes("i")[0];*/
            iQuery = new byte[] { (byte)'S', (byte)'A', (byte)'M', (byte)'P', 0x21, 0x21, 0x21, 0x21, 0x00, 0x00, (byte)'i' };
            Connect(_host, _port);
            

        }

        public override void GetServerInfo()
        {
            if (!IsOnline) { return; }
            Query(Encoding.Default.GetString(iQuery));
            _params["passworded"]=(Response[11]==0 ? true : false).ToString();

            byte[] numPlayers = new byte[2];
            Array.Copy(Response,12,numPlayers,0,2);
            _params["numplayers"] = System.BitConverter.ToInt16(numPlayers,0).ToString();

            byte[] maxPlayers = new byte[2];
            Array.Copy(Response, 14, maxPlayers, 0, 2);
            _params["maxplayers"] = System.BitConverter.ToInt16(maxPlayers, 0).ToString();

            byte[] hostNameLength = new byte[4];
            Array.Copy(Response, 16, hostNameLength, 0, 4);
            int hostNameLengthInt = System.BitConverter.ToInt32(hostNameLength, 0);

            byte[] hostName = new byte[hostNameLengthInt];
            Array.Copy(Response, 20, hostName, 0, hostNameLengthInt);
            _params["hostname"] = Encoding.Default.GetString(hostName);
            _params["mapname"] = "San Andreas";
        }
    }
}
