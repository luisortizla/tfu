using System;
namespace TapFood
{
    public class Restaurante
    {
        public string  IdRestaurante { get; set; }

        public string NombreRestaurante { get; set; }

        public string Ciudad { get; set; }

        public string DirecccionRestaurante { get; set; }

        public int IdPlaza { get; set; }

        public string NombrePlaza { get; set; }

        public string ResponsableRestaurante { get; set; }

        public string EmailRestaurante { get; set; }

        public string ContraseñaRestaurante { get; set; }

        public float TelefonoRestaurante { get; set; }

        public float CuentaDepositoRestaurante { get; set; }

        public string Banco { get; set; }

        public DateTime HoraApertura { get; set; }

        public DateTime HoraCierre { get; set; }

        public byte[] LogoRestaurante { get; set; }

    }
}
