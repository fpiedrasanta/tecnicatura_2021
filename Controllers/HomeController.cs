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

using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ejemplo.dao;
using ejemplo.entities.login;
using ejemplo.Models.vista;
using ejemplo.Models.dto.response;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;

namespace ejemplo.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(
            ILogger<HomeController> logger,
            IWebHostEnvironment webHostEnvironment)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            UsuarioResponse usuarioResponse = 
                HttpContext.Session.Get<UsuarioResponse>("UsuarioLogueado");

            if(usuarioResponse != null)
            {
                return RedirectToAction("Index", "Panel");
            }
            
            ejemplo.Models.vista.ModeloVistaLogin modeloVistaLogin = 
                new Models.vista.ModeloVistaLogin
                {
                    NombreUsuario = "",
                    TieneError = false 
                };

            return View(modeloVistaLogin);
        }

        public IActionResult Login(string nombreUsuario, string password)
        {
            try
            {
                using(DAOFactory df = new DAOFactory())
                {
                    Usuario usuario = df.DAOUsuario.ObtenerUsuario(nombreUsuario, password);
                    if(usuario != null)
                    {
                        UsuarioResponse usuarioResponse = new UsuarioResponse
                        {
                            NombreCompleto = usuario.NombreCompleto,
                            NombreUsuario = usuario.NombreUsuario
                        };
                        
                        HttpContext.Session.Set<UsuarioResponse>(
                            "UsuarioLogueado", 
                            usuarioResponse);
                        
                        return RedirectToAction("Index", "Panel");
                    }

                    ModeloVistaLogin modeloVistaLogin = new ModeloVistaLogin
                    {
                        NombreUsuario = nombreUsuario,
                        TieneError = true,
                        MensajeError = "Nombre de usuario o password incorrecto"
                    };

                    return View("~/Views/Home/Index.cshtml", modeloVistaLogin);
                }
            }
            catch(Exception exc)
            {
                ModeloVistaLogin modeloVistaLogin = new ModeloVistaLogin
                {
                    NombreUsuario = nombreUsuario,
                    TieneError = true,
                    MensajeError = exc.Message
                };

                return View("~/Views/Home/Index.cshtml", modeloVistaLogin);
            }
        }
    }
}
