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
    public class DAOUnidadMedida
    {
        private ISession session;

        public DAOUnidadMedida(ISession session)
        {
            this.session = session;
        }

        public IList<UnidadMedida> ObtenerUnidadesMedida(
            string buscarPor,
            string query,
            Ordenamiento ordenamiento)
        {
            ICriteria lista = this.session.CreateCriteria<UnidadMedida>("UnidadMedida");

            lista.Add(Restrictions.Like(buscarPor, "%" + query + "%"));

            UtilidadesNHibernate.AgregarOrdenamiento(ordenamiento, lista);
            
            IList<UnidadMedida> unidadesMedida = lista.List<UnidadMedida>();

            return unidadesMedida;
        }

        public entities.producto.UnidadMedida ObtenerUnidadMedida(long id)
		{
			try
			{
				return this.session.Get<entities.producto.UnidadMedida>(id);
			}
			catch (Exception ex)
			{
				throw new Exception("ejemplo.dao.producto.ObtenerUnidadMedida(long id): Error al obtener el item con id = " + id.ToString(), ex);
			}
		}
    }
}