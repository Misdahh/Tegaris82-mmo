using System;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;

namespace KaisarMMO.Networking
{
    public sealed class ClientConnection : MonoBehaviour
    {
        public static ClientConnection Instance { get; private set; }
        public bool Connected => _client != null && _client.Connected;
        public event Action<string> MessageReceived;
        TcpClient _client; NetworkStream _stream; Thread _reader; volatile bool _running;

        void Awake(){ if(Instance!=null){Destroy(gameObject);return;} Instance=this; DontDestroyOnLoad(gameObject); }
        public void Connect(string host,int port){ if(Connected)return; _client=new TcpClient(); _client.NoDelay=true; _client.Connect(host,port); _stream=_client.GetStream(); _running=true; _reader=new Thread(ReadLoop){IsBackground=true}; _reader.Start(); }
        public void Send(string json){ if(!Connected)return; var data=Encoding.UTF8.GetBytes(json+"\n"); _stream.Write(data,0,data.Length); }
        void ReadLoop(){var buffer=new byte[8192]; var sb=new StringBuilder(); try{while(_running){int n=_stream.Read(buffer,0,buffer.Length);if(n<=0)break;sb.Append(Encoding.UTF8.GetString(buffer,0,n));string s=sb.ToString();int i;while((i=s.IndexOf('\n'))>=0){MessageReceived?.Invoke(s.Substring(0,i));s=s.Substring(i+1);}sb.Clear();sb.Append(s);}}catch(Exception e){Debug.Log("Network closed: "+e.Message);} }
        void OnDestroy(){_running=false;try{_stream?.Close();_client?.Close();}catch{} }
    }
}
