using CapaEntidad;
using CapaNegocio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace CapaPresentacionTienda.Controllers
{
    public class AccesoController : Controller
    {
        // GET: Acceso
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Registrar()
        {
            return View();
        }
        public ActionResult Restablecer()
        {
            return View();
        }
        public ActionResult CambiarClave()
        {
            return View();
        }



        [HttpPost]
        public ActionResult Registrar(Cliente objeto)
        {

            int resultado;
            string mensaje= string.Empty;

            ViewData["Nombres"] = string.IsNullOrEmpty(objeto.Nombres) ? "" : objeto.Nombres;
            ViewData["Apellidos"] = string.IsNullOrEmpty(objeto.Apellidos) ? "" : objeto.Apellidos;
            ViewData["Correo"] = string.IsNullOrEmpty(objeto.Correo) ? "" : objeto.Correo;

            if (objeto.Clave!= objeto.ConfirmarClave)
            {
                ViewBag.Error = "Las contraseñas no coinciden";
                return View();
            }
            resultado = new CN_Cliente().Registrar(objeto, out mensaje);

            if (resultado > 0)
            {
                ViewBag.Error = null;
                return RedirectToAction("Index", "Acceso");
            }
            else
            {
                ViewBag.Error = mensaje;
                return View();
            }

            
        }

        [HttpPost]
        public ActionResult Index(string correo, string clave)
        {

            Cliente oCliente = null;
            oCliente = new CN_Cliente().Listar().Where(item => item.Correo == correo && item.Clave == CN_Recursos.ConvertirSha256(clave)).FirstOrDefault();

            if (oCliente == null)
            {

                ViewBag.Error = "Correo o contraseña incorrectos";
                return View();
            }

            else {
                if (oCliente.Restablecer)

                {
                    TempData["IdCliente"] = oCliente.IdCliente;
                    return RedirectToAction("CambiarClave", "Acceso");
                }
                else
                {
                    FormsAuthentication.SetAuthCookie(oCliente.Correo, false);
                    Session["Cliente"] = oCliente;
                    ViewBag.Error=null;
                    return RedirectToAction("Index", "Tienda");

                }
            }
        }
        [HttpPost]
        public ActionResult Restablecer(string correo)
        {
            Cliente cliente = new Cliente();

            cliente = new CN_Cliente().Listar().Where(item => item.Correo == correo).FirstOrDefault();

            if (cliente == null)
            {
                ViewBag.Error = "No se encontro usuario relacionado al correo";
                return View();
            }
            string mensaje = string.Empty;
            bool respuesta = new CN_Cliente().ReestablecerClave(cliente.IdCliente, correo, out mensaje);
            if (respuesta)
            {

                ViewBag.Error = null;
                return RedirectToAction("Index", "Acceso");

            }
            else
            {
                ViewBag.Error = mensaje;
                return View();
            }
        }

        [HttpPost]
        public ActionResult CambiarClave(string idCliente, string claveactual, string nuevaclave, string confirmarclave)
        {

            Cliente oCliente = new Cliente();

            oCliente = new CN_Cliente().Listar().Where(u => u.IdCliente == int.Parse(idCliente)).FirstOrDefault();

            if (oCliente.Clave != CN_Recursos.ConvertirSha256(claveactual))
            {

                TempData["IdCliente"] = idCliente;
                ViewData["vClave"] = "";
                ViewBag.Error = "La contraseña actual no es correcta";
                return View();
            }
            else
            {
                if (nuevaclave != confirmarclave)
                {
                    TempData["IdCliente"] = idCliente;
                    ViewData["vClave"] = claveactual;
                    ViewBag.Error = "Las contraseñas no coinciden";
                    return View();
                }
            }
            ViewData["vClave"] = "";
            nuevaclave = CN_Recursos.ConvertirSha256(nuevaclave);

            string mensaje = string.Empty;

            bool respuesta = new CN_Cliente().CambiarClave(int.Parse(idCliente), nuevaclave, out mensaje);

            if (respuesta)
            {
                return RedirectToAction("Index");
            }
            else
            {
                TempData["IdCliente"] = idCliente;
                ViewBag.Error = mensaje;



                return View();

            }
        }
        public ActionResult CerrarSesion()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Acceso");
        }



    }
}