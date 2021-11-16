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
using ejemplo.dao.comun;
using ejemplo.entities.producto;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.SqlCommand;

namespace ejemplo.dao.producto
{
    public class DAOProducto
    {
        private ISession session;

        public DAOProducto(ISession session)
        {
            this.session = session;
        }

        public IList<Producto> ObtenerProductos(
            List<AtributoBusqueda> atributosBusqueda,
            string query,
            Paginado paginado,
            Ordenamiento ordenamiento,
            List<Asociacion> asociaciones,
            out long cantidadTotal)
        {
            ICriteria lista = this.session.CreateCriteria<Producto>("Producto");
            ICriteria cantidad = this.session.CreateCriteria<Producto>("Producto");

            UtilidadesNHibernate.CrearAsociaciones(asociaciones, lista);
            UtilidadesNHibernate.CrearAsociaciones(asociaciones, cantidad);

            UtilidadesNHibernate.AgregarCriteriosDeBusqueda(atributosBusqueda, query, lista);
            UtilidadesNHibernate.AgregarCriteriosDeBusqueda(atributosBusqueda, query, cantidad);
            
            UtilidadesNHibernate.AgregarOrdenamiento(ordenamiento, lista);
            UtilidadesNHibernate.AgregarPaginado(paginado, lista);
            
            cantidadTotal = UtilidadesNHibernate.ObtenerCantidad(cantidad);

            IList<Producto> productos = lista.List<Producto>();

            return productos;
        }
    }
}