namespace MiPrimerProyecto.Utilidades;
using System;


public class Leccion1Interactiva
{
    public void Calculadora(){
        Console.WriteLine("=== Calculadora Simple ===");

        //Variables
        double resultado = 0;
        bool operacionValida = true;

        // Pedir entrada al usuario
        Console.Write("Ingresa el primer número: ");
        double num1 = double.Parse(Console.ReadLine()!);

        Console.Write("Ingresa el segundo número: ");
        double num2 = double.Parse(Console.ReadLine()!);

        Console.WriteLine("\nOperaciones disponibles:");
        Console.WriteLine("+ : Suma");
        Console.WriteLine("- : Resta");
        Console.WriteLine("* : Multiplicación");
        Console.WriteLine("/ : División");
        Console.WriteLine("^ : Potencia");
        Console.WriteLine("% : Módulo");
        Console.Write("\nElige una operación: ");
        Console.Write("Ingresa el signo de operación: ");
        string signo = Console.ReadLine()!;
            
        // Calcular
        switch (signo){
            case "+" :
                resultado = num1 + num2;
                break;

            case "-":
                resultado = num1 - num2;
                break;

            case "*":
                resultado = num1 * num2;
                break;

            case "/":
                if(num2 == 0){
                    Console.WriteLine("Error: No se puede dividir entre 0");
                    operacionValida = false;
                }else{
                    resultado = num1 / num2;
                
                }
                break;

            case "^":
                resultado = Math.Pow(num1, num2);
                break;

            case "%":
                resultado = num1 % num2;
                break;

            default:
                Console.WriteLine("Operación no válida");
                operacionValida = false;
                break;
        }

        if (operacionValida)
        {
            Console.WriteLine($"\nEl resultado de {num1} {signo} {num2} es: {resultado}");
        }
    }
}
