using System.Net;using System.Net.Sockets;using System.Text;using System.Collections.Concurrent;

record Player(string Id,TcpClient Client,NetworkStream Stream,float X,float Y,float Z,float Yaw,int Hp,int Level);
class Program{
 const int Port=28080; static readonly ConcurrentDictionary<string,Player> Players=new(); static int NextId;
 static async Task Main(){var listener=new TcpListener(IPAddress.Any,Port);listener.Start();Console.WriteLine($"KAISAR MMO authoritative server listening on {Port}");while(true){var c=await listener.AcceptTcpClientAsync();_=Handle(c);}}
 static async Task Handle(TcpClient c){c.NoDelay=true;using c;using var s=c.GetStream();var id="P"+Interlocked.Increment(ref NextId);var p=new Player(id,c,s,0,0,0,0,100,1);Players[id]=p;await Send(s,$"{{\"type\":\"login\",\"ok\":true,\"playerId\":\"{id}\"}}\n");var buf=new byte[8192];var sb=new StringBuilder();try{while(true){int n=await s.ReadAsync(buf);if(n<=0)break;sb.Append(Encoding.UTF8.GetString(buf,0,n));while(true){var str=sb.ToString();int i=str.IndexOf('\n');if(i<0)break;var line=str[..i];sb.Remove(0,i+1);Process(p,line);}}}catch{}finally{Players.TryRemove(id,out _);}}
 static void Process(Player p,string line){try{if(line.Contains("\"x\"")){using var doc=System.Text.Json.JsonDocument.Parse(line);var r=doc.RootElement;float x=r.GetProperty("x").GetSingle();float z=r.GetProperty("z").GetSingle();float yaw=r.GetProperty("yaw").GetSingle();float dx=x-p.X,dz=z-p.Z;float max=.8f;if(MathF.Sqrt(dx*dx+dz*dz)>max){x=p.X+Math.Clamp(dx,-max,max);z=p.Z+Math.Clamp(dz,-max,max);}var np=p with {X=x,Z=z,Yaw=yaw};Players[p.Id]=np;BroadcastSnapshot(np);}}catch{}}
 static void BroadcastSnapshot(Player p){string msg=$"{{\"type\":\"snapshot\",\"playerId\":\"{p.Id}\",\"x\":{p.X},\"y\":{p.Y},\"z\":{p.Z},\"yaw\":{p.Yaw},\"hp\":{p.Hp},\"level\":{p.Level}}}\n";byte[] b=Encoding.UTF8.GetBytes(msg);foreach(var q in Players.Values)try{q.Stream.Write(b); }catch{}}
 static Task Send(NetworkStream s,string m){var b=Encoding.UTF8.GetBytes(m);return s.WriteAsync(b);}
}
