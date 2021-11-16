/***********************************************************************************************************
    Copyright (C) 2021 ITSC - Ing. de Software

    Este programa es software libre: usted puede redistribuirlo y/o modificarlo 
    bajo los términos de la Licencia Pública General GNU publicada 
    por la Fundación para el Software Libre, ya sea la versión 3 
    de la Licencia, o (a su elección) cualquier versión posterior.

    Este programa se distribuye con la esperanza de que sea útil, pero 
    SIN GARANTÍA ALGUNA; ni siquiera la garantía implícita 
    MERCANTIL o de APTITUD PARA UN PROPÓSITO DETERMINADO. 
    Consulte los detalles de la Licencia Pública General GNU para obtener 
    una información más detallada. 

    Debería haber recibido una copia de la Licencia Pública General GNU 
    junto a este programa. 
    En caso contrario, consulte http://www.gnu.org/licenses/gpl-3.0.html
 **********************************************************************************************************/

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ejemplo.Models.dto.response;
using Microsoft.AspNetCore.Http;
using ejemplo.Models.comun;
using ejemplo.dao;
using ejemplo.dao.comun;
using System.Collections.Generic;
using ejemplo.entities.producto;
using System;

namespace ejemplo.Controllers
{
    public class ProductoController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public ProductoController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            UsuarioResponse usuarioResponse = 
                HttpContext.Session.Get<UsuarioResponse>("UsuarioLogueado");

            if(usuarioResponse == null)
            {
                return RedirectToAction("Index", "Home");
            }
            
            return View();
        }

        public JsonResult ListarProductosPaginado(ModeloConsultaGrilla modeloConsulta)
        {
            UsuarioResponse usuarioResponse = 
                HttpContext.Session.Get<UsuarioResponse>("UsuarioLogueado");

            if(usuarioResponse == null)
            {
                return Json(JsonReturn.RedireccionarIndex());
            }
            
            try
            {
                long cantidadTotal = 0;
                List<ProductoResponse> productosResponse = new List<ProductoResponse>();

                using(DAOFactory df = new DAOFactory())
                {
                    Ordenamiento ordenamiento = obtenerOrdenamientoProductos(modeloConsulta);
                    List<Asociacion> asociaciones = obtenerAsociacionesProductos();
                    
                    List<AtributoBusqueda> atributosBusqueda = 
                        obtenerAtributosBusquedaProducto();

                    Paginado paginado = new Paginado
                    {
                        Comienzo = modeloConsulta.start,
                        Cantidad = modeloConsulta.length
                    };

                    IList<Producto> productos = df.DAOProducto.ObtenerProductos(
                        atributosBusqueda,
                        modeloConsulta.search.value,
                        paginado,
                        ordenamiento,
                        asociaciones,
                        out cantidadTotal);

                    foreach(Producto producto in productos)
                    {
                        productosResponse.Add(new ProductoResponse
                        {
                            Id = producto.Id,
                            Cantidad = producto.Cantidad.ToString("#,##0.00"),
                            DescripcionProducto = producto.Descripcion,

                            DescripcionUnidadMedida = 
                                producto.ObtenerDescripcionUnidadMedida(),

                            PorcentajeGanancia = "% " + 
                                producto.PorcentajeGanancia.ToString("#,##0.00"),

                            PrecioCompra = 
                                producto.ObtenerPrecioCompra(
                                    producto.Cantidad, 
                                    producto.UnidadMedida).ToString("#,##0.00"),

                            PrecioVenta = 
                                producto.ObtenerPrecioVenta(
                                    producto.Cantidad, 
                                    producto.UnidadMedida).ToString("#,##0.00"),

                            Tipo = producto.ObtenerTipo()
                        });
                    }

                    return Json(JsonReturn.SuccessConRetorno(new ConsultaGrillaResponse
                    {
                        draw = modeloConsulta.draw,
                        recordsFiltered = productosResponse.Count,
                        recordsTotal = cantidadTotal,
                        data = productosResponse
                    }));
                }
            }
            catch(System.Exception ex)
            {
                return Json(JsonReturn.ErrorConMsjSimple(
                    "Se generó un error mientras intentabamos listar los productos<br><br>" +
                    "Más info:<br>" + ex.Message));
            }   
        }

        private static List<AtributoBusqueda> obtenerAtributosBusquedaProducto()
        {
            List<AtributoBusqueda> atributosBusqueda = new List<AtributoBusqueda>();

            atributosBusqueda.Add(new AtributoBusqueda
            {
                NombreAtributo = "Producto.Descripcion",
                TipoDato = TipoDato.String
            });

            atributosBusqueda.Add(new AtributoBusqueda
            {
                NombreAtributo = "UnidadMedida.Descripcion",
                TipoDato = TipoDato.String
            });

            atributosBusqueda.Add(new AtributoBusqueda
            {
                NombreAtributo = "Producto.Cantidad",
                TipoDato = TipoDato.Double
            });

            atributosBusqueda.Add(new AtributoBusqueda
            {
                NombreAtributo = "Producto.PorcentajeGanancia",
                TipoDato = TipoDato.Double
            });

            return atributosBusqueda;
        }

        private static List<Asociacion> obtenerAsociacionesProductos()
        {
            List<Asociacion> asociaciones = new List<Asociacion>();

            asociaciones.Add(new Asociacion
            {
                RutaDeAsociacion = "Producto.UnidadMedida",
                Alias = "UnidadMedida",
                TipoJoin = TipoJoin.InnerJoin
            });

            return asociaciones;
        }

        private static Ordenamiento obtenerOrdenamientoProductos(
            ModeloConsultaGrilla modeloConsulta)
        {
            Ordenamiento ordenamiento = new Ordenamiento
            {
                Atributo = "Producto.Descripcion",
                Direccion = "asc"
            };

            if (modeloConsulta.order != null &&
                modeloConsulta.order.Count > 0)
            {
                int columnIndex = modeloConsulta.order[0].column;
                string col = modeloConsulta.columns[columnIndex].data;

                if (col == "descripcionProducto") col = "Producto.Descripcion";
                else if (col == "descripcionUnidadMedida") col = "UnidadMedida.Descripcion";
                else if (col == "cantidad") col = "Producto.Cantidad";
                else if (col == "porcentajeGanancia") col = "Producto.PorcentajeGanancia";
                else col = "Producto.Descripcion";

                ordenamiento.Atributo = col;
                ordenamiento.Direccion =
                    modeloConsulta.order[0].dir == ModeloDireccion.desc ? "desc" : "asc";
            }

            return ordenamiento;
        }
    
        public IActionResult NuevaMateriaPrima()
        {
            UsuarioResponse usuarioResponse = 
                HttpContext.Session.Get<UsuarioResponse>("UsuarioLogueado");

            if(usuarioResponse == null)
            {
                return RedirectToAction("Index", "Home");
            }
            
            ejemplo.Models.vista.ModeloVistaMateriaPrima modeloVistaMateriaPrima =
                new Models.vista.ModeloVistaMateriaPrima {
                    Cantidad = "1",
                    Descripcion = "",
                    Id = 0,
                    IdUnidadMedida = 0,
                    PorcentajeGanancia = "",
                    Precio = "",
                    UnidadMedida = ""
                };

            return View(
                "~/Views/Producto/MateriaPrima.cshtml", 
                modeloVistaMateriaPrima);
        }        
    
        [HttpPost]
        public JsonResult GuardarMateriaPrima(
            long id,
            string descripcion,
            double precio,
            long idUnidadMedida,
            double cantidad,
            double porcentajeGanancia
        )
        {
            UsuarioResponse usuarioResponse = 
                HttpContext.Session.Get<UsuarioResponse>("UsuarioLogueado");

            if(usuarioResponse == null)
            {
                return Json(JsonReturn.RedireccionarIndex());
            }

            try
            {
                if(string.IsNullOrEmpty(descripcion))
                {
                    return Json(Models.comun.JsonReturn.ErrorConMsjSimple("Debe ingresar una descripción"));
                }

                using(DAOFactory df = new DAOFactory())
                {
                    entities.producto.MateriaPrima materiaPrima = df.DAOMateriaPrima.ObtenerMateriaPrima(id);

                    if(materiaPrima == null)
                    {
                        materiaPrima = new MateriaPrima
                        {
                            Cantidad = 0,
                            Descripcion = "",
                            Id = 0,
                            PorcentajeGanancia = 0,
                            Precio = 0,
                            UnidadMedida = null
                        };
                    }

                    materiaPrima.Cantidad = cantidad;
                    materiaPrima.Descripcion = descripcion;
                    materiaPrima.PorcentajeGanancia = porcentajeGanancia;
                    materiaPrima.Precio = precio;
                    materiaPrima.UnidadMedida = df.DAOUnidadMedida.ObtenerUnidadMedida(idUnidadMedida);

                    df.BeginTrans();
                    df.DAOMateriaPrima.Guardar(materiaPrima);
                    df.Commit();

                    return Json(Models.comun.JsonReturn.SuccessSinRetorno());
                }
            }
            catch(Exception ex)
            {
                return Json(Models.comun.JsonReturn.ErrorConMsjSimple("Se generó un error al intentar guardar la materia prima.<br><br>Mensaje de error: " + ex.Message));
            }
        }
    
        public IActionResult EditarMateriaPrima(long id)
        {
            UsuarioResponse usuarioResponse = 
                HttpContext.Session.Get<UsuarioResponse>("UsuarioLogueado");

            if(usuarioResponse == null)
            {
                return RedirectToAction("Index", "Home");
            }
            
            try
            {
                using(DAOFactory df = new DAOFactory())
                {
                    entities.producto.MateriaPrima materiaPrima = df.DAOMateriaPrima.ObtenerMateriaPrima(id);

                    if(materiaPrima == null)
                    {
                        return View("~/Views/Error/NotFound.cshtml");
                    }

                    ejemplo.Models.vista.ModeloVistaMateriaPrima modeloVistaMateriaPrima =
                        new Models.vista.ModeloVistaMateriaPrima {
                            Cantidad = materiaPrima.Cantidad.ToString("#0.00"),
                            Descripcion = materiaPrima.Descripcion,
                            Id = materiaPrima.Id,
                            IdUnidadMedida = materiaPrima.UnidadMedida != null ? materiaPrima.UnidadMedida.Id : 0,
                            PorcentajeGanancia = materiaPrima.PorcentajeGanancia.ToString("#0.00"),
                            Precio = materiaPrima.Precio.ToString("#0.00"),
                            UnidadMedida = materiaPrima.UnidadMedida != null ? materiaPrima.UnidadMedida.Descripcion : ""
                        };

                    return View(
                        "~/Views/Producto/MateriaPrima.cshtml", 
                        modeloVistaMateriaPrima);
                }
            }
            catch(Exception)
            {
                return View("~/Views/Error/Error.cshtml");
            }
        }        
    
    }
}
