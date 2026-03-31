namespace MiPrimerProyecto;
using System;
using MiPrimerProyecto.Utilidades;
class Leccion1
{
    static void Main()
    {
        //VARIABLES Y TIPOS DE DATOS
        int edad = 24;
        string nombre = "Juan";
        double salario = 2500.50;
        bool enDesarrollador = true;

        Console.WriteLine("=== Mi Información ===");
        Console.WriteLine($"Nombre: {nombre}");
        Console.WriteLine($"Edad: {edad}");
        Console.WriteLine($"Salario: {salario}");
        Console.WriteLine($"Es Desarrollador?: {enDesarrollador}");
        
        // OPERADORES ARITMÉTICOS
        int numero1 = 10;
        int numero2 = 3;
        
        Console.WriteLine("\n=== Operaciones ===");
        Console.WriteLine($"{numero1} + {numero2} = {numero1 + numero2}");
        Console.WriteLine($"{numero1} - {numero2} = {numero1 - numero2}");
        Console.WriteLine($"{numero1} * {numero2} = {numero1 * numero2}");
        Console.WriteLine($"{numero1} / {numero2} = {numero1 / numero2}");
        Console.WriteLine($"{numero1} % {numero2} = {numero1 % numero2}");
        
        // OPERADORES DE COMPARACIÓN
        Console.WriteLine("\n=== Comparaciones ===");
        Console.WriteLine($"¿{numero1} > {numero2}? {numero1 > numero2}");
        Console.WriteLine($"¿{numero1} == {numero2}? {numero1 == numero2}");
        Console.WriteLine($"¿{numero1} != {numero2}? {numero1 != numero2}");

        Leccion1Interactiva miCal = new();
        Leccion2 controlEdad = new();
        Leccion2 validarPass = new();
        
        // miCal.Calculadora();
        //controlEdad.Edad();
        validarPass.ValidacionPassword();
    }

    
}
