using CapaDatos;
using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio
{
    public class CN_Categoria
    {
        private CD_Categoria objCapaDato = new CD_Categoria();

        //LISTAR CATEGORIA
        public List<Categoria> Listar()
        {
            return objCapaDato.Listar();
        }
        //REGISTRAR CATEGORIA

        public int Registrar(Categoria obj, out string Mensaje)
        {
            Mensaje = string.Empty;

            if (string.IsNullOrEmpty(obj.Descripcion) || string.IsNullOrWhiteSpace(obj.Descripcion))
            {
                Mensaje = "La descripción de la categoria no puede ser vacía";
            }
            

            if (string.IsNullOrEmpty(Mensaje))
            {
                
                    return objCapaDato.Registrar(obj, out Mensaje);
            }
            else
            {
                    return 0;
            }
        }

        //EDITAR CATEGORIA
        public bool Editar(Categoria obj, out string Mensaje)
        {
            Mensaje = string.Empty;

            if (string.IsNullOrEmpty(obj.Descripcion) || string.IsNullOrWhiteSpace(obj.Descripcion))
            {
                Mensaje = "La descripción de la categoria no puede ser vacía";
            }
            if (string.IsNullOrEmpty(Mensaje))
            {
                return objCapaDato.Editar(obj, out Mensaje);
            }
            else
            {
                return false;
            }

        }


        //ELIMINAR CATEGORIA

        public bool Eliminar(int id, out string Mensaje)
        {
            return objCapaDato.Eliminar(id, out Mensaje);


        }




    }
}
