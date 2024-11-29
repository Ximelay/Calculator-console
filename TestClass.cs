/*
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deligatu
{
    internal class TestClass
    {
        // Объявляем делегат, который описывает операцию
        delegate int Operation(int x, int y);

        // Методы для арифметических операций
        static int Add(int x, int y) => x + y;
        static int Subtract(int x, int y) => x - y;
        static int Multiply(int x, int y) => x * y;
        static int Divide(int x, int y) => y != 0 ? x / y : throw new DivideByZeroException();

        // Используем метод Add через делегат
        Operation addOp = Add;
        Console.WriteLine($"Add: 5 + 3 = {addOp(5, 3)}"); // 8

        // Используем метод Subtract через делегат
        Operation subtractOp = Subtract;
        Console.WriteLine($"Subtract: 5 - 3 = {subtractOp(5, 3)}"); // 2

        // Используем анонимный метод для деления
        Operation divideOp = delegate (int x, int y)
        {
            return y != 0 ? x / y : 0;
        };
        Console.WriteLine($"Divide: 6 / 3 = {divideOp(6, 3)}"); // 2

        // Используем лямбда-выражение для умножения
        Operation multiplyOp = (x, y) => x * y;
        Console.WriteLine($"Multiply: 5 * 3 = {multiplyOp(5, 3)}"); // 15
    }

}
*/
