namespace Api.Dtos
{
    public class PagoDto
    {
        public int Id { get; set; }
        public int IdCita { get; set; }
        public string MetodoPago { get; set; }
        public decimal Monto { get; set; }
        public DateTime FechaPago { get; set; }
        public string? ReferenciaPago { get; set; }
    }

    public class CreatePagoDto
    {
        public int IdCita { get; set; }
        public string MetodoPago { get; set; }
        public decimal Monto { get; set; }
        public string? ReferenciaPago { get; set; }
    }

    public class ReportePagosDto
    {
        public decimal TotalVentas { get; set; }
        public decimal Efectivo { get; set; }
        public decimal Transferencia { get; set; }
        public decimal NEQUI { get; set; }
        public decimal DaviPlata { get; set; }
        public decimal Tarjeta { get; set; }
        public decimal TotalPagos { get; set; }
    }
}
