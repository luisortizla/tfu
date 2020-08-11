using System;
namespace TapFood.Entidades
{
    public class Pedidoadap
    {
        public string IdPedido { get; set; }
        public string NombrePlaza { get; set; }
        public string Ciudad { get; set; }
        public string IdUsuario { get; set; }
        public string NombreUsuario { get; set; }
        public string IdRestaurante { get; set; }
        public string IdProducto { get; set; }
        public string NombreProducto { get; set; }
        public float PrecioProducto { get; set; }
        public int Cantidad { get; set; }
        public byte[] Foto { get; set; }
        public string LatitudPlaza { get; set; }
        public string LongitudPlaza { get; set; }
    }
}
