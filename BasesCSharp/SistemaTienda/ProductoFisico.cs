namespace Tienda
{
    public class ProductoFisico : Producto
    {
        public double PesoKg {get; set;}

        public ProductoFisico(string nombre, double precio, int stock, double pesoKg)
        :base(nombre, precio, stock)
        {
            PesoKg = pesoKg;
        }

        public override double PrecioFinal()
        {
            double resultado = Precio + (PesoKg * 2);
            return resultado;
        }

        public override void MostrarInfo()
        {
            base.MostrarInfo();
            Console.WriteLine($"Peso: {PesoKg}kg (Incluido en el precio)");
        }

        
    }
}