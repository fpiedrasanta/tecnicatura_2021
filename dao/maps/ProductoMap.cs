using ejemplo.entities.producto;
using FluentNHibernate.Mapping;

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

namespace ejemplo.dao.producto
{
    public class ProductoMap : ClassMap<Producto>
    {
        public ProductoMap()
        {
            Id(x => x.Id).GeneratedBy.Increment();
            
            Map(x => x.Descripcion);

            Map(x => x.Cantidad);
            
            Map(x => x.PorcentajeGanancia);
            
            References(x => x.UnidadMedida, "IdUnidadMedida");

            DiscriminateSubClassesOnColumn("discriminador")
                .AlwaysSelectWithValue()
                .Length(20)
                .Not.Nullable()
                .CustomType<string>();
        }
    }

    public class MateriaPrimaMap : SubclassMap<MateriaPrima>
    {
        public MateriaPrimaMap()
        {
            DiscriminatorValue("MateriaPrima");
            Map(x => x.Precio);
        }
    }

    public class ProductoElaboradoMap : SubclassMap<ProductoElaborado>
    {
        public ProductoElaboradoMap()
        {
            DiscriminatorValue("ProductoElaborado");
            
            HasMany(x => x.DetallesProducto)
                .KeyColumn("IdProductoElaborado")
                .Inverse();
        }
    }
}