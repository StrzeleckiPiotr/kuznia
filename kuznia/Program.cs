
using System;
using System.Threading;

namespace kuznia
{
    class Program
    { 
        
        
        static void Main(string[] args)
        {
            while (true)
            {
                System.Console.WriteLine("Type number and press Return");
                try
                {
                    int i = int.Parse(System.Console.ReadLine());
                    switch (i)
                    {
                        case 0:
                        case 1:
                            {
                                MlNetTrial mlNetTrial = new MlNetTrial();
                                Console.Write("Select Model,EN ->1 , Pl-2");
                                Console.WriteLine();
                                int c = int.Parse(Console.ReadLine());
                                mlNetTrial.Execute(c);
                                break;
                            }
                        case 2:
                            {
                                AzurecognityService azurecognityService = new AzurecognityService();
                                break;
                            }
                        case 3:
                        case 4:
                        case 5:
                            {
                                System.Console.WriteLine("Medium number");
                                break;
                            }
                        default:
                            {
                                System.Console.WriteLine("Other number");
                                break;
                            }
                    }
                }
                catch
                {

                }
               
            }

            
        }
    }
}
            
