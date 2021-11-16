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
    public class ModeloConsultaGrilla
    {
        public int draw { get; set; }
        public int start { get; set; }
        public int length { get; set; }
        public ModeloBusquedaPorColumna search { get; set; }
        public List<ModeloOrderBy> order { get; set; }
        public List<ModeloColumna> columns { get; set; }
    }

    public enum ModeloDireccion
    {
        asc,
        desc
    }

    public class ModeloOrderBy
    {
        public int column { get; set; }
        public ModeloDireccion dir { get; set; }
    }

    public class ModeloBusquedaPorColumna
    {
        public string value { get; set; }
        public string regex { get; set; }
    }

    public class ModeloColumna
    {
        public string data { get; set; }
        public string name { get; set; }
        public Boolean sercheable { get; set; }
        public Boolean orderable { get; set; }
        public ModeloBusquedaPorColumna search { get; set; }
    }
}