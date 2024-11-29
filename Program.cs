using System;
using System.Collections.Generic;
using System.Text;

namespace Deligatu
{
    class Program
    {
        delegate double Operation(double x, double y);

        static void Main(string[] args)
        {
            var operations = new Dictionary<string, Operation>()
            {
                {"+", (x, y) => x + y},
                {"-", (x, y) => x - y},
                {"*", (x, y) => x * y},
                {"/", (x, y) => y != 0 ? x / y : throw new DivideByZeroException()}
            };

            // Строка в которой будет отображаться ввод
            StringBuilder currentInput = new StringBuilder();
            double result = 0;
            bool isResultDisplayed = false;
            bool isError = false;
            string errorMessage = string.Empty;

            while (true)
            {
                Console.Clear();
                Console.WriteLine(" ======Калькулятор=================");
                Console.WriteLine($"|[               {currentInput} ]|");
                Console.WriteLine(" |================================|");
                Console.WriteLine(" |[  1  ] [  2  ] [  3  ] [  +  ]| ");
                Console.WriteLine(" |[  4  ] [  5  ] [  6  ] [  -  ]| ");
                Console.WriteLine(" |[  7  ] [  8  ] [  9  ] [  *  ]| ");
                Console.WriteLine(" |[  0  ] [  .  ] [  =  ] [  /  ]| ");
                Console.WriteLine(" ==================================");

                // Если есть ошибка, то выводим её
                if (isError)
                {
                    Console.WriteLine($"Ошибка: {errorMessage}");
                    Console.WriteLine("Нажмите любую клавишу для продолжения...");
                }
                else
                {
                    Console.WriteLine("Для ввода чисел используйте клавиши (0-9)");
                    Console.WriteLine("Для ввода операторов используйте клавиши ('+', '-', '*', '/')");
                    Console.WriteLine("Нажмите 'Enter' для вычисления результата.");
                    Console.WriteLine("Для выхода нажмите на клавишу 'Escape'.");
                }

                // Обработка нажатия клавиш
                ConsoleKeyInfo keyInfo = Console.ReadKey(true);

                if (keyInfo.Key == ConsoleKey.Escape)
                {
                    break;
                }

                if (keyInfo.Key == ConsoleKey.Enter)
                {
                    if (currentInput.Length > 0 && !isResultDisplayed)
                    {
                        try
                        {
                            string expression = currentInput.ToString();
                            result = EvaluateExpression(expression, operations);
                            isResultDisplayed = true;
                            currentInput.Clear();
                            currentInput.Append(result);
                            isError = false;
                        }
                        catch (Exception ex)
                        {
                            errorMessage = ex.Message;
                            isError = true;
                        }
                    }
                }
                else if (keyInfo.Key == ConsoleKey.Backspace)
                {
                    // Обработка Backspace для удаления символов
                    if (currentInput.Length > 0)
                    {
                        currentInput.Remove(currentInput.Length - 1, 1); // Удаляем последний символ
                    }
                }
                else if (keyInfo.Key == ConsoleKey.D0 || keyInfo.Key == ConsoleKey.D1 || keyInfo.Key == ConsoleKey.D2 ||
                         keyInfo.Key == ConsoleKey.D3 || keyInfo.Key == ConsoleKey.D4 || keyInfo.Key == ConsoleKey.D5 ||
                         keyInfo.Key == ConsoleKey.D6 || keyInfo.Key == ConsoleKey.D7 || keyInfo.Key == ConsoleKey.D8 ||
                         keyInfo.Key == ConsoleKey.D9)
                {
                    // Добавляем цифры
                    if (isResultDisplayed)
                    {
                        currentInput.Clear();
                        isResultDisplayed = false;
                    }

                    currentInput.Append(keyInfo.KeyChar);
                }
                else if (keyInfo.Key == ConsoleKey.OemPlus || keyInfo.Key == ConsoleKey.OemMinus ||
                         keyInfo.Key == ConsoleKey.Multiply || keyInfo.Key == ConsoleKey.Divide)
                {
                    // Добавляем операторы
                    if (isResultDisplayed)
                    {
                        currentInput.Clear();
                        isResultDisplayed = false;
                    }

                    currentInput.Append(keyInfo.KeyChar);
                }
                else if (keyInfo.Key == ConsoleKey.OemPeriod)
                {
                    // чтобы не вводить больше одной точки в числе
                    if (currentInput.Length == 0 ||
                        "/*+-".Contains(currentInput[currentInput.Length - 1].ToString())
                        || currentInput.ToString().Contains("."))
                    {
                        // если точка есть, то не добавляем её
                        return;
                    }
                    if (isResultDisplayed)
                    {
                        currentInput.Clear();
                        isResultDisplayed = false;
                    }

                    currentInput.Append(keyInfo.KeyChar);
                }

                if (isError)
                {
                    Console.ReadKey(true);
                    isError = false;
                }
            }
        }

        // Вычисление выражения с несколькими операциями
        static double EvaluateExpression(string expression, Dictionary<string, Operation> operations)
        {
            try
            {
                var tokens = TokenizeExpression(expression);

                // Применение операций
                double result = double.Parse(tokens[0]);
                for (int i = 1; i < tokens.Count; i += 2)
                {
                    string op = tokens[i];
                    double operand = double.Parse(tokens[i + 1]);

                    if (operations.ContainsKey(op))
                    {
                        result = operations[op](result, operand);
                    }
                    else
                    {
                        throw new InvalidOperationException($"Оператор {op} не поддерживается");
                    }
                }

                return result;
            }
            catch (Exception ex)
            {
                throw new Exception($"Невозможно вычислить выражение. {ex.Message}");
            }
        }

        // Метод для токенизации введенного выражения
        static List<string> TokenizeExpression(string expression)
        {
            List<string> tokens = new List<string>();
            StringBuilder currentNumber = new StringBuilder();

            foreach (char c in expression)
            {
                if (char.IsDigit(c) || c == '.')
                {
                    currentNumber.Append(c);
                }
                else if ("+-*/".Contains(c))
                {
                    if (currentNumber.Length > 0)
                    {
                        tokens.Add(currentNumber.ToString());
                        currentNumber.Clear();
                    }
                    tokens.Add(c.ToString());
                }
                else
                {
                    throw new InvalidOperationException($"Некорректный символ: {c}");
                }
            }
            if (currentNumber.Length > 0)
            {
                tokens.Add(currentNumber.ToString());
            }

            return tokens;
        }
    }
}
