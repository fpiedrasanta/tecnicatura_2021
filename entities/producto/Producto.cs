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

namespace ejemplo.entities.producto
{
    public abstract class Producto
    {
        public virtual long Id { get; set; }

        public virtual string Descripcion { get; set;}

        public virtual UnidadMedida UnidadMedida { get; set; }

        public virtual double Cantidad { get; set; }

        public virtual double PorcentajeGanancia { get; set; }

        public abstract double ObtenerPrecioCompraPorUnidad();

        public abstract string ObtenerTipo();

        public virtual double ObtenerPrecioVentaPorUnidad()
        {
            //Si me cuesta $100/g (25%) $100 * (1 + 25/100) = 100 * 1.25 = $125.
            return this.ObtenerPrecioCompraPorUnidad() * (1 + this.PorcentajeGanancia/100);
        }

        public virtual double ObtenerRentabilidadPorUnidad()
        {
            return this.ObtenerPrecioVentaPorUnidad() -
                this.ObtenerPrecioCompraPorUnidad();
        }

        public virtual double ObtenerPrecioCompra(
            double cantidad, 
            UnidadMedida unidadMedida)
        {
            //$100/g => 10 k => 100 * 10 * 1000 => $100.000,00
            double precioPorUnidad = this.ObtenerPrecioCompraPorUnidad();
            return precioPorUnidad * cantidad * unidadMedida.Multiplicador;
        }

        public virtual double ObtenerPrecioVenta(
            double cantidad, 
            UnidadMedida unidadMedida)
        {
            double precioVenta = this.ObtenerPrecioVentaPorUnidad();
            return precioVenta * cantidad * unidadMedida.Multiplicador;
        }

        public virtual double ObtenerRentabilidad(
            double cantidad, 
            UnidadMedida unidadMedida)
        {
            return this.ObtenerPrecioVenta(cantidad, unidadMedida)
                - this.ObtenerPrecioCompra(cantidad, unidadMedida);
        }

        public virtual string ObtenerDescripcionUnidadMedida()
        {
            if(this.UnidadMedida == null) return "";
            return this.UnidadMedida.Descripcion;
        }
    }
}