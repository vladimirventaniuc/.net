using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biblioteca.API
{
    public class UiHelp
    {
        public string ConsoleRead()
        {
            string result = "";
            ConsoleShow("\nScrieti:");
            result = Console.ReadLine();
            while (true)
            {
                if (String.IsNullOrWhiteSpace(result))
                {
                    ConsoleShow("\nNu ati tastat corect! Reincercati.");
                    result = Console.ReadLine();
                }
                else
                {
                    break;
                }
            }
            return result;
        }
        public void Options()
        {
            Console.WriteLine("\n1.Lista de optiuni.");
            Console.WriteLine("2.Achizitie carte.");
            Console.WriteLine("3.Imprumutare carte.");
            Console.WriteLine("4.Restituire carte.");
            Console.WriteLine("5.Statistici.");
            Console.WriteLine("6.Adaugare cititor.");
            Console.WriteLine("7.Query direct.");
            Console.WriteLine("0.Iesire.");
        }
        public DateTime CheckIfIsDate(string input)
        {
            DateTime date;
            while (true)
            {
                if (DateTime.TryParse(input, out DateTime result))
                {
                    date = result;
                    break;
                }
                else
                {
                    ConsoleShow("Nu ati tastat o data valida!Rescrieti.");
                    input = ConsoleRead();
                }
            }
            return date;
        }
        public int CheckIfIsNumber(string input)
        {
            int number;
            while (true)
            {
                if (Int32.TryParse(input, out int result))
                {
                    number = result;
                    break;
                }
                else
                {
                    ConsoleShow("Nu ati tastat un numar valid!Rescrieti.");
                    input = ConsoleRead();
                }
            }
            return number;
        }

        public void ConsoleShow(string output)
        {
            Console.WriteLine("\n" + output);
        }

    }
}
