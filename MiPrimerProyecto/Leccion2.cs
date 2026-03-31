namespace MiPrimerProyecto.Utilidades;
using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Versioning;

public class Leccion2
{
    public void Edad(){
        Console.WriteLine("Cual es tu edad ?");
        int edad = int.Parse(Console.ReadLine()!);
        string categoria;

        // Usar if-else if-else para rangos
        if (edad < 13)
        {
            categoria = "Niño";
        }
        else if (edad < 18)
        {
            categoria = "Adolescente";
        }
        else if (edad < 65)
        {
            categoria = "Adulto";
        }
        else
        {
            categoria = "Adulto Mayor";
        }
        
        Console.WriteLine($"Tu categoría es: {categoria}");

        // Bonus: Información adicional con más lógica
        Console.WriteLine($"¿Puede votar?: {(edad >= 18 ? "Sí" : "No")}");
        Console.WriteLine($"¿Puede jubilarse?: {(edad >= 65 ? "Sí" : "No")}");
    }

    public void ValidacionPassword(){
        Console.Write("Ingresa una contraseña: ");
        string contrasena = Console.ReadLine()!;

        //Validar Caracteristicas
        bool tieneLongitud = contrasena.Length >= 8;
        bool tieneNumero = false;
        bool tieneMayuscula = false;

        //Revisar cada caracter
        foreach (char c in contrasena){
            if(char.IsDigit(c)){
                tieneNumero = true;
            }

            if(char.IsUpper(c)){
                tieneMayuscula = true;
            }

        // Mostrar resultado
        Console.WriteLine("\n=== Validación ===");
        Console.WriteLine($"Longitud >= 8: {tieneLongitud}");
        Console.WriteLine($"Tiene números: {tieneNumero}");
        Console.WriteLine($"Tiene mayúsculas: {tieneMayuscula}");
        }


        if(tieneLongitud && tieneMayuscula && tieneNumero){
            Console.WriteLine("Contraseña válida");
        }else{
            Console.WriteLine("Contraseña es débil");
        }
    }
}