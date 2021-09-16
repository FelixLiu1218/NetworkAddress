using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Security;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;

namespace Address
{
    class Program
    {
        //create address class
        class Address
        {
            private String ipAddress;
            private String subNetworkMask;

            public Address(String ipAddress, String subNetworkMask)
            {
                this.ipAddress = ipAddress;
                this.subNetworkMask = subNetworkMask;
            }

            public string getIp()
            {
                return ipAddress;
            }

            public string getSub()
            {
                return subNetworkMask;
            }
        }


        //convert Decimal to Binary
        public static String Binary(String number)
        {
            int[] binary = new int[8];

            //remember to add a try
            try
            {
                int n = Convert.ToInt32(number);
                for (int i = 0; n > 0; i++)
                {
                    binary[i] = n % 2;
                    n /= 2;
                }
            }
            catch(FormatException)
            {
                //Console.WriteLine("ohhh boom!");
            }
                


            String result = String.Join("",binary.ToArray());
            return new string(result.Reverse().ToArray());
        }


        //convert Binary to Decimal
        public static int Decimal(String number)
        {
            int a = 1;
            int number_dec = 0;
            
            for (int i = number.Length -1; i >= 0; i--)
            {
                if (number[i] == '1')
                {
                    number_dec = number_dec + 1*a;
                    a = a * 2;
                }
                else
                {
                    number_dec = number_dec + 0;
                    a = a * 2;
                }
            }

            return number_dec;
        }

        static void Main(string[] args)
        {
            String ip;
            String sub;
            Console.Write("Server IP Address:");
            ip = Convert.ToString(Console.ReadLine());
            Console.Write("Server Subnetwork Mask:");
            sub = Convert.ToString(Console.ReadLine());
            Address Server = new Address(ip, sub);

            Console.Write("Client IP Address:");
            ip = Convert.ToString(Console.ReadLine());
            Console.Write("Client Subnetwork Mask:");
            sub = Convert.ToString(Console.ReadLine());
            Address Client = new Address(ip, sub);


            //split and add to list
            String number;
            List<String> Server_list = new List<string>();
            List<String> Client_list = new List<string>();
            List<String> Server_ip = new List<string>();
            List<String> Server_sub = new List<string>();
            List<String> Client_ip = new List<string>();
            List<String> Client_sub = new List<string>();
            for (int i = 0; i < 2; i++)
            {
                if (i == 0)
                {
                    for (int j = 0; j < 1; j++)
                    {
                        number = Server.getIp();
                        String[] nums = number.Split(".");
                        Server_ip.AddRange(nums);
                    }
                }
                else if ( i ==1)
                {
                    for (int k = 0; k < 1; k++)
                    {
                        number = Server.getSub();
                        String[] nums = number.Split(".");
                        Server_sub.AddRange(nums);
                    }
                }
            }
            Server_list.AddRange(Server_ip);
            Server_list.AddRange(Server_sub);


            for (int i = 0; i < 2; i++)
            {
                if (i == 0)
                {
                    for (int j = 0; j < 1; j++)
                    {
                        number = Client.getIp();
                        String[] nums = number.Split(".");
                        Client_ip.AddRange(nums);
                    }
                }
                else if (i == 1)
                {
                    for (int k = 0; k < 1; k++)
                    {
                        number = Client.getSub();
                        String[] nums = number.Split(".");
                            Client_sub.AddRange(nums);
                    }
                }
            }
            Client_list.AddRange(Client_ip);
            Client_list.AddRange(Client_sub);


            //from decimal to binary
            List<String> Server_list_bin = new List<string>();
            List<String> Client_list_bin = new List<string>();
            for (int i = 0; i < Server_list.Count; i++)
            {
                number = Binary(Server_list[i]);
                Server_list_bin.Add(number);
            }
            for (int i = 0; i < Client_list.Count; i++)
            {
                number = Binary(Client_list[i]);
                Client_list_bin.Add(number);
            }

            //add result(bin) to list
            List<String> Server_result_bin = new List<string>();
            List<String> Client_result_bin = new List<string>();

            //part of 255(server) ONE
            int count = 0;
            for (int i = 4; i < Server_list.Count; i++)
            {
                if (Server_list[i] == "255")
                {
                    count++;
                }
            }
            if (count != 0)
            {
                for (int i = 0; i < count; i++)
                {
                    Server_result_bin.Add(Server_list_bin[i]);
                }
            }

            String num = "";
            int cnt = 0;

            if (count == 2)
            {
                cnt = 4 - count;
            }
            else
            {
                cnt = count;
            }

            for (int i = cnt; i < 4; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (Server_list_bin[i][j] == Server_list_bin[4 + i][j] && Server_list_bin[i][j].ToString() == "1")
                    {
                        num = num + "1";
                    }
                    else
                    {
                        num = num + "0";
                    }
                }
                Server_result_bin.Add(num);
                num = "";
            }
            Console.WriteLine();


            //part of 255(server) TWO
            count = 0;
            num = "";
            for (int i = 4; i < Client_list.Count; i++)
            {
                if (Client_list[i] == "255")
                {
                    count++;
                }
            }
            if (count != 0)
            {
                for (int i = 0; i < count; i++)
                {
                    Server_result_bin.Add(Server_list_bin[i]);
                }
            }


            if (count == 2)
            {
                cnt = 4 - count;
            }
            else
            {
                cnt = count;
            }

            for (int i = cnt; i < 4; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (Server_list_bin[i][j] == Client_list_bin[4 + i][j] && Server_list_bin[i][j].ToString() == "1")
                    {
                        num = num + "1";
                    }
                    else
                    {
                        num = num + "0";
                    }
                }
                Server_result_bin.Add(num);
                num = "";
            }


            //part of 255(client) ONE
            count = 0;
            num = "";
            for (int i = 4; i < Client_list.Count; i++)
            {
                if (Client_list[i] == "255")
                {
                    count++;
                }
            }
            if (count != 0)
            {
                for (int i = 0; i < count; i++)
                {
                    Client_result_bin.Add(Client_list_bin[i]);
                }
            }


            if (count == 2)
            {
                cnt = 4 - count;
            }
            else
            {
                cnt = count;
            }

            for (int i = cnt; i < 4; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (Client_list_bin[i][j] == Client_list_bin[4 + i][j] && Client_list_bin[i][j].ToString() == "1")
                    {
                        num = num + "1";
                    }
                    else
                    {
                        num = num + "0";
                    }
                }
                Client_result_bin.Add(num);
                num = "";
            }


            //part of 255(client) TWO
            count = 0;
            num = "";
            for (int i = 4; i < Server_list.Count; i++)
            {
                if (Server_list[i] == "255")
                {
                    count++;
                }
            }
            if (count != 0)
            {
                for (int i = 0; i < count; i++)
                {
                    Client_result_bin.Add(Client_list_bin[i]);
                }
            }


            if (count == 2)
            {
                cnt = 4 - count;
            }
            else
            {
                cnt = count;
            }

            for (int i = cnt; i < 4; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (Client_list_bin[i][j] == Server_list_bin[4 + i][j] && Client_list_bin[i][j].ToString() == "1")
                    {
                        num = num + "1";
                    }
                    else
                    {
                        num = num + "0";
                    }
                }
                Client_result_bin.Add(num);
                num = "";
            }


            //binary to decimal
            List<String> Server_result_dec = new List<string>();
            List<String> Client_result_dec = new List<string>();

            int number_dec = 0;
            for (int i = 0; i < Server_result_bin.Count; i++)
            {
                number_dec = Decimal(Server_result_bin[i]);
                Server_result_dec.Add(Convert.ToString(number_dec));
            }

            number_dec = 0;
            for (int i = 0; i < Client_result_bin.Count; i++)
            {
                number_dec = Decimal(Client_result_bin[i]);
                Client_result_dec.Add(Convert.ToString(number_dec));
            }


            String text = "";
            for (int i = 0; i < 4; i++)
            {
                text = text + "." + Server_result_dec[i];
            }
            Console.WriteLine(text);
        }
    }
}
