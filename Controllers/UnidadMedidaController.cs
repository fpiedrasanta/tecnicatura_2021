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

namespace ejemplo.Controllers
{
    public class UnidadMedidaController : Controller
    {
        private readonly ILogger<UnidadMedidaController> _logger;

        public UnidadMedidaController(ILogger<UnidadMedidaController> logger)
        {
            _logger = logger;
        }

        [HttpPost]
        public JsonResult ListarUnidadMedidaCombo(string consulta)
        {
            UsuarioResponse usuarioResponse = 
                HttpContext.Session.Get<UsuarioResponse>("UsuarioLogueado");

            if(usuarioResponse == null)
            {
                return Json(JsonReturn.RedireccionarIndex());
            }
            
            try
            {
                List<object> unidadesMedidaResponse = new List<object>();

                using(DAOFactory df = new DAOFactory())
                {
                    Ordenamiento ordenamiento = new Ordenamiento
                    {
                        Atributo = "Descripcion",
                        Direccion = "ASC"
                    };

                    IList<UnidadMedida> unidadesMedida = df.DAOUnidadMedida.ObtenerUnidadesMedida(
                        "Descripcion",
                        consulta,
                        ordenamiento);

                    foreach(UnidadMedida unidadMedida in unidadesMedida)
                    {
                        unidadesMedidaResponse.Add(new
                        {
                            id = unidadMedida.Id,
                            text = unidadMedida.Descripcion
                        });
                    }

                    return Json(JsonReturn.SuccessConRetorno(unidadesMedidaResponse));
                }
            }
            catch(System.Exception ex)
            {
                return Json(JsonReturn.ErrorConMsjSimple(
                    "Se generó un error mientras intentabamos listar las unidades de medida<br><br>" +
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
    }
}
