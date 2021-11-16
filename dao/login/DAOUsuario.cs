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

using NHibernate;
using NHibernate.Criterion;
using System.Collections.Generic;

using ejemplo.entities.login;

namespace ejemplo.dao.login
{
    public class DAOUsuario
    {
        private ISession session;

        public DAOUsuario(ISession session)
        {
            this.session = session;
        }

        public Usuario ObtenerUsuario(string nombreUsuario, string password)
        {
            ICriteria criterio = this.session.CreateCriteria<Usuario>();

            criterio.Add(Restrictions.Eq("NombreUsuario", nombreUsuario));
            criterio.Add(Restrictions.Eq("Password", password));

            IList<Usuario> usuarios = criterio.List<Usuario>();

            if(usuarios != null && usuarios.Count > 0) return usuarios[0];

            return null;
        }
    }
}