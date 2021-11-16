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
using ejemplo.dao.login;
using ejemplo.dao.producto;
using NHibernate;

namespace ejemplo.dao
{
    public class DAOFactory : IDisposable
    {
        #region atributos privados
        private ISession session = null;
        private ITransaction transaction = null;
        #endregion
        
        #region Constructor
        public DAOFactory()
        {
            this.session = Database.Instance.SessionFactory.OpenSession();
        }
        #endregion

        #region métodos de la base de datos
        public bool BeginTrans()
        {
            try
            {
                this.transaction = this.session.BeginTransaction();
                return true;
            }
            catch(System.Exception e)
            {
                throw new System.Exception(
                    "ejemplo.dao.NHibernate.NHibernateDAOFactory.BeginTrans()", 
                    e);
            }
        }

        public bool Commit()
        {
            try
            {
                this.transaction.Commit();

                this.transaction = null;

                return true;
            }
            catch(System.Exception e)
            {
                throw new System.Exception(
                    "ejemplo.dao.NHibernate.NHibernateDAOFactory.Commit()", 
                    e);
            }
        }

        public void Rollback()
        {
            try
            {
                this.transaction.Rollback();

                this.transaction = null;
            }
            catch(System.Exception e)
            {
                throw new System.Exception("ejemplo.dao.NHibernate.NHibernateDAOFactory.Rollback()", e);
            }
        }

        public void Close()
        {
            try
            {
                if(this.transaction != null && this.transaction.IsActive)
                {
                    this.transaction.Rollback();
                }

                this.session.Close();
            }
            catch(System.Exception e)
            {
                throw new System.Exception("ejemplo.dao.NHibernate.NHibernateDAOFactory.Close()", e);
            }
        }
        
        public void Dispose()
        {
            try
            {
                this.Close();
            }
            catch(System.Exception e)
            {
                throw new System.Exception("ejemplo.dao.NHibernate.NHibernateDAOFactory.Dispose()", e);
            }
        }
        #endregion

        #region DAOs: Agregar los DAOs de ustedes
        private DAOUsuario daoUsuario = null;

        public DAOUsuario DAOUsuario 
        { 
            get 
            {
                if(this.daoUsuario == null)
                {
                    this.daoUsuario = new DAOUsuario(this.session);
                }

                return daoUsuario;
            }
        }

        private DAOProducto daoProducto = null;

        public DAOProducto DAOProducto 
        { 
            get 
            {
                if(this.daoProducto == null)
                {
                    this.daoProducto = new DAOProducto(this.session);
                }

                return daoProducto;
            }
        }
        
        private DAOUnidadMedida daoUnidadMedida = null;

        public DAOUnidadMedida DAOUnidadMedida 
        { 
            get 
            {
                if(this.daoUnidadMedida == null)
                {
                    this.daoUnidadMedida = new DAOUnidadMedida(this.session);
                }

                return daoUnidadMedida;
            }
        }

        private DAOMateriaPrima daoMateriaPrima = null;

        public DAOMateriaPrima DAOMateriaPrima 
        { 
            get 
            {
                if(this.daoMateriaPrima == null)
                {
                    this.daoMateriaPrima = new DAOMateriaPrima(this.session);
                }

                return daoMateriaPrima;
            }
        }
        #endregion
    }
}