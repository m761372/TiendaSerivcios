using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TiendaServicios.Api.CarritoCompra.Modelo;
using TiendaServicios.Api.CarritoCompra.Persistencia;
using TiendaServicios.Api.CarritoCompra.RemoteInterface;

namespace TiendaServicios.Api.CarritoCompra.Aplicacion
{
    public class Consulta
    {

        public class Ejecuta: IRequest<CarritoDto>
        {
            public int CarritoSesionId { get; set; }

            public Ejecuta(int id)
            {
                CarritoSesionId = id;
            }

            public class Manejador : IRequestHandler<Ejecuta, CarritoDto>
            {
                private readonly ContextoCarrito _contexto;
                private readonly ILibrosService _librosService;

                public Manejador(ContextoCarrito contexto, ILibrosService service)
                {
                    _contexto = contexto;
                    _librosService = service;
                }


                public async Task<CarritoDto> Handle(Ejecuta request, CancellationToken cancellationToken)
                {
                    var carrito = await _contexto.CarritoSesion.FirstOrDefaultAsync(x => x.CarritoSesionId == request.CarritoSesionId);
                    var carritoDetalle = await _contexto.CarritoSesionDetalle.Where(x => x.CarritoSesionId == request.CarritoSesionId).ToListAsync();

                   var carritoDetallesDto = new List<CarritoDetalleDto>();

                    foreach (var item in carritoDetalle)
                    {
                        var response = await _librosService.GetLibro(new Guid(item.ProductoSeleccionado.ToUpper()));
                        if (response.resultado)
                        {
                            var objLibreo = response.Libro;
                            var carritoDetalleDto = new CarritoDetalleDto
                            {
                                TituloLibro = objLibreo.Titulo,
                                FechaPublicacion = objLibreo.FechaPublicacion,
                                LibroId = objLibreo.LibreriaMaterialId
                            };

                            carritoDetallesDto.Add(carritoDetalleDto);
                        }
                    }

                    var carritoSesionDto = new CarritoDto
                    {
                        CarritoId = carrito.CarritoSesionId,
                        FechaPublicacion = carrito.FechaCreacion,
                        ListaProductos = carritoDetallesDto

                    };

                    return carritoSesionDto;
                }
            }
        }
    }
}
