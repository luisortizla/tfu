using System;
namespace TapFood
{
    public class Pedido
    {
        public int IdPedido { get; set; }

        public string IdUsuario { get; set; }

        public string NombreUsuario { get; set; }

        public string IdProducto { get; set; }

        public string NombreProducto { get; set; }

        public float PrecioProducto { get; set; }

        public float CostoServicio { get; set; }

        public string IdRepartidor { get; set; }

        public string NombreRepartidor { get; set; }

        public string TipoDePago { get; set; }

        public double LatitudPlaza { get; set; }

        public double LongitudPlaza { get; set; }

        public double LatitudUsuario { get; set; }

        public double LongitudUsuario { get; set; }

        public DateTime Creada { get; set; }

        public DateTime Entregada { get; set; }

    }
}
