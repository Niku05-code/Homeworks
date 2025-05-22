// Scrieti o aplicatie consola in C# care sa ofere posibilitatea efectuarii
// operatiilor de baza cu matrice: adunare, inmultire si inmultire a unei
// matrice cu un scalar.
// Aplicatia consola va avea un meniu alcatuit din patru optiuni
// numerotate de la 1 la 4. Daca se selecteaza o optiune de la 1 la 3 se
// va efectua operatia respectiva, daca se selecteaza optiunea 4 se va
// termina aplicatia.

using System;

namespace MatrixOperations
{
    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                Console.WriteLine("Matrix Operations Menu:");
                Console.WriteLine("1. Add two matrices");
                Console.WriteLine("2. Multiply two matrices");
                Console.WriteLine("3. Multiply matrix by scalar");
                Console.WriteLine("4. Exit");
                Console.Write("Select an option (1-4): ");

                if (!int.TryParse(Console.ReadLine(), out int option) || option < 1 || option > 4)
                {
                    Console.WriteLine("Invalid option. Please try again.");
                    continue;
                }

                if (option == 4)
                {
                    Console.WriteLine("Exiting the application...");
                    break;
                }

                try
                {
                    switch (option)
                    {
                        case 1:
                            PerformMatrixAddition();
                            break;
                        case 2:
                            PerformMatrixMultiplication();
                            break;
                        case 3:
                            PerformScalarMultiplication();
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred: {ex.Message}");
                }

                Console.WriteLine("\nPress any key to continue...");
                Console.ReadKey();
                Console.Clear();
            }
        }

        static void PerformMatrixAddition()
        {
            Console.WriteLine("\nMatrix Addition");
            
            Console.WriteLine("Enter first matrix:");
            var matrix1 = ReadMatrix();
            
            Console.WriteLine("Enter second matrix:");
            var matrix2 = ReadMatrix();

            if (matrix1.GetLength(0) != matrix2.GetLength(0) || matrix1.GetLength(1) != matrix2.GetLength(1))
            {
                throw new InvalidOperationException("Matrices must have the same dimensions for addition.");
            }

            var result = AddMatrices(matrix1, matrix2);
            
            Console.WriteLine("\nResult of addition:");
            PrintMatrix(result);
        }

        static void PerformMatrixMultiplication()
        {
            Console.WriteLine("\nMatrix Multiplication");
            
            Console.WriteLine("Enter first matrix:");
            var matrix1 = ReadMatrix();
            
            Console.WriteLine("Enter second matrix:");
            var matrix2 = ReadMatrix();

            if (matrix1.GetLength(1) != matrix2.GetLength(0))
            {
                throw new InvalidOperationException("Number of columns in first matrix must equal number of rows in second matrix.");
            }

            var result = MultiplyMatrices(matrix1, matrix2);
            
            Console.WriteLine("\nResult of multiplication:");
            PrintMatrix(result);
        }

        static void PerformScalarMultiplication()
        {
            Console.WriteLine("\nScalar Multiplication");
            
            Console.WriteLine("Enter the matrix:");
            var matrix = ReadMatrix();
            
            Console.Write("Enter the scalar value: ");
            if (!double.TryParse(Console.ReadLine(), out double scalar))
            {
                throw new ArgumentException("Invalid scalar value.");
            }

            var result = MultiplyMatrixByScalar(matrix, scalar);
            
            Console.WriteLine("\nResult of scalar multiplication:");
            PrintMatrix(result);
        }

        static double[,] ReadMatrix()
        {
            Console.Write("Enter number of rows: ");
            int rows = int.Parse(Console.ReadLine());
            Console.Write("Enter number of columns: ");
            int cols = int.Parse(Console.ReadLine());

            var matrix = new double[rows, cols];

            Console.WriteLine($"Enter {rows}x{cols} matrix values (row by row):");
            for (int i = 0; i < rows; i++)
            {
                var rowValues = Console.ReadLine().Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
                if (rowValues.Length != cols)
                {
                    throw new ArgumentException($"Expected {cols} values for row {i + 1}.");
                }

                for (int j = 0; j < cols; j++)
                {
                    matrix[i, j] = double.Parse(rowValues[j]);
                }
            }

            return matrix;
        }

        static double[,] AddMatrices(double[,] matrix1, double[,] matrix2)
        {
            int rows = matrix1.GetLength(0);
            int cols = matrix1.GetLength(1);
            var result = new double[rows, cols];

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    result[i, j] = matrix1[i, j] + matrix2[i, j];
                }
            }

            return result;
        }

        static double[,] MultiplyMatrices(double[,] matrix1, double[,] matrix2)
        {
            int rows1 = matrix1.GetLength(0);
            int cols1 = matrix1.GetLength(1);
            int cols2 = matrix2.GetLength(1);
            var result = new double[rows1, cols2];

            for (int i = 0; i < rows1; i++)
            {
                for (int j = 0; j < cols2; j++)
                {
                    for (int k = 0; k < cols1; k++)
                    {
                        result[i, j] += matrix1[i, k] * matrix2[k, j];
                    }
                }
            }

            return result;
        }

        static double[,] MultiplyMatrixByScalar(double[,] matrix, double scalar)
        {
            int rows = matrix.GetLength(0);
            int cols = matrix.GetLength(1);
            var result = new double[rows, cols];

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    result[i, j] = matrix[i, j] * scalar;
                }
            }

            return result;
        }

        static void PrintMatrix(double[,] matrix)
        {
            int rows = matrix.GetLength(0);
            int cols = matrix.GetLength(1);

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    Console.Write(matrix[i, j].ToString("F2").PadLeft(8));
                }
                Console.WriteLine();
            }
        }
    }
}