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

using System.Collections.Generic;

namespace ejemplo.entities.producto
{
    public class ProductoElaborado : Producto
    {
        public virtual IList<DetalleProducto> DetallesProducto { get; set; }

        public override double ObtenerPrecioCompraPorUnidad()
        {
            double precioCompra = 0;

            foreach(DetalleProducto detalleProducto in this.DetallesProducto)
            {
                precioCompra += detalleProducto.ObtenerPrecioCompra();
            }

            return precioCompra/(this.Cantidad * this.UnidadMedida.Multiplicador);
        }

        public override string ObtenerTipo()
        {
            return "ProductoElaborado";
        }
    }
}