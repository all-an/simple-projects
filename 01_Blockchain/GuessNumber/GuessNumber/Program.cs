using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GuessNumber
{
    class Program
    {
        static void Main(string[] args)
        {
            //https://gist.github.com/MrAwesomeness/6196354b6643faaad801

            Random rand = new Random();
            int guess = 0;
            string welcome = "Guess a number between 1 and 100";

            int num = rand.Next(1, 100);

            //Console.WriteLine(num);

            Console.WriteLine(welcome);

            int i = 0;

            while(guess != num)
            {
                try // exception handling
                {
                    guess = Convert.ToInt32(Console.ReadLine());

                    if (guess > num)
                    {
                        Console.WriteLine("Too High");
                    }
                    else
                    {
                        Console.WriteLine("Too Low");
                    }
                }
                catch //exception handling in case of an error
                {
                    Console.WriteLine("Guess must be a number");
                    i--;
                }
                i++;
            }
            Console.WriteLine("Congrats, it took you " + i + " tries.");

        }
    }
}













