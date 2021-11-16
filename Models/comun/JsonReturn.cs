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
using System.Collections.Generic;

namespace ejemplo.Models.comun
{
    public class JsonReturn
    {
        //0 - Todo ok. 1 - Error. 2 - Error (redireccionar)
        public short TipoError
        {
            get;
            set;
        }

        public bool Success
        {
            get;
            set;
        }

        public string MensajeError
        {
            get;
            set;
        }

        public object InnerObject
        {
            get;
            set;
        }

        public long TotalCount
        {
            get;
            set;
        }

        public static JsonReturn SuccessSinRetorno()
        {
            return new JsonReturn
            {
                InnerObject = null,
                MensajeError = "",
                Success = true,
                TipoError = 0,
                TotalCount = 0
            };
        }

        public static JsonReturn SuccessConRetorno(object obj)
        {
            return new JsonReturn
            {
                InnerObject = obj,
                MensajeError = "",
                Success = true,
                TipoError = 0,
                TotalCount = 0
            };
        }

        public static JsonReturn ErrorConMsjSimple(string msj)
        {
            return new JsonReturn
            {
                InnerObject = null,
                MensajeError = msj,
                Success = false,
                TipoError = 1,
                TotalCount = 0
            };
        }

        public static JsonReturn ErrorConLista(string title, List<string> listaErrores)
        {
            return new JsonReturn
            {
                InnerObject = listaErrores,
                MensajeError = title,
                Success = false,
                TipoError = 1,
                TotalCount = 0
            };
        }

        public static JsonReturn Redireccionar(string url)
        {
            return new JsonReturn
            {
                InnerObject = null,
                MensajeError = url,
                Success = false,
                TipoError = 2,
                TotalCount = 0
            };
        }

        public static JsonReturn RedireccionarIndex()
        {
            return new JsonReturn
            {
                InnerObject = null,
                MensajeError = "/",
                Success = false,
                TipoError = 2,
                TotalCount = 0
            };
        }

        public static JsonReturn SuccessListaPaginada(object lista, long cantidad)
        {
            return new JsonReturn
            {
                InnerObject = lista,
                MensajeError = "",
                Success = true,
                TipoError = 0,
                TotalCount = cantidad
            };
        }
    }
}