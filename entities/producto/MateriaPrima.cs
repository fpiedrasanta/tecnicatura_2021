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

namespace ejemplo.entities.producto
{
    public class MateriaPrima : Producto
    {
        public virtual double Precio { get; set; }

        public override double ObtenerPrecioCompraPorUnidad()
        {
            //Si cuesta $10.000,00 los 10 Kg, me devolverá:
            //$10.000/(10 * 1.000g) = $1/g (Un peso el gramo).

            return this.Precio/(this.Cantidad * this.UnidadMedida.Multiplicador);
        }

        public override string ObtenerTipo()
        {
            return "MateriaPrima";
        }
    }
}