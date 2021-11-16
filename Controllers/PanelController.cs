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

namespace ejemplo.Controllers
{
    public class PanelController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public PanelController(ILogger<HomeController> logger)
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
    }
}
