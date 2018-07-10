using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Pizzas
{
    class Program
    {
        class Result
        {
            public int Rank { get; set; }
            public int Orders { get; set; }
            public List<string> Combinition { get; set; }
        }

        class Pizza
        {
            public List<string> toppings { get; set; }

            public List<List<string>> GetAllCombinitions(List<Pizza> Pizzas)
            {
                List<List<string>> Combinitions = new List<List<string>>();


                foreach (Pizza pizza in Pizzas)
                {
                    pizza.toppings.Sort();

                    if (!this.CombinationExists(Combinitions, pizza.toppings))
                    {
                        Combinitions.Add(pizza.toppings);
                    }
                }

                return Combinitions;
            }

            public bool CombinationExists(List<List<string>> combinations, List<string> combination)
            {
                foreach (List<string> item in combinations)
                {
                    if (item.SequenceEqual(combination))
                    {
                        return true;
                    }
                }

                return false;
            }

            public int GetCombinationsOrders(List<Pizza> Pizzas, List<string> toppings)
            {
                int count = 0;
                foreach (Pizza pizza in Pizzas)
                {
                    if (pizza.toppings.All(toppings.Contains) && pizza.toppings.Count == toppings.Count)
                    {
                        count++;
                    }
                }

                return count;
            }
        }




        static void Main(string[] args)
        {
            Pizza pizza = new Pizza();
            List<Result> result = new List<Result>();
            using (StreamReader r = new StreamReader("pizzas.json"))
            {
                string json = r.ReadToEnd();
                List<Pizza> Pizzas = JsonConvert.DeserializeObject<List<Pizza>>(json);

                List<List<string>> Combinations = pizza.GetAllCombinitions(Pizzas);

                foreach (List<string> Combinition in Combinations)
                {
                    Result rsult = new Result();
                    rsult.Orders = pizza.GetCombinationsOrders(Pizzas, Combinition);
                    rsult.Combinition = Combinition;
                    result.Add(rsult);
                }

                result = result.Where(i => i.Combinition.Count > 1).OrderByDescending(a => a.Orders).Take(20).ToList();

                int count = 0;
                foreach (Result item in result)
                {
                    count++;
                    Console.WriteLine("#" + count);
                    Console.WriteLine(string.Join(",", item.Combinition.Select(a => a)));
                    Console.WriteLine("Orders Count : " + item.Orders);
                }

            }

            Console.ReadLine();

        }
    }
}
