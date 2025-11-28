using Api.Dtos;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    /// <summary>
    /// Controlador para la gestión de pagos.
    /// </summary>
    public class PagoController : BaseController
    {
        private readonly IPagoService _pagoService;
        private readonly IMapper _mapper;

        public PagoController(IPagoService pagoService, IMapper mapper)
        {
            _pagoService = pagoService;
            _mapper = mapper;
        }

        /// <summary>
        /// Registra un nuevo pago para una cita.
        /// </summary>
        /// <param name="createPagoDto">Datos del pago.</param>
        /// <returns>El pago registrado.</returns>
        /// <response code="201">Pago registrado exitosamente.</response>
        /// <response code="400">Si los datos son inválidos.</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<PagoDto>> RegistrarPago([FromBody] CreatePagoDto createPagoDto)
        {
            try
            {
                var pago = _mapper.Map<Pago>(createPagoDto);
                var nuevoPago = await _pagoService.RegistrarPagoAsync(pago);
                return CreatedAtAction(nameof(GetPagosByCita), new { citaId = nuevoPago.IdCita }, _mapper.Map<PagoDto>(nuevoPago));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Obtiene los pagos asociados a una cita.
        /// </summary>
        /// <param name="citaId">El ID de la cita.</param>
        /// <returns>Lista de pagos de la cita.</returns>
        /// <response code="200">Retorna la lista de pagos.</response>
        /// <response code="404">Si la cita no existe.</response>
        [HttpGet("cita/{citaId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<PagoDto>>> GetPagosByCita(int citaId)
        {
            try
            {
                var pagos = await _pagoService.GetPagosByCitaAsync(citaId);
                return Ok(_mapper.Map<IEnumerable<PagoDto>>(pagos));
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        /// <summary>
        /// Genera un reporte de pagos en un rango de fechas.
        /// </summary>
        /// <param name="inicio">Fecha de inicio del reporte.</param>
        /// <param name="fin">Fecha de fin del reporte.</param>
        /// <returns>Resumen de pagos por método y total.</returns>
        /// <response code="200">Retorna el reporte de pagos.</response>
        [HttpGet("reporte")]
        [Authorize(Roles = "admin")] // RF D4: Reportes para admin
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<ReportePagosDto>> GetReporte([FromQuery] DateTime inicio, [FromQuery] DateTime fin)
        {
            var reporteDict = await _pagoService.GetReportePagosByPeriodoAsync(inicio, fin);
            
            var reporteDto = new ReportePagosDto
            {
                TotalVentas = reporteDict.GetValueOrDefault("TotalVentas"),
                Efectivo = reporteDict.GetValueOrDefault("Efectivo"),
                Transferencia = reporteDict.GetValueOrDefault("Transferencia"),
                NEQUI = reporteDict.GetValueOrDefault("NEQUI"),
                DaviPlata = reporteDict.GetValueOrDefault("DaviPlata"),
                Tarjeta = reporteDict.GetValueOrDefault("Tarjeta"),
                TotalPagos = reporteDict.GetValueOrDefault("TotalPagos")
            };

            return Ok(reporteDto);
        }
    }
}
