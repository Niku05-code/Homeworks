// Folosindu-vă cunoştinţele de bază acumulate până acum, implementaţi, organizând codul conform
// principiilor Programării Orientate înspre Obiecte (clase), un calculator cu interfaţă de tip consolă,
// care să ofere toate operaţiile puse la dispoziţie de calculatorul standard din Windows, inclusiv funcţia
// de memorare a rezultatului.

using System;
using System.Collections.Generic;

namespace ConsoleCalculator
{
    interface IOperation
    {
        double Execute(double a, double b = 0);
    }

    class Addition : IOperation { public double Execute(double a, double b) => a + b; }
    class Subtraction : IOperation { public double Execute(double a, double b) => a - b; }
    class Multiplication : IOperation { public double Execute(double a, double b) => a * b; }
    class Division : IOperation
    {
        public double Execute(double a, double b)
        {
            if (b == 0) throw new DivideByZeroException("Împărțirea la zero nu este permisă.");
            return a / b;
        }
    }
    class Modulo : IOperation { public double Execute(double a, double b) => a % b; }

    class Power : IOperation { public double Execute(double a, double b) => Math.Pow(a, b); }
    class SquareRoot : IOperation
    {
        public double Execute(double a, double b = 0)
        {
            if (a < 0) throw new ArgumentException("Nu se poate calcula radicalul unui număr negativ.");
            return Math.Sqrt(a);
        }
    }
    class Logarithm : IOperation
    {
        public double Execute(double a, double b = 0)
        {
            if (a <= 0) throw new ArgumentException("Logaritmul este definit doar pentru numere pozitive.");
            return Math.Log10(a);
        }
    }
    class Sine : IOperation { public double Execute(double a, double b = 0) => Math.Sin(a); }
    class Cosine : IOperation { public double Execute(double a, double b = 0) => Math.Cos(a); }
    class Tangent : IOperation { public double Execute(double a, double b = 0) => Math.Tan(a); }

    class Calculator
    {
        private Dictionary<string, IOperation> operations;
        private double currentValue = 0;
        private string pendingOperation = null;
        private bool isNewInput = true;

        public Calculator()
        {
            operations = new Dictionary<string, IOperation>
            {
                { "+", new Addition() },
                { "-", new Subtraction() },
                { "*", new Multiplication() },
                { "/", new Division() },
                { "%", new Modulo() },
                { "pow", new Power() },
                { "sqrt", new SquareRoot() },
                { "log", new Logarithm() },
                { "sin", new Sine() },
                { "cos", new Cosine() },
                { "tan", new Tangent() },
            };
        }

        public void ListOperations()
        {
            Console.WriteLine("Operații disponibile: " + string.Join(", ", operations.Keys));
        }

        public void Input(string input)
        {
            if (input == "exit")
            {
                Environment.Exit(0);
            }

            if (operations.ContainsKey(input))
            {
                pendingOperation = input;
                Console.WriteLine($"Operația setată: {pendingOperation}");
            }
            else if (double.TryParse(input, out double number))
            {
                if (pendingOperation == null || isNewInput)
                {
                    currentValue = number;
                    Console.WriteLine($"Valoare setată: {currentValue}");
                }
                else
                {
                    var op = operations[pendingOperation];
                    currentValue = op.Execute(currentValue, number);
                    Console.WriteLine($"Rezultat: {currentValue}");
                    pendingOperation = null;
                }

                isNewInput = false;
            }
            else
            {
                Console.WriteLine("Input invalid.");
            }
        }

        public void Reset()
        {
            currentValue = 0;
            pendingOperation = null;
            isNewInput = true;
        }
    }

    class UI
    {
        private Calculator calculator = new Calculator();

        public void Run()
        {
            Console.WriteLine("=== Calculator Inteligent (C# OOP) ===");
            calculator.ListOperations();
            Console.WriteLine("Scrie 'exit' pentru a ieși.");

            while (true)
            {
                Console.Write("\n> ");
                string input = Console.ReadLine().Trim().ToLower();
                try
                {
                    calculator.Input(input);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Eroare: {ex.Message}");
                }
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            UI ui = new UI();
            ui.Run();
        }
    }
}
