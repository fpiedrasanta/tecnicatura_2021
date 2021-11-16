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
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using FluentMigrator.Runner;
using Microsoft.Extensions.DependencyInjection;
using NHibernate;

namespace ejemplo.dao
{
    public class Database
    {
        //Modificar la cadena de conexión
        private static string connectionString = 
            "Server=localhost;" +
            "Database=bd_eva_desarrollo;" +
            "Uid=root;" + 
			"Pwd=password;" +
			"SSL Mode=None;";

        #region Métodos del singleton
        private static Database instance = null;

        public static Database Instance
        {
            get
            {
                if(instance == null)
                {
                    instance = new Database();
                }

                return instance;
            }
        }

        private Database()
        {
            //Genero mi fábrica de sesiones
            this.sessionFactory = this.CreateSessionFactory();
        }
        #endregion

        #region SessionFactory, único método público
        private ISessionFactory sessionFactory = null;

        public ISessionFactory SessionFactory 
        { 
            get 
            { 
                return this.sessionFactory; 
            }
        }
        #endregion
    
        #region Método que deben cambiar si no usan MySQL
        private ISessionFactory CreateSessionFactory()
        {
            return Fluently.Configure()
                .Database(
                    MySQLConfiguration.Standard.ConnectionString(connectionString)
                )
                .Mappings(m => m.FluentMappings
                    .AddFromAssemblyOf<Database>())
                .BuildSessionFactory();
        }
        #endregion
    }
}